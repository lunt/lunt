using Xunit;

namespace Lake.Tests.Integration
{
    public class IntegrationTestAttribute : TraitAttribute
    {
        public IntegrationTestAttribute()
            : base("Category", "IntegrationTest")
        {
        }
    }
}
