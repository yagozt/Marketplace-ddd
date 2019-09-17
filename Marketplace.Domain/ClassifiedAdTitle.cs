using Marketplace.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Marketplace.Domain
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        public static ClassifiedAdTitle FromString(string title) => new ClassifiedAdTitle(title);
        private readonly string _value;

        private ClassifiedAdTitle(string value)
        {
            if (value.Length > 100)
            {
                throw new ArgumentOutOfRangeException("Title cannot be longer that 100 characters", nameof(value));
            }

            _value = value;
        }
    }
}
