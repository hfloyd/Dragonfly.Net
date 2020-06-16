namespace Dragonfly.NetHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using HtmlAgilityPack;

    /// <summary>
    /// Helpers dealing with text strings
    /// </summary>
    [Serializable]
    public static class Strings
    {
        #region Testing String Values

        public static bool RegExMatchesExactly(string RegularExpression, string StringToTest, bool CaseSensitive = false)
        {
            bool bolResult;

            Match match;
            if (CaseSensitive)
            { match = Regex.Match(StringToTest, RegularExpression); }
            else
            { match = Regex.Match(StringToTest, RegularExpression, RegexOptions.IgnoreCase); }

            if (match.ToString() == StringToTest)
            { bolResult = true; }
            else
            { bolResult = false; }


            return bolResult;
        }

        public static bool EmailAddressIsValid(string EmailToTest)
        {
            bool Result;
            Regex rgx = new Regex(@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})");

            Result = rgx.IsMatch(EmailToTest) ? true : false;

            return Result;
        }

        /// <summary>
        /// Tests whether a string contains any of the separate values specified in  a delimited string list
        /// </summary>
        /// <param name="StringToTest">The string which might contain one or more of the various values</param>
        /// <param name="DelimitedListOfTestValues">All the values to test</param>
        /// <param name="DelimChar">Character used as the delimiter</param>
        /// <returns>TRUE if any of the values appears in the string. FALSE if NONE of the values appear in the string</returns>
        public static bool StringContainsValueFromList(string StringToTest, string DelimitedListOfTestValues, char DelimChar, bool CaseSensitive = false)
        {
            bool ValueIsInString = false;
            int TotalMatches = 0;

            string StringToTestFin = "";
            string DelimitedListOfTestValuesFin = "";

            if (!CaseSensitive)
            {
                StringToTestFin = StringToTest.ToLower();
                DelimitedListOfTestValuesFin = DelimitedListOfTestValues.ToLower();
            }
            else
            {
                StringToTestFin = StringToTest;
                DelimitedListOfTestValuesFin = DelimitedListOfTestValues;
            }

            List<string> ValuesList = DelimitedListOfTestValuesFin.Split(DelimChar).ToList();

            for (int i = 0; i < ValuesList.Count; i++)
            {
                string ThisValue = ValuesList[i].ToString();

                if (StringToTestFin.Contains(ThisValue))
                {
                    TotalMatches++;
                }
            }

            if (TotalMatches > 0)
            { ValueIsInString = true; }

            return ValueIsInString;

        }

        public static bool UrlsAreOnSameDomain(string Url1, string Url2, string RelativeUrlAssumedDomain)
        {
            if (Url1.StartsWith("/") | Url1.StartsWith("~"))
            {
                Url1 = RelativeUrlAssumedDomain + Url1;
            }

            if (Url2.StartsWith("/") | Url2.StartsWith("~"))
            {
                Url2 = RelativeUrlAssumedDomain + Url2;
            }

            Uri Uri1 = new Uri(Url1);
            Uri Uri2 = new Uri(Url2);

            // There are overlaods for the constructor too
            //Uri uri3 = new Uri(url3, UriKind.Absolute);

            if (Uri1.Host == Uri2.Host)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool IsNumeric(this string StringToTest)
        {
            try
            {
                decimal TestDec = Decimal.Parse(StringToTest);
                return true;
            }
            catch (Exception ex)
            {
                //TODO: Update using new code pattern:
                //var functionName = string.Format("{0}.GetMySQLDataSet", ThisClassName);
                //var msg = string.Format("");
                //Info.LogException("Extensions : String.IsNumeric", ex);
                return false;
            }
        }

        /// <summary>
        /// Tests whether a string contains any of the separate values specified in  a delimited string list
        /// </summary>
        /// <param name="StringToTest">The string which might contain one or more of the various values</param>
        /// <param name="DelimitedListOfTestValues">All the values to test</param>
        /// <param name="DelimChar">Character used as the delimiter</param>
        /// <returns>TRUE if any of the values appears in the string. FALSE if NONE of the values appear in the string</returns>
        public static bool ContainsValueFromList(this string StringToTest, string DelimitedListOfTestValues, char DelimChar, bool CaseSensitive = false)
        {
            bool ValueIsInString = false;
            int TotalMatches = 0;

            string StringToTestFin = "";
            string DelimitedListOfTestValuesFin = "";

            if (!CaseSensitive)
            {
                StringToTestFin = StringToTest.ToLower();
                DelimitedListOfTestValuesFin = DelimitedListOfTestValues.ToLower();
            }
            else
            {
                StringToTestFin = StringToTest;
                DelimitedListOfTestValuesFin = DelimitedListOfTestValues;
            }

            List<string> ValuesList = DelimitedListOfTestValuesFin.Split(DelimChar).ToList();

            for (int i = 0; i < ValuesList.Count; i++)
            {
                string ThisValue = ValuesList[i].ToString();

                if (StringToTestFin.Contains(ThisValue))
                {
                    TotalMatches++;
                }
            }

            if (TotalMatches > 0)
            { ValueIsInString = true; }

            return ValueIsInString;

        }

        public static bool IsValidDate(this string DateStringToTest, string DateFormat)
        {
            bool ValidDate = false;
            try
            {
                var DateTest = DateTime.ParseExact(DateStringToTest, DateFormat, null);

                if (DateTest != null)
                {
                    ValidDate = true;
                }

            }
            catch (Exception exNonValidDate)
            {
                //TODO: Update using new code pattern:
                //var functionName = string.Format("{0}.GetMySQLDataSet", ThisClassName);
                //var msg = string.Format("");
                //Info.LogException("Functions.StringIsValidDate", exNonValidDate, "[DateStringToTest=" + DateStringToTest + "] [DateFormat=" + DateFormat + "] FALSE value returned. No action necessary");
                ValidDate = false;
            }

            return ValidDate;
        }

        public static bool IsValidEmailAddress(this string EmailToTest)
        {
            bool Result;
            Regex rgx = new Regex(@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})");

            Result = rgx.IsMatch(EmailToTest) ? true : false;

            return Result;
        }

        #endregion

        #region Altering String Values

        public static string ReplaceBadChars(string StringToFix, string WordSeparator = "-")
        {
            string NewString = StringToFix;

            NewString = NewString.Replace("'", "");
            NewString = NewString.Replace("\"", WordSeparator);
            NewString = NewString.Replace("/", WordSeparator);
            NewString = NewString.Replace("®", "");
            NewString = NewString.Replace("%", "");
            NewString = NewString.Replace(".", WordSeparator);
            NewString = NewString.Replace(";", WordSeparator);
            NewString = NewString.Replace(":", WordSeparator);
            NewString = NewString.Replace("#", "");
            NewString = NewString.Replace("+", "");
            NewString = NewString.Replace("*", "");
            NewString = NewString.Replace("&", "and");
            NewString = NewString.Replace("?", "");
            NewString = NewString.Replace("*", "");
            NewString = NewString.Replace("æ", "ae");
            NewString = NewString.Replace("ø", "oe");
            NewString = NewString.Replace("å", "aa");
            NewString = NewString.Replace("ä", "ae");
            NewString = NewString.Replace("Ä", "ae");
            NewString = NewString.Replace("ö", "oe");
            NewString = NewString.Replace("ü", "ue");
            NewString = NewString.Replace("ß", "ss");
            NewString = NewString.Replace("Ö", "oe");

            NewString = NewString.Replace(" ", WordSeparator);

            return NewString;
        }

        public static string ReplaceMultiple(this string OriginalString, Dictionary<string, string> ReplacementsDictionary)
        {
            var finalString = OriginalString;

            foreach (var replacement in ReplacementsDictionary)
            {
                finalString = finalString.Replace(replacement.Key, replacement.Value);
            }

            return finalString;
        }

        public static string MakeCodeSafe(this string StringToFix, string WordSeparator = "-", bool ConvertNumbersToWords = false)
        {
            string NewString = StringToFix;

            if (ConvertNumbersToWords)
            {
                NewString = NumeralsToWords(NewString);
            }

            NewString = ReplaceBadChars(NewString, WordSeparator);

            NewString = NewString.Replace(" ", "");

            if (WordSeparator != "")
            {
                string SeparatorDoubled = String.Concat(WordSeparator, WordSeparator);
                bool DupedSeparator = NewString.Contains(SeparatorDoubled);
                do
                {
                    NewString = NewString.Replace(SeparatorDoubled, WordSeparator);
                    DupedSeparator = NewString.Contains(SeparatorDoubled);

                } while (DupedSeparator);
            }

            return NewString;
        }

        public static string NumeralsToWords(this string StringToFix, bool DoComplexReplacements = true, bool Capitalize = true)
        {
            string NewString = StringToFix;

            if (DoComplexReplacements)
            {
                NewString = NewString.Replace("1/2", "Half");
                NewString = NewString.Replace("1/3", "OneThird");
                NewString = NewString.Replace("2/3", "TwoThirds");
                NewString = NewString.Replace("1/4", "OneFourth");
                NewString = NewString.Replace("3/4", "ThreeFourths");
                NewString = NewString.Replace("1/5", "OneFifth");
                NewString = NewString.Replace("2/5", "TwoFifths");
                NewString = NewString.Replace("3/5", "ThreeFifths");
                NewString = NewString.Replace("4/5", "FourFifths");
                NewString = NewString.Replace("1/6", "OneSixth");
                NewString = NewString.Replace("5/6", "FiveSixths");
                NewString = NewString.Replace("1/7", "OneSeventh");
                NewString = NewString.Replace("2/7", "TwoSevenths");
                NewString = NewString.Replace("3/7", "ThreeSevenths");
                NewString = NewString.Replace("4/7", "FourSevenths");
                NewString = NewString.Replace("5/7", "FiveSevenths");
                NewString = NewString.Replace("6/7", "SixSevenths");
                NewString = NewString.Replace("1/8", "OneEighth");
                NewString = NewString.Replace("3/8", "ThreeEighths");
                NewString = NewString.Replace("5/8", "FiveEighths");
                NewString = NewString.Replace("7/8", "Eighths");
                NewString = NewString.Replace("1/9", "OneNinth");
                NewString = NewString.Replace("2/9", "TwoNinths");
                NewString = NewString.Replace("4/9", "FourNinths");
                NewString = NewString.Replace("5/9", "FiveNinths");
                NewString = NewString.Replace("7/9", "SevenNinths");
                NewString = NewString.Replace("8/9", "EightNinths");
                NewString = NewString.Replace("1/10", "OneTenth");
                NewString = NewString.Replace("2/10", "TwoTenths");
                NewString = NewString.Replace("3/10", "ThreeTenths");
                NewString = NewString.Replace("4/10", "FourTenths");
                NewString = NewString.Replace("6/10", "SixTenths");
                NewString = NewString.Replace("7/10", "SevenTenths");
                NewString = NewString.Replace("8/10", "EightTenths");
                NewString = NewString.Replace("9/10", "NineTenths");
                NewString = NewString.Replace("1/11", "OneEleventh");
                NewString = NewString.Replace("2/11", "TwoElevenths");
                NewString = NewString.Replace("3/11", "ThreeElevenths");
                NewString = NewString.Replace("4/11", "FourElevenths");
                NewString = NewString.Replace("5/11", "FiveElevenths");
                NewString = NewString.Replace("6/11", "SixElevenths");
                NewString = NewString.Replace("7/11", "SevenElevenths");
                NewString = NewString.Replace("8/11", "EightElevenths");
                NewString = NewString.Replace("9/11", "NineElevenths");
                NewString = NewString.Replace("10/11", "TenElevenths");
                NewString = NewString.Replace("1/12", "OneTwelfth");
                NewString = NewString.Replace("5/12", "FiveTwelfths");
                NewString = NewString.Replace("7/12", "SevenTwelfths");
                NewString = NewString.Replace("11/12", "ElevenTwelfths");
            }


            NewString = NewString.Replace("0", "Zero");
            NewString = NewString.Replace("1", "One");
            NewString = NewString.Replace("2", "Two");
            NewString = NewString.Replace("3", "Three");
            NewString = NewString.Replace("4", "Four");
            NewString = NewString.Replace("5", "Five");
            NewString = NewString.Replace("6", "Six");
            NewString = NewString.Replace("7", "Seven");
            NewString = NewString.Replace("8", "Eight");
            NewString = NewString.Replace("9", "Nine");

            if (!Capitalize)
            {
                NewString = NewString.ToLower();
            }

            return NewString;
        }

        public static string SplitCamelCase(this string StringToConvert, string SplitCharacters = " ")
        {
            String NewString = "";

            //NewString = System.Text.RegularExpressions.Regex.Replace(StringToConvert, "([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();

            NewString = Regex.Replace(StringToConvert, @"(?<a>(?<!^)[A-Z][a-z])", @" ${a}");
            NewString = Regex.Replace(NewString, @"(?<a>[a-z])(?<b>[A-Z0-9])", @"${a} ${b}");


            if (SplitCharacters != " ")
            {
                NewString = NewString.Replace(" ", SplitCharacters);
            }

            return NewString;
        }

        /// <summary>
        /// Using a dictionary of replacement keys with their corresponding values,
        /// replace the placeholders in the Template content. 
        /// </summary>
        /// <param name="TemplateContent">The email template content to process.</param>
        /// <param name="PlaceholdersData">The placeholder data Dictionary</param>
        /// <param name="TemplatePattern">The format pattern to indicate placeholders in the template content</param>
        public static string ReplacePlaceholders(this string TemplateContent, Dictionary<string, string> PlaceholdersData, string TemplatePattern = "[{0}]", bool EscapeHtml = false)
        {
            StringBuilder templ = new StringBuilder(TemplateContent);

            foreach (var kv in PlaceholdersData)
            {
                var placeholder = string.Format(TemplatePattern, kv.Key);
                var val = kv.Value;

                if (EscapeHtml)
                {
                    val = HttpContext.Current.Server.HtmlEncode(val);
                }

                templ.Replace(placeholder, val);
            }

            return templ.ToString();
        }

        /// <summary>
        /// Add an absolute path to all the img tags in the html of a passed-in string.
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string AddImgAbsolutePath(this string HtmlString)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(HtmlString);

            var uri = new Uri(HttpContext.Current.Request.Url.AbsoluteUri);
            var domainUrl = string.Format("{0}://{1}", uri.Scheme, uri.Authority);

            if (doc.DocumentNode.SelectNodes("//img[@src]") != null)
            {
                foreach (HtmlNode img in doc.DocumentNode.SelectNodes("//img[@src]"))
                {
                    HtmlAttribute att = img.Attributes["src"];
                    if (att.Value.StartsWith("/"))
                    {
                        att.Value = domainUrl + att.Value;
                    }
                }
            }

            return doc.DocumentNode.InnerHtml;
        }

        /// <summary>
        /// Searches for Urls in a string and replaces them with full a href tags
        /// </summary>
        /// <param name="HtmlString">String to search and replace in</param>
        /// <param name="Target">Target for links (default = '_blank' (new window))</param>
        /// <returns></returns>
        public static string EnableHtmlLinks(this string HtmlString, string Target="_blank")
        {
            var fixedHtml = HtmlString;

            var urlRegEx = @"(?:(?:https?|ftp|file):\/\/|www\.|ftp\.)(?:\([-A-Z0-9+&@#\/%=~_|$?!:,.]*\)|[-A-Z0-9+&@#\/%=~_|$?!:,.])*(?:\([-A-Z0-9+&@#\/%=~_|$?!:,.]*\)|[A-Z0-9+&@#\/%=~_|$])";
            //*var urlRegEx = @"(?:(?:https?|ftp):\/\/)?[\w/\-?=%.]+\.[\w/\-?=%.]+";
            //var urlRegEx = @"/(http|https|ftp|ftps)\:\/\/[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(\/\S*)?";
            //var urlRegEx = @"/(ftp:\/\/|www\.|https?:\/\/){1}[a-zA-Z0-9u00a1-\uffff0-]{2,}\.[a-zA-Z0-9u00a1-\uffff0-]{2,}(\S*)";
            //var urlRegEx = @"/[-a-zA-Z0-9@:%_\+.~#?&//=]{2,256}\.[a-z]{2,4}\b(\/[-a-zA-Z0-9@:%_\+.~#?&//=]*)?";
            //var urlRegEx = @"/([--:\w?@%&+~#=]*\.[a-z]{2,4}\/{0,2})((?:[?&](?:\w+)=(?:\w+))+|[--:\w?@%&+~#=]+)?/g";

            var matches = Regex.Matches(HtmlString, urlRegEx, RegexOptions.IgnoreCase);

            if (matches.Count > 0)
            {
                foreach (var match in matches)
                {
                    var matchUrl = match.ToString();
                    var linkHtml = $"<a href=\"{matchUrl}\" target=\"{Target}\">{matchUrl}</a>";
                    fixedHtml = fixedHtml.Replace(matchUrl, linkHtml);
                }

            }

            return fixedHtml;
        }

        /// <summary>
        /// Takes a string with multiple words and returns a string with all words capitalized
        /// </summary>
        /// <param name="Original"></param>
        /// <param name="LowercaseAbbreviations">If TRUE will change ALL CAPS words to lowercase before capitalizing (returns 'All Caps Words')</param>
        /// <returns></returns>
        public static string MakeCamelCase(this string Original, bool LowercaseAbbreviations = false)
        {
            var finalString = "";

            var allWords = Original.Split(' ');

            foreach (var word in allWords)
            {
                if (LowercaseAbbreviations)
                {
                    finalString += word.ToLower().Capitalize();
                }
                else
                {
                    finalString += word.Capitalize();
                }
                
            }

            return finalString;
        }

        public static string Capitalize(this string Word)
        {
            var finalString = "";

            var allLetters = Word.ToCharArray();
            var isFirstLetter = true;

            foreach (var letter in allLetters)
            {
                if (isFirstLetter)
                {
                    finalString += letter.ToString().ToUpper();
                    isFirstLetter = false;
                }
                else
                {
                    finalString += letter;
                }
            }

            return finalString;
        }

        public static string SplitByTokenIfItExists(string Name, string Token)
        {
            if (Name.IndexOf(Token) > -1)
            {
                return Name.Substring(0, Name.IndexOf(Token));
            }
            return Name;
        }

        public static string StripNumbers(string TextWithNumbers)
        {
            var stripped = TextWithNumbers;

            stripped = new String(TextWithNumbers.Where(c => c != '-' && (c < '0' || c > '9')).ToArray());

            return stripped;
        }

        [Obsolete("Use 'RemoveAllParagraphTags()")]
        public static string RemoveParagraphTags(this string Html, bool RetainBreaks)
        {
            return Html.RemoveAllParagraphTags(RetainBreaks);
        }

        [Obsolete("Use 'RemoveAllParagraphTags()")]
        public static IHtmlString RemoveParagraphTags(this IHtmlString Html, bool RetainBreaks)
        {
            return Html.RemoveAllParagraphTags(RetainBreaks);
        }

        public static string RemoveAllParagraphTags(this string Html, bool RetainBreaks)
        {
            var result = RemoveAllParagraphTags(new HtmlString(Html), RetainBreaks);
            return result.ToString();
        }

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

        public static string RemoveOuterParagrahTags(this string HtmlToFix)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(HtmlToFix);
            string result = doc.DocumentNode.FirstChild.InnerHtml;

            return result;
        }

        public static IHtmlString RemoveOuterParagrahTags(this IHtmlString HtmlToFix)
        {
            string result = RemoveOuterParagrahTags(HtmlToFix.ToString());
            return new HtmlString(result);
        }

        public static IHtmlString ReplaceLineBreaksForWeb(this string StringToFix)
        {
            return new HtmlString(StringToFix.Replace("\r\n", "<br />").Replace("\n", "<br />"));
        }


        /// <summary>
        /// Encodes the string
        /// </summary>
        /// <param name="OriginalString"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string OriginalString)
        { 
            return System.Web.HttpUtility.UrlEncode(OriginalString);
        }

        /// <summary>
        /// Decodes the string
        /// </summary>
        /// <param name="EncodedString"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string EncodedString)
        {
            return System.Web.HttpUtility.UrlDecode(EncodedString);
        }

        #endregion

        #region Returning a String from a String or Multiple Strings

        //TODO: Convert to use param list for unlimited testing values, like string.concat()
        public static string NoEmptyString(string FirstPreferredString, string SecondString, string ThirdString = "")
        {
            if (FirstPreferredString != null & FirstPreferredString != "")
            { return FirstPreferredString; }
            else if (SecondString != null & SecondString != "")
            { return SecondString; }
            else
            { return ThirdString; }
        }

        public static string HtmlTagContents(string TagName, string TextToSearch)
        {
            string ReturnContent = "";

            Regex TagRegex = new Regex(
                "<(?<tag>\\w*)>(?<text>.*)</\\k<tag>>",
                RegexOptions.IgnoreCase
                | RegexOptions.CultureInvariant
                | RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
            );

            bool IsMatch = TagRegex.IsMatch(TextToSearch);
            bool Found = false;

            if (IsMatch)
            {
                // Capture all Matches in the InputText
                MatchCollection AllMatches = TagRegex.Matches(TextToSearch);

                for (int i = 0; i < AllMatches.Count; i++)
                {
                    if (!Found)
                    {
                        string Tag = AllMatches[i].Groups["tag"].Value;
                        string Text = AllMatches[i].Groups["text"].Value;

                        if (Tag == TagName)
                        {
                            ReturnContent = Text;
                            Found = true;
                        }
                    }
                }
            }

            return ReturnContent;
        }

        /// <summary>
        /// Converts a phone number in text format to a format which can be used in a 'tel:' link
        /// </summary>
        /// <param name="PhoneData">Original Phone number</param>
        /// <param name="ReturnAllIfNoMatch">If it doesn't match a defined phone number format, should the whole string be returned as-is? (FALSE will return an empty string)</param>
        /// <param name="StripChars">Remove additional characters such as ( ) - and spaces from the string</param>
        /// <returns></returns>
        public static string GetClickablePhoneNumber(string PhoneData, bool ReturnAllIfNoMatch = false, bool StripChars = false)
        {
            var returnString = "";

            //TODO: Enhancement - Add support for international phone numbers
            //Regex stuff to id US phone numbers
            var countrycodes = "1";
            var delimiters = "-|\\.|—|–|&nbsp;";
            var phonedef = "\\+?(?:(?:(?:" + countrycodes + ")(?:\\s|" + delimiters + ")?)?\\(?[2-9]\\d{2}\\)?(?:\\s|"
                           + delimiters + ")?[2-9]\\d{2}(?:" + delimiters + ")?[0-9a-z]{4})";

            var regEx = new Regex(phonedef);

            if (regEx.IsMatch(PhoneData))
            {
                var match = regEx.Match(PhoneData);
                returnString = match.Value;
            }
            else
            {
                if (ReturnAllIfNoMatch)
                {
                    returnString = PhoneData;
                }
            }

            if (StripChars)
            {
                returnString = returnString.Replace("-", "").Replace(".", "").Replace("(", "").Replace(")", "").Replace(" ", "");
            }

            if (!returnString.StartsWith("+"))
            {
                returnString = string.Format("+{0}", returnString);
            }

            return returnString;
        }

        public static string GetDomain(string Url)
        {
            var domain = new Uri(Url).Host;
            return domain;
        }

        /// <summary>
        /// Similar to String.Join(), but allows for 2 different separators in order to provide a natural text representation of a list 
        /// (Example: 'A, B, and C' uses ', ' &amp; ' and ' as the two separators.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Separator">First separator (ex: ', ')</param>
        /// <param name="SeparatorLast">Second separator (ex: ' and ')</param>
        /// <param name="Values">IEnumerable to provide strings</param>
        /// <returns>String including all values</returns>
        public static string JoinAsText<T>(string Separator, string SeparatorLast, IEnumerable<T> Values)
        {
            if (Values == null)
            {
                return "";
            }

            var values = Values.ToList();

            if (!values.Any())
            {
                return "";
            }

            var total = values.Count();

            if (total == 1)
            {
                return values.First().ToString();
            }
            else if (total == 2)
            {
                return string.Join(SeparatorLast, values);
            }
            else
            {
                var lastItem = values.Last();
                values.Remove(lastItem);

                var joinedString = string.Join(Separator, values);
                joinedString += SeparatorLast + lastItem;

                return joinedString;
            }

        }

        /// <summary>
        /// Truncates a long string to a maximum length without leaving a partial word at the end.
        /// </summary>
        /// <param name="OriginalString">Long string</param>
        /// <param name="MaxLength">Maximum desired length after truncation and Suffix</param>
        /// <param name="Suffix">Appended to end of truncated string (default = ellipse character)</param>
        /// <returns>Truncated string with suffix</returns>
        public static string TruncateAtWord(this string OriginalString, int MaxLength, string Suffix = "…")
        {
            var length = MaxLength - Suffix.Length;

            if (OriginalString == null || OriginalString.Length <= MaxLength)
                return OriginalString;

            int iNextSpace = OriginalString.LastIndexOf(" ", length, StringComparison.Ordinal);
            var shortString = OriginalString.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim();
            return string.Format("{0}{1}", shortString, Suffix);
        }
        #endregion

        #region Misc

        /// <summary>
        /// Count occurrences of a string inside another string.
        /// </summary>
        public static int CountStringOccurrences(this string Text, string Pattern)
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ((i = Text.IndexOf(Pattern, i)) != -1)
            {
                i += Pattern.Length;
                count++;
            }
            return count;
        }

        #endregion

        #region Converting between String and Objects

        /// <summary>
        /// Splits a string into a List of strings
        /// </summary>
        /// <param name="DelimitedString"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static List<string> ConvertToList(this string DelimitedString, char Separator)
        {
            List<string> myList = new List<string>(DelimitedString.Split(Separator));
            return myList;
        }

        /// <summary>
        /// Converts an array of strings, each separated with the provided character, into a Dictionary 
        /// </summary>
        /// <param name="StringArray"></param>
        /// <param name="Separator">Character used to separate values in each item, default is an equal sign (=)</param>
        /// <returns></returns>
        public static Dictionary<string, string> ConvertToDictionary(this string[] StringArray, char Separator = '=')
        {
            var dictionary = new Dictionary<string, string>();

            for (int i = 0; i < StringArray.Count(); i++)
            {
                var item = StringArray[i].Split(Separator);
                var key = item[0];
                var val = item[1];
                dictionary.Add(key, val);
            }
            return dictionary;
        }

        /// <summary>
        /// Converts a Dictionary into an array of strings, each separated with the provided character
        /// </summary>
        /// <param name="Dictionary"></param>
        /// <param name="Separator">Character to separate values in each dictionary item, default is an equal sign (=)</param>
        /// <returns></returns>
        public static string[] ConvertToStringArray(this Dictionary<string, string> Dictionary, char Separator = '=')
        {
            var num = Dictionary.Count;
            string[] items = new string[num];

            int iCount = 0;

            foreach (KeyValuePair<string, string> attribute in Dictionary)
            {
                var combined = attribute.Key + Separator + attribute.Value;
                items[iCount] = combined;
                iCount++;
            }

            return items;
        }

        /// <summary>
        /// Takes a collection of string and turns them into a single string using the provided separator
        /// </summary>
        /// <param name="ListOfStrings"></param>
        /// <param name="Separator"></param>
        /// <returns></returns>
        public static string ConvertToSeparatedString(this IEnumerable<string> ListOfStrings, string Separator)
        {
            string myString = Separator;

            foreach (var listItem in ListOfStrings)
            {
                myString += Separator + listItem;
            }

            myString = myString.Replace(String.Concat(Separator, Separator), "");

            return myString;
        }

        /// <summary>
        /// Converts a string in the format "a=b" into a KeyValuePair
        /// </summary>
        /// <param name="SingleKvString">String in the format "a=b"</param>
        /// <returns>A single KeyValuePair object</returns>
        public static KeyValuePair<string, string> ConvertStringToKvPair(string SingleKvString)
        {
            if (SingleKvString.Contains("="))
            {
                var splitKV = SingleKvString.Split('=');
                var kvp = new KeyValuePair<string, string>(splitKV[0], splitKV[1]);
                return kvp;
            }
            else
            {
                //invalid data, return empty KVP
                var kvp = new KeyValuePair<string, string>();
                return kvp;
            }
        }

        /// <summary>
        /// Converts a string in the format "a=b,x=y" or "a=b&x=y" or "a=b|x=y" into an IEnum of KeyValuePairs
        /// </summary>
        /// <param name="KvString">String in the format "a=b,x=y" or "a=b&x=y" or "a=b|x=y"</param>
        /// <returns>Parsed KeyValuePair objects</returns>
        public static IEnumerable<KeyValuePair<string, string>> ParseStringToKvPairs(string KvString)
        {
            var kvPairs = new List<KeyValuePair<string, string>>();

            var countPairs = KvString.CountStringOccurrences("=");

            if (countPairs == 1)
            {
                //Single pair
                var kvp = ConvertStringToKvPair(KvString);
                kvPairs.Add(kvp);
            }
            else if (countPairs > 1)
            {
                if (KvString.Contains("&"))
                {
                    var splitPairs = KvString.Split('&');

                    foreach (var pair in splitPairs)
                    {
                        var kvp = ConvertStringToKvPair(pair);
                        kvPairs.Add(kvp);
                    }
                }
                else if (KvString.Contains(","))
                {
                    var splitPairs = KvString.Split(',');

                    foreach (var pair in splitPairs)
                    {
                        var kvp = ConvertStringToKvPair(pair);
                        kvPairs.Add(kvp);
                    }
                }
                else if (KvString.Contains("|"))
                {
                    var splitPairs = KvString.Split('|');

                    foreach (var pair in splitPairs)
                    {
                        var kvp = ConvertStringToKvPair(pair);
                        kvPairs.Add(kvp);
                    }
                }
            }

            return kvPairs;
        }

        #endregion

    }
}