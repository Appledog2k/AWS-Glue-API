using Amazon.Glue.Model;
using Amazon.Runtime;
using GlueAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlueAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CrawlerController
    {
        private readonly IAllService _allService;
        public CrawlerController(IAllService allService)
        {
            _allService = allService;
        }

        [HttpGet]
        public Crawler GetCrawler(string crawlerName)
        {
            var crawlerInfo = _allService.GetCrawlerAsync(crawlerName);
            return crawlerInfo.Result;
        }

        [HttpPost]
        public bool CreateCrawler(
            string crawlerName,
            string crawlerDescription,
            string role,
            string schedule,
            string s3Path,
            string dbName)
        {
            var response = _allService.CreateCrawlerAsync(
                crawlerName, 
                crawlerDescription,
                role, 
                schedule,
                s3Path,
                dbName);
            return response.Result;
        }

        [HttpDelete]
        public bool DeleteCrawler(string crawlerName)
        {
            var response = _allService.DeleteCrawlerAsync(crawlerName);
            return response.Result;
        }

        [HttpPost]
        public bool StartCrawler(string crawlerName)
        {
            var response = _allService.StartCrawlerAsync(crawlerName);
            return response.Result;
        }

        [HttpGet]
        public string GetCrawlerState(string crawlerName)
        {
            var response = _allService.GetCrawlerStateAsync(crawlerName);
            return response.Result;
        }

        [HttpGet]
        public List<Crawler> GetListCrawler()
        {
            var response = _allService.GetListCrawlerAsync();
            return response.Result;
        }

        [HttpGet]
        public string GetCrawlerLog(string crawlerName)
        {
            var response = _allService.GetCrawlerLogAsync(crawlerName);
            return response.Result;
        }
    }
}
