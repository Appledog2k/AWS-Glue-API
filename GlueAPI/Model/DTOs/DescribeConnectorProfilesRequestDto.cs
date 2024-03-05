namespace GlueAPI.Model.DTOs
{
    public class DescribeConnectorProfilesRequestDto
    {
        public string ConnectorLabel { get; set; }
        public List<string> ConnectorProfileNames { get; set; }
        public string ConnectorType { get; set; }
    }
}
