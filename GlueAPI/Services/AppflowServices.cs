using Amazon.Appflow.Model;
using Amazon.Appflow;
using System.Runtime.CompilerServices;

namespace GlueAPI.Services
{
    public class AppflowServices : IAppflowServices
    {
        private readonly IAmazonAppflow _amazonAppflow;
        public AppflowServices(IAmazonAppflow amazonAppflow)
        {
            _amazonAppflow = amazonAppflow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectorType">Kiểu kết nối: custom hay không ?</param>
        /// <param name="connectorLabel">Tên kết nôi</param>
        /// <returns></returns>
        public async Task<DescribeConnectorResponse> DescribeConnector(string connectorType, string connectorLabel)
        {
            var request = new DescribeConnectorRequest
            {
                ConnectorType = connectorType,
                ConnectorLabel = connectorLabel
            };
            var response = await _amazonAppflow.DescribeConnectorAsync(request);
            return response;
        }

        // Get fields and data types
        public async Task<DescribeConnectorEntityResponse> DescribeConnectorEntity(
            string apiVersion,
            string connectorEntityName,
            string connectorProfileName,
            ConnectorType connectorType)
        {
            var request = new DescribeConnectorEntityRequest
            {
                ConnectorProfileName = connectorProfileName,
                ApiVersion = apiVersion,
                ConnectorEntityName = connectorEntityName,
                ConnectorType = connectorType
            };
            var response = await _amazonAppflow.DescribeConnectorEntityAsync(request);
            return response;
        }

        public async Task<DescribeConnectorProfilesResponse> DescribeConnectorProfiles(
            string connectorLabel,
            List<string> connectorProfileNames,
            ConnectorType connectorType
            )
        {
            var request = new DescribeConnectorProfilesRequest
            {
                ConnectorLabel = connectorLabel,
                ConnectorProfileNames = connectorProfileNames,
                ConnectorType = connectorType,
                MaxResults = 100
            };
            var response = await _amazonAppflow.DescribeConnectorProfilesAsync(request);
            return response;
        }

        public async Task<DescribeConnectorsResponse> DescribeConnectors(
            List<string> connectorTypes,
            int maxResults)
        {
            var request = new DescribeConnectorsRequest
            {
                ConnectorTypes = connectorTypes,
                MaxResults = maxResults
            };
            var response = await _amazonAppflow.DescribeConnectorsAsync(request);
            return response;
        }

        public async Task<DescribeFlowResponse> DescribeFlow(string flowName)
        {
            var request = new DescribeFlowRequest
            {
                FlowName = flowName
            };
            var response = await _amazonAppflow.DescribeFlowAsync(request);
            return response;
        }

        public async Task<DescribeFlowExecutionRecordsResponse> DescribeFlowExecutionRecords(string flowName, int maxResults)
        {
            var request = new DescribeFlowExecutionRecordsRequest
            {
                FlowName = flowName,
                MaxResults = maxResults
            };
            var response = await _amazonAppflow.DescribeFlowExecutionRecordsAsync(request);
            return response;
        }

        public async Task<ListConnectorEntitiesResponse> ListConnectorEntities(
            string apiVersion,
            string connectorProfileName,
            string connectorType,
            string entitiesPath,
            int maxResults)
        {
            var request = new ListConnectorEntitiesRequest
            {
                ApiVersion = apiVersion,
                ConnectorProfileName = connectorProfileName,
                ConnectorType = connectorType,
                EntitiesPath = entitiesPath,
                MaxResults = maxResults
            };
            var response = await _amazonAppflow.ListConnectorEntitiesAsync(request);
            return response;
        }

        public async Task<ListConnectorsResponse> ListConnectors(int maxResult)
        {
            var request = new ListConnectorsRequest
            {
                MaxResults = maxResult
            };

            var response = await _amazonAppflow.ListConnectorsAsync(request);
            return response;
        }

        public async Task<ListFlowsResponse> ListFlows(int maxResult)
        {
            var request = new ListFlowsRequest
            {
                MaxResults = maxResult
            };
            var response = await _amazonAppflow.ListFlowsAsync(request);
            return response;
        }

        public async Task<ListTagsForResourceResponse> ListTagsForResource(string resourceArn)
        {
            var request = new ListTagsForResourceRequest
            {
                ResourceArn = resourceArn
            };
            var response = await _amazonAppflow.ListTagsForResourceAsync(request);
            return response;
        }
    }
}
