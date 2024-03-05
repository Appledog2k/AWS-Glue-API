using Amazon.Runtime;

namespace GlueAPI.Services
{
    public class CrawlState : ConstantClass
    {
        public CrawlState(string value) : base(value)
        {
        }

        public CrawlState FindValue(string value)
        {
            return ConstantClass.FindValue<CrawlState>(value);
        }
    }
}
