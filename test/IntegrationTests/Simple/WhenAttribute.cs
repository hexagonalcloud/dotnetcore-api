using System;

namespace IntegrationTests.Simple
{
    [AttributeUsage(AttributeTargets.Method)]
    public class WhenAttribute : Attribute
    {
        public WhenAttribute(string decription)
        {
            Description = decription;
        }

        public string Description { get; }
    }
}
