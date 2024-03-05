using Amazon.Glue;
using Amazon.Glue.Model;
using GlueAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlueAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DatabaseController
    {
        private readonly IAllService _allService;
        public DatabaseController(IAllService allService)
        {
            _allService = allService;
        }

        [HttpGet]
        public Database GetDatabase(string name)
        {
            var databaseInfo = _allService.GetDatabaseAsync(name);
            return databaseInfo.Result;
        }

        [HttpGet]
        public async Task<GetDatabasesResponse> GetListDatabase()
        {
            var client = new AmazonGlueClient();
            var request = new GetDatabasesRequest();
            var response = await client.GetDatabasesAsync(request);
            return response;
        }

        [HttpDelete]
        public async Task<bool> DeleteDatabase(string dbName)
        {
            var response = await _allService.DeleteDatabaseAsync(dbName);
            return response;
        }

        [HttpPost]
        public async Task<bool> CreateDatabase(string dbName, string description)
        {
            var response = await _allService.CreateDatabaseAsync(dbName, description);
            return response;
        }
    }
}
