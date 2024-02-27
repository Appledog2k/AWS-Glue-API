using Amazon.Glue.Model;
using GlueAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlueAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class JobController
    {
        private readonly IAllService _allService;
        public JobController(IAllService allService)
        {
            _allService = allService;
        }

        [HttpGet]
        public JobRun GetJobRun(string jobName, string jobRunId)
        {
            var jobRunInfo = _allService.GetJobRunAsync(jobName, jobRunId);
            return jobRunInfo.Result;
        }

        [HttpGet]
        public Job GetJob(string jobName)
        {
            var jobInfo = _allService.GetJobAsync(jobName);
            return jobInfo.Result;
        }

        [HttpGet]
        public List<JobRun> GetJobRuns(string jobName)
        {
            var jobRunsInfo = _allService.GetJobRunsAsync(jobName);
            return jobRunsInfo.Result;
        }

        [HttpGet]
        public List<string> ListJobs()
        {
            var jobsInfo = _allService.ListJobsAsync();
            return jobsInfo.Result;
        }

        [HttpPost]
        public string StartJobRun(string jobName, string inputDatabase, string inputTable, string bucketName)
        {
            var response = _allService.StartJobRunAsync(jobName, inputDatabase, inputTable, bucketName);
            return response.Result;
        }

        [HttpPost]
        public bool CreateJob(
                       string dbName,
                       string tableName,
                       string bucketUrl,
                       string jobName,
                       string roleName,
                       string description,
                       string scriptUrl)
        {
            var response = _allService.CreateJobAsync(
                               dbName,
                               tableName,
                               bucketUrl,
                               jobName,
                               roleName,
                               description,
                               scriptUrl);
            return response.Result;
        }

        [HttpDelete]
        public bool DeleteJob(string jobName)
        {
            var response = _allService.DeleteJobAsync(jobName);
            return response.Result;
        }
    }
}
