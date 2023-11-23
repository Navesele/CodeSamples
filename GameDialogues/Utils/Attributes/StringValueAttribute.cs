using System;

namespace Marshtown.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class StringValueAttribute : Attribute
    {
        public string StringValue { get; }

        public StringValueAttribute(string value)
        {
            StringValue = value;
        }
    }
}
