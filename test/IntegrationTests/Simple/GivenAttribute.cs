using System;

namespace IntegrationTests.Simple
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GivenAttribute : Attribute
    {
        public GivenAttribute(string decription)
        {
            Description = decription;
        }

        public string Description { get; }
    }
}
