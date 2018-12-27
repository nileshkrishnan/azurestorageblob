using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlobHostedInWeb.BlobHandler
{
    public class BlobProcesser
    {
        CloudStorageAccount myStorageAccount = null;
        CloudBlobContainer myContainer = null;
        CloudBlobClient blobClient = null;

        string _generalStorageConString = "";
        string _premiumStorageConString = "";
        string _storageConString = "";
        bool _isPremium = false;


        public BlobProcesser(bool isPremium = false)
        {
            _isPremium = isPremium;

            _storageConString = !_isPremium ? _generalStorageConString : _premiumStorageConString;
        }

        public async Task WriteDocToBlobAsync(byte[] docBytes, int count)
        {
            if (CloudStorageAccount.TryParse(_storageConString, out myStorageAccount))
            {
                try
                {
                    CloudBlobClient blobClient = myStorageAccount.CreateCloudBlobClient();

                    myContainer = blobClient.GetContainerReference("nileshdemoblobs");

                    await myContainer.CreateIfNotExistsAsync(BlobContainerPublicAccessType.Blob, new BlobRequestOptions(), new OperationContext());


                    CloudBlockBlob blockBlob = myContainer.GetBlockBlobReference("fileFromService");

                    await blockBlob.UploadFromByteArrayAsync(docBytes, 0, count);
                    //blockBlob.u
                }
                catch (StorageException stex)
                {

                    throw;
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {

                }
            }
        }

        public async Task<DummyModel> ReadDocFromBlobAsync()
        {
            DummyModel outputModel = new DummyModel();

            if (CloudStorageAccount.TryParse(_storageConString, out myStorageAccount))
            {
                try
                {
                    CloudBlobClient blobClient = myStorageAccount.CreateCloudBlobClient();

                    myContainer = blobClient.GetContainerReference("nileshdemoblobs");

                    CloudBlockBlob blockBlob = myContainer.GetBlockBlobReference("fileFromService");

                    Stream fileContent = await blockBlob.OpenReadAsync();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        fileContent.CopyTo(ms);

                        outputModel.Data = ms.ToArray();
                    }

                    outputModel.FileName = "fileFromService";


                }
                catch (StorageException stex)
                {

                    throw;
                }
                catch (Exception ex)
                {

                }
                finally
                {

                }
            }
            return outputModel;
        }
    }
}
