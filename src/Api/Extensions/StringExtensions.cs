using System;

// ReSharper disable once CheckNamespace
namespace Api
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string value)
        {
            var tokens = value.Split(new[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token == token.ToUpper()
                    ? token
                    : token.Substring(0, 1).ToUpper() + token.Substring(1).ToLower();
            }

            return string.Join(" ", tokens);
        }
    }
}
