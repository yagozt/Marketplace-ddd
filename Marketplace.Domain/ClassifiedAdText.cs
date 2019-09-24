using System;
using Marketplace.Framework;

namespace Marketplace.Domain
{
    public class ClassifiedAdText : Value<ClassifiedAdText>
    {
        public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);
        public string Value { get; }
        private ClassifiedAdText(string value) => Value = value;

        public static implicit operator string(ClassifiedAdText text) => text.Value;
    }
}