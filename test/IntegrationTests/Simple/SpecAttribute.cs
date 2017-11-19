using System;
using Xunit.Sdk;

namespace IntegrationTests.Simple
{
    public class SpecAttribute: Attribute, ITraitAttribute
    {
        public SpecAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}
