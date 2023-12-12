using System.Text.RegularExpressions;

namespace LearnProject.Data.DAL.Configuration
{
    internal static class StringExtensions
    {
        /// <summary>
        /// перевод из CamelCase в snake_case
        /// </summary>
        /// <param name="input">строка</param>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) { return input; }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }
    }
}
