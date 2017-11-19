using System;
using Xunit;

namespace IntegrationTests.Simple
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ThenAttribute : FactAttribute
    {
        public string Description { get; set; }
    }
}
