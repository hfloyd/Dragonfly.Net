namespace Dragonfly.NetHelpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public static class Html
    {
        /// <summary>
        /// Takes a List of IHtmlStrings and concatenates them together using a provided delimiter
        /// </summary>
        /// <param name="List">List of IHtmlStrings</param>
        /// <param name="Delimiter">string to separate HTML Strings</param>
        /// <returns>IHtmlString</returns>
        public static IHtmlString ToConcatenatedHtmlString(this List<IHtmlString> List, string Delimiter)
        {
            var returnSB = new StringBuilder();

            foreach (var item in List)
            {
                returnSB.Append(item.ToString());

                if (item != List.Last())
                {
                    returnSB.Append(Delimiter);
                }
            }

            return new HtmlString(returnSB.ToString());
        }
    }
}
