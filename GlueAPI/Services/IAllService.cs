using Amazon.Glue.Model;

namespace GlueAPI.Services
{
    public interface IAllService
    {
        Task<Database> GetDatabaseAsync(string dbName);
        Task<Crawler?> GetCrawlerAsync(string crawlerName);
        Task<bool> DeleteDatabaseAsync(string dbName);
        Task<bool> CreateCrawlerAsync(string crawlerName, string crawlerDescription, string role, string schedule, string s3Path, string dbName);
        Task<bool> DeleteCrawlerAsync(string crawlerName);
        Task<bool> StartCrawlerAsync(string crawlerName);
        Task<JobRun> GetJobRunAsync(string jobName, string jobRunId);
        Task<Job> GetJobAsync(string jobName);
        Task<List<JobRun>> GetJobRunsAsync(string jobName);
        Task<List<string>> ListJobsAsync();
        Task<bool> DeleteTableAsync(string dbName, string tableName);
        Task<List<Table>> GetTablesAsync(string dbName);
        Task<string> StartJobRunAsync(string jobName, string inputDatabase, string inputTable, string bucketName);
        Task<bool> CreateJobAsync(string dbName, string tableName, string bucketUrl, string jobName, string roleName, string description, string scriptUrl);
        Task<bool> DeleteJobAsync(string jobName);
    }
}
