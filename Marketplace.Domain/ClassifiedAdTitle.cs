using Marketplace.Framework;
using System;
using System.Text.RegularExpressions;

namespace Marketplace.Domain
{
    public class ClassifiedAdTitle : Value<ClassifiedAdTitle>
    {
        public static ClassifiedAdTitle FromString(string title)
        {
            CheckValidity(title);
            return new ClassifiedAdTitle(title);
        }

        private static void CheckValidity(string value)
        {
            if (value.Length > 100)
                throw new ArgumentOutOfRangeException("Title cannot be longer than 100 characters", nameof(value));
        }

        public static ClassifiedAdTitle FromHTML(string htmlTitle)
        {
            var supportedTagsReplaced = htmlTitle
            .Replace("<i>", "*")
            .Replace("</i>", "*")
            .Replace("<b>", "*")
            .Replace("</b>", "*");

            var value = Regex.Replace(supportedTagsReplaced, "<.*?>", string.Empty);
            CheckValidity(value);

            return new ClassifiedAdTitle(value);
        }
        public string Value { get; }

        internal ClassifiedAdTitle(string value)
        {
            Value = value;
        }
        public static implicit operator string(ClassifiedAdTitle self) => self.Value;
    }
}
