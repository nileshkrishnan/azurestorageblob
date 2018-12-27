using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlobHostedInWeb.BlobHandler;
using Microsoft.AspNetCore.Mvc;

namespace BlobHostedInWeb.Controllers
{
    //[ApiController]
    public class BlobController : ControllerBase
    {
        //[Route("api/values")]
        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        [Route("api/values/Blob/{count}")]
        [HttpGet]
        public async Task<DummyModel> Get(int count)
        {
            DummyModel response=await new BlobProcesser().ReadDocFromBlobAsync();

            return response;
        }


        [Route("api/values/Blob")]
        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] DummyModel dummyModel)
        {
            //Writing single file with same connection
             await new BlobProcesser().WriteDocToBlobAsync(dummyModel.Data, dummyModel.Data.Length);

        }
    }
}
