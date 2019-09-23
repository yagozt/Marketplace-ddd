using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdText : Value<ClassifiedAdText>
    {
        public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);
        private readonly string _value;
        private ClassifiedAdText(string value)
        {
            _value = value;
        }
    }
}