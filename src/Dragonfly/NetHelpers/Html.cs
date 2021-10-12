namespace Dragonfly.NetHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;

    public static class Html
    {
        #region Testing HtmlString Values

        /// <summary>
        /// Checks for content 
        /// </summary>
        /// <param name="HtmlString">String to test</param>
        /// <param name="EmptyParagraphsIsNull">Should a string made up of only empty &lt;p&gt; tags be considered null?</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this IHtmlString HtmlString, bool EmptyParagraphsIsNull)
        {
            if (HtmlString == null)
            {
                return true;
            }

            var testString = HtmlString.ToHtmlString();
            if (string.IsNullOrWhiteSpace(testString))
            {
                return true;
            }

            if (EmptyParagraphsIsNull)
            {
                testString = testString.RemoveAllParagraphTags(false).Trim();
                return string.IsNullOrWhiteSpace(testString);
            }

            //If we get here
            return false;
        }



        #endregion

        #region Altering String Values

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

        /// <summary>
        /// Replaces plain-text line breaks with &lt;br/&gt; tags
        /// </summary>
        /// <param name="StringToFix">Original string</param>
        /// <returns></returns>
        public static IHtmlString ReplaceLineBreaksForWeb(this string StringToFix)
        {
            return new HtmlString(StringToFix.Replace("\r\n", "<br />").Replace("\n", "<br />"));
        }

        /// <summary>
        /// Strips out &lt;p&gt; and &lt;/p&gt; tags if they were used as a wrapper
        /// for other HTML content.
        /// </summary>
        /// <param name="Text">The HTML text.</param>
        /// <param name="ConvertEmptyParagraphsToBreaks"></param>
        public static IHtmlString RemoveParagraphWrapperTags(this IHtmlString Text, bool ConvertEmptyParagraphsToBreaks = false)
        {
            var str = Text.ToString();
            var fixedText = str.RemoveParagraphWrapperTags(ConvertEmptyParagraphsToBreaks);
            return new HtmlString(fixedText);
        }

        /// <summary>
        /// Remove all &lt;p&gt; tags
        /// </summary>
        /// <param name="Html"></param>
        /// <param name="RetainBreaks">Replaces the paragraph tag with two &lt;br&gt; tags</param>
        /// <returns></returns>
        public static IHtmlString RemoveAllParagraphTags(this IHtmlString Html, bool RetainBreaks)
        {
            var result = Html.ToString();

            if (RetainBreaks)
            {
                result = result.Replace("\r\n<p>", "<br/><br/>");
                result = result.Replace("</p><p>", "<br/><br/>");
            }
            result = result.Replace("<p>", "");
            result = result.Replace("</p>", " ");

            return new HtmlString(result);
        }


        /// <summary>
        /// Removes surrounding &lt;p&gt; tags
        /// </summary>
        /// <param name="HtmlToFix"></param>
        /// <returns></returns>
        public static IHtmlString RemoveOuterParagrahTags(this IHtmlString HtmlToFix)
        {
            string result = HtmlToFix.ToString().RemoveOuterParagrahTags();
            return new HtmlString(result);
        }


        /// <summary>
        /// Removes all Tags from IHtmlString
        /// </summary>
        /// <param name="Input">Original IHtmlString</param>
        /// <returns></returns>
        public static string StripHtml(this IHtmlString Input)
        {
            var inputS = Input.ToString();
            return Strings.StripHtml(inputS);
        }

        /// <summary>
        /// Umbraco 7 Version
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static IHtmlString RemoveFirstParagraphTag(this IHtmlString Text)
        {
            var str = Text.ToHtmlString();
            var fix = str.RemoveFirstParagraphTag();
            return new HtmlString(fix);
        }

        [Obsolete("Use 'RemoveAllParagraphTags()")]
        public static IHtmlString RemoveParagraphTags(this IHtmlString Html, bool RetainBreaks)
        {
            return Html.RemoveAllParagraphTags(RetainBreaks);
        }

        #endregion
    }

}
