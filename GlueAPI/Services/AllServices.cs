using Amazon.Appflow;
using Amazon.Appflow.Model;
using Amazon.Glue;
using Amazon.Glue.Model;
using System.Net;

namespace GlueAPI.Services
{
    public class AllServices : IAllService
    {
        private readonly IAmazonGlue _amazonGlue;
        public AllServices(IAmazonGlue amazonGlue)
        {
            _amazonGlue = amazonGlue;
        }

        public async Task<Database> GetDatabaseAsync(string dbName)
        {
            var databasesRequest = new GetDatabaseRequest
            {
                Name = dbName,
            };

            var response = await _amazonGlue.GetDatabaseAsync(databasesRequest);
            return response.Database;
        }

        public async Task<Crawler?> GetCrawlerAsync(string crawlerName)
        {
            var crawlerRequest = new GetCrawlerRequest
            {
                Name = crawlerName,
            };

            var response = await _amazonGlue.GetCrawlerAsync(crawlerRequest);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var databaseName = response.Crawler.DatabaseName;
                Console.WriteLine($"{crawlerName} has the database {databaseName}");
                return response.Crawler;
            }

            Console.WriteLine($"No information regarding {crawlerName} could be found.");
            return null;
        }

        public async Task<bool> DeleteDatabaseAsync(string dbName)
        {
            var response = await _amazonGlue.DeleteDatabaseAsync(new DeleteDatabaseRequest { Name = dbName });
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Create an AWS Glue crawler.
        /// </summary>
        /// <param name="crawlerName">The name for the crawler.</param>
        /// <param name="crawlerDescription">A description of the crawler.</param>
        /// <param name="role">The AWS Identity and Access Management (IAM) role to
        /// be assumed by the crawler.</param>
        /// <param name="schedule">The schedule on which the crawler will be executed.</param>
        /// <param name="s3Path">The path to the Amazon Simple Storage Service (Amazon S3)
        /// bucket where the Python script has been stored.</param>
        /// <param name="dbName">The name to use for the database that will be
        /// created by the crawler.</param>
        /// <returns>A Boolean value indicating the success of the action.</returns>
        public async Task<bool> CreateCrawlerAsync(
            string crawlerName,
            string crawlerDescription,
            string role,
            string schedule,
            string s3Path,
            string dbName)
        {
            var s3Target = new S3Target
            {
                Path = s3Path,
            };

            var targetList = new List<S3Target>
            {
            s3Target,
            };

            var targets = new CrawlerTargets
            {
                S3Targets = targetList,
            };

            var crawlerRequest = new CreateCrawlerRequest
            {
                DatabaseName = dbName,
                Name = crawlerName,
                Description = crawlerDescription,
                Targets = targets,
                Role = role,
                Schedule = schedule,
            };

            var response = await _amazonGlue.CreateCrawlerAsync(crawlerRequest);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> DeleteCrawlerAsync(string crawlerName)
        {
            var response = await _amazonGlue.DeleteCrawlerAsync(new DeleteCrawlerRequest { Name = crawlerName });
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<JobRun> GetJobRunAsync(string jobName, string jobRunId)
        {
            var response = await _amazonGlue.GetJobRunAsync(new GetJobRunRequest { JobName = jobName, RunId = jobRunId });
            return response.JobRun;
        }

        public async Task<Job> GetJobAsync(string jobName)
        {
            var jobRequest = new GetJobRequest
            {
                JobName = jobName,
            };

            var response = await _amazonGlue.GetJobAsync(jobRequest);
            return response.Job;
        }

        /// <summary>
        /// Get information about all AWS Glue runs of a specific job.
        /// </summary>
        /// <param name="jobName">The name of the job.</param>
        /// <returns>A list of JobRun objects.</returns>
        public async Task<List<JobRun>> GetJobRunsAsync(string jobName)
        {
            var jobRuns = new List<JobRun>();

            var request = new GetJobRunsRequest
            {
                JobName = jobName,
            };

            // No need to loop to get all the log groups--the SDK does it for us behind the scenes
            var paginatorForJobRuns =
                _amazonGlue.Paginators.GetJobRuns(request);

            await foreach (var response in paginatorForJobRuns.Responses)
            {
                response.JobRuns.ForEach(jobRun =>
                {
                    jobRuns.Add(jobRun);
                });
            }

            return jobRuns;
        }

        /// <summary>
        /// List AWS Glue jobs using a paginator.
        /// </summary>
        /// <returns>A list of AWS Glue job names.</returns>
        public async Task<List<string>> ListJobsAsync()
        {
            var jobNames = new List<string>();

            var listJobsPaginator = _amazonGlue.Paginators.ListJobs(new ListJobsRequest { MaxResults = 10 });
            await foreach (var response in listJobsPaginator.Responses)
            {
                jobNames.AddRange(response.JobNames);
            }

            return jobNames;
        }

        /// <summary>
        /// Start an AWS Glue crawler.
        /// </summary>
        /// <param name="crawlerName">The name of the crawler.</param>
        /// <returns>A Boolean value indicating the success of the action.</returns>
        public async Task<bool> StartCrawlerAsync(string crawlerName)
        {
            var crawlerRequest = new StartCrawlerRequest
            {
                Name = crawlerName,
            };

            var response = await _amazonGlue.StartCrawlerAsync(crawlerRequest);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete a table from an AWS Glue database.
        /// </summary>
        /// <param name="tableName">The table to delete.</param>
        /// <returns>A Boolean value indicating the success of the action.</returns>
        public async Task<bool> DeleteTableAsync(string dbName, string tableName)
        {
            var response = await _amazonGlue.DeleteTableAsync(new DeleteTableRequest { Name = tableName, DatabaseName = dbName });
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Get a list of tables for an AWS Glue database.
        /// </summary>
        /// <param name="dbName">The name of the database.</param>
        /// <returns>A list of Table objects.</returns>
        public async Task<List<Table>> GetTablesAsync(string dbName)
        {
            var request = new GetTablesRequest { DatabaseName = dbName };
            var tables = new List<Table>();

            // Get a paginator for listing the tables.
            var tablePaginator = _amazonGlue.Paginators.GetTables(request);

            await foreach (var response in tablePaginator.Responses)
            {
                tables.AddRange(response.TableList);
            }

            return tables;
        }

        /// <summary>
        /// Start an AWS Glue job run.
        /// </summary>
        /// <param name="jobName">The name of the job.</param>
        /// <returns>A string representing the job run Id.</returns>
        public async Task<string> StartJobRunAsync(
            string jobName,
            string inputDatabase,
            string inputTable,
            string bucketName)
        {
            var request = new StartJobRunRequest
            {
                JobName = jobName,
                Arguments = new Dictionary<string, string>
            {
                {"--input_database", inputDatabase},
                {"--input_table", inputTable},
                {"--output_bucket_url", $"s3://{bucketName}/"}
            }
            };

            var response = await _amazonGlue.StartJobRunAsync(request);
            return response.JobRunId;
        }

        /// <summary>
        /// Create an AWS Glue job.
        /// </summary>
        /// <param name="jobName">The name of the job.</param>
        /// <param name="roleName">The name of the IAM role to be assumed by
        /// the job.</param>
        /// <param name="description">A description of the job.</param>
        /// <param name="scriptUrl">The URL to the script.</param>
        /// <returns>A Boolean value indicating the success of the action.</returns>
        public async Task<bool> CreateJobAsync(string dbName,
            string tableName,
            string bucketUrl,
            string jobName,
            string roleName, 
            string description, 
            string scriptUrl)
        {
            var command = new JobCommand
            {
                PythonVersion = "3",
                Name = "glueetl",
                ScriptLocation = scriptUrl,
            };

            var arguments = new Dictionary<string, string>
            {
                { "--input_database", dbName },
                { "--input_table", tableName },
                { "--output_bucket_url", bucketUrl }
            };

            var request = new CreateJobRequest
            {
                Command = command,
                DefaultArguments = arguments,
                Description = description,
                GlueVersion = "3.0",
                Name = jobName,
                NumberOfWorkers = 10,
                Role = roleName,
                WorkerType = "G.1X"
            };

            var response = await _amazonGlue.CreateJobAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete an AWS Glue job.
        /// </summary>
        /// <param name="jobName">The name of the job.</param>
        /// <returns>A Boolean value indicating the success of the action.</returns>
        public async Task<bool> DeleteJobAsync(string jobName)
        {
            var response = await _amazonGlue.DeleteJobAsync(new DeleteJobRequest { JobName = jobName });
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> CreateDatabaseAsync(string dbName, string description)
        {
            var request = new CreateDatabaseRequest
            {
                DatabaseInput = new DatabaseInput
                {
                    Name = dbName,
                    Description = description
                }
            };

            var response = await _amazonGlue.CreateDatabaseAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        // Get State of Crawler (Running, Ready, etc.)
        public async Task<string> GetCrawlerStateAsync(string crawlerName)
        {
            var response = await _amazonGlue.GetCrawlerAsync(new GetCrawlerRequest { Name = crawlerName });
            return response.Crawler.State;
        }

        // Get List Crawler
        public async Task<List<Crawler>> GetListCrawlerAsync()
        {
            var response = await _amazonGlue.GetCrawlersAsync(new GetCrawlersRequest());
            return response.Crawlers;
        }

        // Create Table in Database
        public async Task<bool> CreateTableAsync(string dbName, string tableName, string description, string inputFormat, string outputFormat, string location)
        {
            var request = new CreateTableRequest
            {
                DatabaseName = dbName,
                TableInput = new TableInput
                {
                    Name = tableName,
                    Description = description,
                    StorageDescriptor = new StorageDescriptor
                    {
                        InputFormat = inputFormat,
                        OutputFormat = outputFormat,
                        Location = location
                    }
                }
            };

            var response = await _amazonGlue.CreateTableAsync(request);
            return response.HttpStatusCode == HttpStatusCode.OK;
        }

        // Get log from crawler
        public async Task<string> GetCrawlerLogAsync(string crawlerName)
        {
            var response = await _amazonGlue.GetCrawlerMetricsAsync(new GetCrawlerMetricsRequest { CrawlerNameList = new List<string> { crawlerName } });
            return response.CrawlerMetricsList[0].CrawlerName;
        }
    }
}
