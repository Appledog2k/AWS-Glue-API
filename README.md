# AWS-Glue-API (ASP.NET Core 8)

API .NET 8 cung cấp các endpoint thao tác AWS Glue (Database, Table, Crawler, Job) và Amazon AppFlow (Connectors, Flows, Entities) qua AWS SDK for .NET.

## Tổng quan
- Framework: ASP.NET Core 8 Web API, Swagger/OpenAPI tích hợp.
- AWS SDK: `AWSSDK.Glue`, `AWSSDK.Appflow`, DI thông qua `AddAWSService<T>`.
- Chức năng chính:
  - Glue: Database, Table, Crawler (tạo/xoá/bắt đầu/đọc trạng thái), Job (tạo/chạy/xoá/liệt kê run).
  - AppFlow: Mô tả connector, entity, profiles, connectors, flows, tags.
- Cảnh báo: Nhiều endpoint có thể thay đổi/xoá tài nguyên (Delete*). Hãy dùng account/test env.

## Cấu trúc
```
AWS-Glue-API/
  GlueAPI.sln
  GlueAPI/
    Program.cs
    appsettings.json
    Controllers/
      DatabaseController.cs
      TableController.cs
      CrawlerController.cs
      JobController.cs
      AppflowController.cs
    Services/
      AllServices.cs (Glue)
      AppflowServices.cs (AppFlow)
      IAllService.cs, IAppflowServices.cs
    Properties/launchSettings.json (port mặc định)
    Docs/amazon-appflow-api.pdf
    Images/*.png
```

## Yêu cầu
- .NET SDK 8.0
- Tài khoản AWS + quyền Glue/AppFlow phù hợp (IAM).
- AWS CLI (khuyến nghị) để cấu hình profile.

## Cấu hình AWS credentials/region
API dùng default credential chain của AWS SDK.
- Dùng profile:
```powershell
$env:AWS_PROFILE = "default"     # hoặc tên profile của bạn
$env:AWS_REGION = "ap-southeast-1"  # hoặc region bạn muốn
```
- Hoặc thiết lập trực tiếp biến môi trường:
```powershell
$env:AWS_ACCESS_KEY_ID = "..."
$env:AWS_SECRET_ACCESS_KEY = "..."
$env:AWS_SESSION_TOKEN = "..."    # nếu dùng tạm thời
$env:AWS_REGION = "ap-southeast-1"
```

## Chạy ứng dụng
```powershell
cd c:\workspace\AWS-Glue-API\GlueAPI
dotnet restore
dotnet run
```
- Swagger UI mở tại: `http://localhost:5150/swagger` (theo `launchSettings.json`).
- HTTPS: `https://localhost:7292/swagger`.

## Endpoints chính (tóm tắt)
Các route đều ở dạng `[controller]/[action]` trừ khi ghi khác. Tham khảo Swagger để xem schema tham số đầy đủ.

### DatabaseController
- GET `Database/GetDatabase?name={db}`: Lấy thông tin database.
- GET `Database/GetListDatabase`: Liệt kê databases.
- POST `Database/CreateDatabase?dbName={name}&description={desc}`: Tạo database.
- DELETE `Database/DeleteDatabase?dbName={name}`: Xoá database.

### TableController
- GET `Table/GetTable?dbName={db}`: Liệt kê bảng trong database.
- POST `Table/CreateTable?dbName={db}&tableName={t}&description={d}&inputFormat={if}&outputFormat={of}&location={s3Url}`: Tạo bảng (StorageDescriptor đơn giản).
- DELETE `Table/DeleteTable?dbName={db}&tableName={t}`: Xoá bảng.

### CrawlerController
- GET `Crawler/GetCrawler?crawlerName={name}`: Lấy thông tin crawler.
- GET `Crawler/GetListCrawler`: Liệt kê crawlers.
- GET `Crawler/GetCrawlerState?crawlerName={name}`: Trạng thái crawler (Running/Ready...).
- GET `Crawler/GetCrawlerLog?crawlerName={name}`: Thông tin/metrices cơ bản.
- POST `Crawler/CreateCrawler?crawlerName=&crawlerDescription=&role=&schedule=&s3Path=&dbName=`: Tạo crawler target S3.
- POST `Crawler/StartCrawler?crawlerName={name}`: Chạy crawler.
- DELETE `Crawler/DeleteCrawler?crawlerName={name}`: Xoá crawler.

### JobController
- GET `Job/GetJob?jobName={name}`: Lấy thông tin job.
- GET `Job/GetJobRun?jobName={name}&jobRunId={id}`: Lấy thông tin 1 job run.
- GET `Job/GetJobRuns?jobName={name}`: Liệt kê job runs.
- GET `Job/ListJobs`: Liệt kê job names.
- POST `Job/CreateJob?dbName=&tableName=&bucketUrl=&jobName=&roleName=&description=&scriptUrl=`: Tạo Glue job (glueetl, Python 3).
- POST `Job/StartJobRun?jobName=&inputDatabase=&inputTable=&bucketName=`: Chạy job với arguments.
- DELETE `Job/DeleteJob?jobName={name}`: Xoá job.

### AppflowController
- GET `Appflow/DescribeConnector?connectorType=&connectorLabel=`
- GET `Appflow/DescribeConnectorEntity?apiVersion=&connectorEntityName=&connectorProfileName=&connectorType=`
- POST `Appflow/DescribeConnectorProfiles` (body: `DescribeConnectorProfilesRequestDto`)
- POST `Appflow/DescribeConnectors` (body: `connectorTypes: string[]`, `maxResults`)
- POST `Appflow/DescribeFlow?flowName=`
- POST `Appflow/DescribeFlowExecutionRecords?flowName=&maxResults=`
- POST `Appflow/ListConnectorEntities?apiVersion=&connectorProfileName=&connectorType=&entitiesPath=&maxResults=`
- POST `Appflow/ListConnectors?maxResult=`
- POST `Appflow/ListFlows?maxResult=`
- GET `Appflow/ListTagsForResource?resourceArn=`

## Ví dụ nhanh (PowerShell)
```powershell
# Cấu hình AWS profile/region
$env:AWS_PROFILE = "default"; $env:AWS_REGION = "ap-southeast-1"

# Tạo database
Invoke-RestMethod -Method POST "http://localhost:5150/Database/CreateDatabase?dbName=demo_db&description=demo"

# Tạo crawler target S3
Invoke-RestMethod -Method POST "http://localhost:5150/Crawler/CreateCrawler?crawlerName=demo_crawler&crawlerDescription=demo&role=arn:aws:iam::123456789012:role/GlueRole&schedule=cron(0/30 * * * ? *)&s3Path=s3://my-bucket/path/&dbName=demo_db"

# Chạy crawler
Invoke-RestMethod -Method POST "http://localhost:5150/Crawler/StartCrawler?crawlerName=demo_crawler"

# Liệt kê bảng
Invoke-RestMethod -Method GET "http://localhost:5150/Table/GetTable?dbName=demo_db"
```

## Ghi chú bảo mật & quyền
- Cần IAM role/permissions cho Glue, AppFlow, S3, CloudWatch Logs (tuỳ tác vụ).
- Endpoint Delete* có rủi ro phá huỷ dữ liệu; không dùng trên production nếu chưa kiểm soát.
- API không bật xác thực ứng dụng; triển khai thực tế nên dùng auth (JWT/Key) hoặc network ACL.

## Phát triển
```powershell
# Chạy test thủ công qua Swagger
cd c:\workspace\AWS-Glue-API\GlueAPI
dotnet run
start http://localhost:5150/swagger
```

## Tài liệu tham khảo
- AWS Glue .NET SDK
- Amazon AppFlow .NET SDK
- amazon-appflow-api.pdf (Docs trong repo)
