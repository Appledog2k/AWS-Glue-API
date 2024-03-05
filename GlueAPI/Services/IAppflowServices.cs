using Amazon.Appflow;
using Amazon.Appflow.Model;

namespace GlueAPI.Services
{
    public interface IAppflowServices
    {
        Task<DescribeConnectorResponse> DescribeConnector(string connectorType, string connectorLabel);
        Task<DescribeConnectorEntityResponse> DescribeConnectorEntity(
            string apiVersion,
            string connectorEntityName,
            string connectorProfileName,
            ConnectorType connectorType);
        Task<DescribeConnectorProfilesResponse> DescribeConnectorProfiles(
            string connectorLabel,
            List<string> connectorProfileNames,
            ConnectorType connectorType);
        Task<DescribeConnectorsResponse> DescribeConnectors(
            List<string> connectorTypes,
            int maxResults);
        Task<DescribeFlowResponse> DescribeFlow(string flowName);
        Task<DescribeFlowExecutionRecordsResponse> DescribeFlowExecutionRecords(string flowName, int maxResults);
        Task<ListConnectorEntitiesResponse> ListConnectorEntities(
            string apiVersion,
            string connectorProfileName,
            string connectorType,
            string entitiesPath,
            int maxResults);
        Task<ListConnectorsResponse> ListConnectors(int maxResult);
        Task<ListFlowsResponse> ListFlows(int maxResult);
        Task<ListTagsForResourceResponse> ListTagsForResource(string resourceArn);
    }
}
