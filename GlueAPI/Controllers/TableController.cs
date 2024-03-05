using Amazon.Appflow.Model;
using Amazon.Glue.Model;
using GlueAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlueAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TableController
    {
        private readonly IAllService _allService;
        public TableController(IAllService allService)
        {
            _allService = allService;
        }

        [HttpDelete]
        public bool DeleteTable(string dbName, string tableName)
        {
            var response = _allService.DeleteTableAsync(dbName, tableName);
            return response.Result;
        }

        [HttpGet]
        public List<Table> GetTable(string dbName)
        {
            var tableInfo = _allService.GetTablesAsync(dbName);
            return tableInfo.Result;
        }

        [HttpPost]
        public bool CreateTable(
                       string dbName,
                                  string tableName,
                                             string description,
                                                        string inputFormat,
                                                                   string outputFormat,
                                                                              string location)
        {
            var response = _allService.CreateTableAsync(
                               dbName,
                                              tableName,
                                                             description,
                                                                            inputFormat,
                                                                                           outputFormat,
                                                                                                          location);
            return response.Result;
        }
    }
}
