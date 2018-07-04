using System.Text;

namespace DocsScraper
{
    /// <summary>
    /// Formats English text with proper punctuation, etc.
    /// </summary>
    public static class TextFormatter
    {
        /// <summary>
        /// Formats plain text and corrects spacing and punctuation.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Format(string input)
        {
            var output = new StringBuilder(input);

            output.Replace('\n', ' ');
            output.Replace('\r', ' ');

            // Remove duplicated spaces
            int length;
            do
            {
                length = output.Length;
                output.Replace("  ", " ");
            } while (length != output.Length);

            return output.ToString();
        }
    }
}
