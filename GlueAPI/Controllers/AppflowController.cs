using Amazon.Appflow;
using Amazon.Appflow.Model;
using GlueAPI.Model.DTOs;
using GlueAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GlueAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AppflowController
    {
        private readonly IAppflowServices _appflowServices;
        public AppflowController(IAppflowServices appflowServices)
        {
            _appflowServices = appflowServices;
        }

        /// <summary>
        /// Mô tả về connector
        /// </summary>
        /// <param name="connectorType"></param>
        /// <param name="connectorLabel"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<DescribeConnectorResponse> DescribeConnector(string? connectorType, string? connectorLabel)
        {
            var response = await _appflowServices.DescribeConnector(connectorType, connectorLabel);
            return response;
        }

        /// <summary>
        /// Lấy về các trường của application business
        /// </summary>
        /// <param name="databaseSource">Connection Name</param>
        /// <param name="entityName">Object</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<DescribeConnectorEntityResponse> DescribeConnectorEntity(
            string apiVersion,
            string connectorEntityName,
            string connectorProfileName,
            ConnectorType connectorType)
        {
            var response = await _appflowServices.DescribeConnectorEntity(connectorEntityName, connectorProfileName, apiVersion, connectorType);
            return response;
        }

        [HttpPost]
        public async Task<DescribeConnectorProfilesResponse> DescribeConnectorProfiles([FromBody]DescribeConnectorProfilesRequestDto requestDto)
        {
            var response = await _appflowServices.DescribeConnectorProfiles(requestDto.ConnectorLabel, requestDto.ConnectorProfileNames, requestDto.ConnectorType);
            return response;
        }

        /// <summary>
        /// Lấy về danh sách connector theo loại
        /// </summary>
        /// <param name="connectorTypes">Loại: Custom Connector,...</param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DescribeConnectorsResponse> DescribeConnectors(List<string> connectorTypes, int maxResults)
        {
            var response = await _appflowServices.DescribeConnectors(connectorTypes, maxResults);
            return response;
        }

        /// <summary>
        /// Mô tả kết nối
        /// </summary>
        /// <param name="flowName"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DescribeFlowResponse> DescribeFlow(string flowName)
        {
            var response = await _appflowServices.DescribeFlow(flowName);
            return response;
        }

        /// <summary>
        /// Mô tả các trạng thái thực thi của flow (thành công hay thất bại)
        /// </summary>
        /// <param name="flowName"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DescribeFlowExecutionRecordsResponse> DescribeFlowExecutionRecords(string flowName, int maxResults)
        {
            var response = await _appflowServices.DescribeFlowExecutionRecords(flowName, maxResults);
            return response;
        }

        [HttpPost]
        public async Task<ListConnectorEntitiesResponse> ListConnectorEntities(
            string apiVersion,
            string? connectorProfileName,
            string? connectorType,
            string? entitiesPath,
            int maxResults)
        {
            var response = await _appflowServices.ListConnectorEntities(
                apiVersion,
                connectorProfileName,
                connectorType,
                entitiesPath,
                maxResults);
            return response;
        }


        /// <summary>
        /// Lấy về danh sách tất cả connector
        /// </summary>
        /// <param name="maxResult"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ListConnectorsResponse> ListConnectors(int maxResult)
        {
            var response = await _appflowServices.ListConnectors(maxResult);
            return response;
        }

        /// <summary>
        /// Lấy về danh sách tất cả flow
        /// </summary>
        /// <param name="maxResult"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ListFlowsResponse> ListFlows(int maxResult)
        {
            var response = await _appflowServices.ListFlows(maxResult);
            return response;
        }

        /// <summary>
        /// Lấy về danh sách tag của resource
        /// </summary>
        /// <param name="resourceArn"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ListTagsForResourceResponse> ListTagsForResource(string resourceArn)
        {
            var response = await _appflowServices.ListTagsForResource(resourceArn);
            return response;
        }
    }
}
