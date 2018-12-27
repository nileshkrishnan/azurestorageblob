using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;

namespace BlobAndAdlsPerfoamance
{
    class Program
    {
        static DateTime _startTime = DateTime.MinValue;
        static DateTime _endTime = DateTime.MaxValue;
        static string _baseAddressUri = "http://localhost:64372";
        static int count = 5;

        static void Main(string[] args)
        {

            WriteToBlobAsync().GetAwaiter().GetResult();

            Console.WriteLine("Completed Blob Write         ");
            Console.ReadLine();

            DummyModel blobContentReceived = ReadFromBlobAsync().GetAwaiter().GetResult();

            Console.WriteLine("Completed Blob Read       content received is \n   " + Encoding.ASCII.GetString(blobContentReceived.Data));

            Console.ReadLine();
        }

        public static async Task WriteToBlobAsync()
        {
            await WriteContentAsync("api/values/Blob");
        }

        public static async Task<DummyModel> ReadFromBlobAsync()
        {
           return await ReadContentAsync("api/values/Blob/1");
        }

        public static async Task WriteContentAsync(string Url)
        {

            byte[] bytesFromString = Encoding.ASCII.GetBytes("Hello from Nilesh");

            //byte[] bytesFromFile = System.IO.File.ReadAllBytes(@"C:\OffShore\Poc\BlobSample\Nilesh_Photo.JPG");

            

            var client = new HttpClient
            {
                BaseAddress = new Uri(_baseAddressUri)
            };

            client.DefaultRequestHeaders.Accept.Clear();

            var request = new DummyModel
            {
                FileName = "From String",
                Data = bytesFromString
                //Data = bytesFromFile
            };

            string serializedContent = JsonConvert.SerializeObject(request);
            HttpContent latestContent = new StringContent(serializedContent, Encoding.UTF8, "application/json");
            _startTime = DateTime.Now;

            Console.WriteLine("Starting " + Url + " Write " +_startTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            Task result = client.PostAsync(Url, latestContent);

            await result;
        }

        public static async Task<DummyModel> ReadContentAsync(string Url)
        {

            var client = new HttpClient
            {
                BaseAddress = new Uri(_baseAddressUri)
            };

            client.DefaultRequestHeaders.Accept.Clear();
            _startTime = DateTime.Now;

            Console.WriteLine("Starting "+Url+" Read " + _startTime.ToString("yyyy-MM-dd HH:mm:ss.fff"));

            var result = await client.GetAsync(Url);
            object x = null;

            if (result.IsSuccessStatusCode)
            {
                x = await result.Content.ReadAsAsync(typeof(DummyModel));
            }
            return (DummyModel)x;
        }

        public class DummyModel
        {
            public byte[] Data { get; set; }

            public string FileName { get; set; }
        }
        static byte[] PrepareContentToWrite()
        {

            throw new NotImplementedException();
        }
    }

}
