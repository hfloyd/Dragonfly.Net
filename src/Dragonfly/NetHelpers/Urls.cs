﻿namespace Dragonfly.NetHelpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// Functions to assist with Url-related tasks - reading, editing/building, query strings, basic calling
    /// </summary>
    public static class Urls
    {
        private const string ThisClassName = "Dragonfly.NetHelpers.Urls";

        #region Read Data from Url

        /// <summary>
        /// Parse a Query string, returning a Dictionary representing all the Query String Values
        /// </summary>
        /// <param name="Query">Query-string portion of a url</param>
        /// <returns>Dictionary&gt;string, string&lt;</returns>
        public static Dictionary<string, string> GetQueryStringDictionary(string Query)
        {
            var returnDict = new Dictionary<string, string>();

            if (Query != "")
            {
                var splitString = Query.Replace("?", "").Split('&');

                foreach (var pairString in splitString)
                {
                    var pair = pairString.Split('=');
                    returnDict.Add(pair[0], pair[1]);
                }
            }

            return returnDict;
        }

        /// <summary>
        /// Compares 2 URls to see if they are on the same top-level-domain
        /// </summary>
        /// <param name="Url1">First Url</param>
        /// <param name="Url2">Second Url</param>
        /// <param name="RelativeUrlAssumedDomain">If there is no TLD (a relative Url) assume that this is on the same domain</param>
        /// <returns></returns>
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

        #endregion

        #region Get QueryString Values

        /// <summary>
        /// Returns the Querystring value cast to T, or the Default, if missing
        /// </summary>
        /// <param name="Request">HttpRequest (Just use 'Request')</param>
        /// <param name="QueryStringKey">Key name</param>
        /// <param name="DefaultIfMissing">A default value to return in case the Querystring value is missing</param>
        /// <param name="ReturnDefaultOnConversionFailure">If there is a value, but converting it to the desired time fails, should the default be returned?
        /// (FALSE will throw an Exception on failure)</param>
        /// <returns></returns>
        public static T GetSafeQueryStringValue<T>(System.Web.HttpRequestBase Request, string QueryStringKey, T DefaultIfMissing = default, bool ReturnDefaultOnConversionFailure = false)
        {
            var qsVal = Request.QueryString[QueryStringKey];
            
            if (string.IsNullOrEmpty(qsVal))
            {
                return DefaultIfMissing;
            }
            else
            {
                try
                {
                    return (T)Convert.ChangeType(qsVal, typeof(T));
                }
                catch (Exception e1)
                {
                    try
                    {
                        Type returnType = typeof(T);
                        object convertedObj = (object)qsVal;

                        //if (returnType == typeof(int))
                        //{
                        //    convertedObj = (object)Convert.ToInt32(qsVal);
                        //}

                        return (T)convertedObj;
                    }
                    catch (Exception e2)
                    {
                        if (ReturnDefaultOnConversionFailure)
                        {
                            return DefaultIfMissing;
                        }
                        else
                        {
                            throw;
                        }

                    }
                }
            }


        }



        /// <summary>
        /// Returns a string value (empty string, if missing)
        /// </summary>
        /// <param name="Request">HttpRequest (Just use 'Request')</param>
        /// <param name="QueryStringKey">Key name</param>
        /// <param name="DefaultIfMissing">Value to return if missing/empty</param>
        /// <returns></returns>
        [Obsolete("Consider using 'GetSafeQueryStringValue<T>()' function")]
        public static string GetSafeQueryString(System.Web.HttpRequestBase Request, string QueryStringKey, string DefaultIfMissing = "")
        {
            var returnVal = DefaultIfMissing;

            var qsVal = Request.QueryString[QueryStringKey];

            if (!string.IsNullOrEmpty(qsVal))
            {
                returnVal = qsVal;
            }

            return returnVal;
        }

        /// <summary>
        /// Returns a boolean value. Assumes "True" or "true" as string. (false, if missing) 
        /// </summary>
        /// <param name="Request">HttpRequest (Just use 'Request')</param>
        /// <param name="QueryStringKey">Key name</param>
        /// <param name="DefaultIfMissing">Value to return if missing/empty</param>
        /// <returns></returns>
        [Obsolete("Consider using 'GetSafeQueryStringValue<T>()' function")]
        public static bool GetSafeQueryBool(System.Web.HttpRequestBase Request, string QueryStringKey, bool DefaultIfMissing = false)
        {
            var returnVal = DefaultIfMissing;

            var qsVal = Request.QueryString[QueryStringKey];

            if (!string.IsNullOrEmpty(qsVal))
            {
                if (qsVal.ToLower() == "true")
                {
                    returnVal = true;
                }
            }
            return returnVal;
        }


        /// <summary>
        /// Returns a boolean value. Assumes "True" or "true" as string. (false, if missing) 
        /// </summary>
        /// <param name="Request">HttpRequest (Just use 'Request')</param>
        /// <param name="QueryStringKey">Key name</param>
        /// <param name="DefaultIfMissing">Value to return if missing/empty</param>
        /// <returns></returns>
        [Obsolete("Consider using 'GetSafeQueryStringValue<T>()' function")]
        public static int GetSafeQueryInt(System.Web.HttpRequestBase Request, string QueryStringKey, int DefaultIfMissing = 0)
        {
            var returnVal = DefaultIfMissing;

            var qsVal = Request.QueryString[QueryStringKey];

            if (!string.IsNullOrEmpty(qsVal))
            {
                var isInt = Int32.TryParse(qsVal, out int intVal);
                if (isInt)
                {
                    returnVal = intVal;
                }
            }
            return returnVal;
        }

        static string GetSafeQuery(this Uri uri)
        {
            //Copied from https://github.com/umbraco/Umbraco-CMS/blob/d50e49ad37fd5ca7bad2fd6e8fc994f3408ae70c/src/Umbraco.Core/UriExtensions.cs

            if (uri.IsAbsoluteUri)
                return uri.Query;

            // cannot get .Query on relative uri (InvalidOperation)
            var s = uri.OriginalString;
            var posq = s.IndexOf("?", StringComparison.Ordinal);
            var posf = s.IndexOf("#", StringComparison.Ordinal);
            var query = posq < 0 ? null : (posf < 0 ? s.Substring(posq) : s.Substring(posq, posf - posq));

            return query;
        }


        #endregion

        #region Goto Url

        /// <summary>
        /// Method to simplify calling an external url.  Any results from the url will be saved as a string that can
        /// either be xml, html, json, etc etc.  It won't handle calling files directly, I think, not tried it,
        /// not had a need.  It works great with string based result though.
        /// </summary>
        /// <param name="url">The url you want to call</param>
        /// <param name="method">How you want to call it: "GET" or "POST"</param>
        /// <returns></returns>
        public static string CallUrl(string Url, string Method, string UserAgentString = "C# Application (compatible; MSIE 6.0; Windows NT 5.1)")
        {
            //From http://you.arenot.me/2010/09/28/facebooks-graph-api-and-asp-net/
            string UserAgent = UserAgentString;
            int _timeout = 300000;

            HttpWebRequest req = null;
            HttpWebResponse res = null;

            // Initialise the web request
            req = (HttpWebRequest)WebRequest.Create(Url);
            req.Method = Method.Length > 0 ? Method : "POST";

            req.UserAgent = UserAgent;

            // if (Proxy != null) req.Proxy = Proxy;
            req.Timeout = _timeout;
            req.KeepAlive = false;

            // This is needed in the Compact Framework
            // See for more details: http://msdn2.microsoft.com/en-us/library/1afx2b0f.aspx
            if (Method != "GET")
                req.GetRequestStream().Close();

            string responseString = String.Empty;

            try
            {
                // Get response from the internet
                res = (HttpWebResponse)req.GetResponse();
                using (StreamReader sr = new StreamReader(res.GetResponseStream()))
                {
                    responseString = sr.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                //TODO: Update using new code pattern:
                //var functionName = string.Format("{0}.GetMySQLDataSet", ThisClassName);
                //var msg = string.Format("");
                //Info.LogException("Functions.CallUrl", ex);
            }

            return responseString;
        }

        /// <summary>
        /// Just an overload of above.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CallUrl(string Url)
        {
            return CallUrl(Url, "GET");
        }

        #endregion

        #region Edit/Build Url


        /// <summary>
        /// Updates a URL with new/additional query string values
        /// </summary>
        /// <param name="OriginalUrl">Url string to append to</param>
        /// <param name="QsDictionary">Query String Key/Value dictionary</param>
        /// <param name="AppendMatchingTagDelim">The string used to append additional values to an existing Key. If omitted, existing tags will be replaced, not appended.</param>
        /// <param name="NewAnchor">If Provided, will append/replace the anchor tag</param>
        /// <returns>URI</returns>
        public static string AppendQueryStringToUrl(string OriginalUrl, Dictionary<string, string> QsDictionary, string AppendMatchingTagDelim = "", string NewAnchor = "")
        {
            var url = new Uri(OriginalUrl);
            foreach (var qs in QsDictionary)
            {
                var newUrl = AppendQueryStringToUrl(url, qs.Key, qs.Value, AppendMatchingTagDelim, NewAnchor);
                url = new Uri(newUrl);
            }

            return url.ToString();
        }


        /// <summary>
        /// Updates a URL with new/additional query string values
        /// </summary>
        /// <param name="OriginalUrl">Url string to append to</param>
        /// <param name="QsKey">Query String Key name</param>
        /// <param name="QsValue">Value to append</param>
        /// <param name="AppendMatchingTagDelim">The string used to append additional values to an existing Key. If omitted, existing tags will be replaced, not appended.</param>
        /// <param name="NewAnchor">If Provided, will append/replace the anchor tag</param>
        /// <returns>URI</returns>
        public static string AppendQueryStringToUrl(string OriginalUrl, string QsKey, string QsValue, string AppendMatchingTagDelim = "", string NewAnchor = "")
        {
            var url = new Uri(OriginalUrl);
            return AppendQueryStringToUrl(url, QsKey, QsValue, AppendMatchingTagDelim, NewAnchor);
        }


        /// <summary>
        /// Updates a URL with new/additional query string values
        /// </summary>
        /// <param name="OriginalUri">Uri to append to</param>
        /// <param name="QsKey">Query String Key name</param>
        /// <param name="QsValue">Value to append</param>
        /// <param name="AppendMatchingTagDelim">The string used to append additional values to an existing Key. If omitted, existing tags will be replaced, not appended.</param>
        /// <param name="NewAnchor">If Provided, will append/replace the anchor tag</param>
        /// <returns>URI</returns>
        public static string AppendQueryStringToUrl(Uri OriginalUri, string QsKey, string QsValue, string AppendMatchingTagDelim = "", string NewAnchor = "")
        {
            var uri = OriginalUri;

            var baseUrl = $"{uri.Scheme}://{uri.Host}{uri.LocalPath}";
            //var basePath = uri.AbsolutePath;

            //Anchor Tag
            var currAnchor = uri.Fragment.Replace("#", "");
            var providedAnchor = NewAnchor.Replace("#", "");
            var newAnchor = "";

            if (currAnchor != "")
            {
                if (NewAnchor != "")
                {
                    newAnchor = providedAnchor;
                }
                else
                { newAnchor = currAnchor; }

                newAnchor = "#" + newAnchor;
            }
            else
            {
                newAnchor = providedAnchor;
            }

            //Query String Values
            var qs = GetQueryStringDictionary(uri.Query);
            var allQs = "&";
            var newKeyProcessed = false;

            foreach (var item in qs)
            {
                if (item.Key == QsKey)
                {
                    //matches the new KV - process
                    newKeyProcessed = true;
                    if (QsValue != "")
                    {
                        if (AppendMatchingTagDelim != "")
                        {
                            var currVals = item.Value.Split(AppendMatchingTagDelim.ToCharArray());
                            if (!currVals.Contains(QsValue))
                            {
                                //append value to existing 
                                allQs = string.Format("{0}&{1}={2}{3}{4}", allQs, QsKey, item.Value, AppendMatchingTagDelim, QsValue);
                            }
                        }
                        else
                        {
                            //replace value
                            allQs = string.Format("{0}&{1}={2}", allQs, QsKey, QsValue);
                        }
                    }
                    else
                    {
                        //remove tag - skip
                    }
                }
                else
                {
                    //Just append it as-is
                    allQs = string.Format("{0}&{1}={2}", allQs, item.Key, item.Value);
                }
            }

            if (!newKeyProcessed && QsValue != "")
            {
                //Wasn't in current URI, needs to be added
                allQs = string.Format("{0}&{1}={2}", allQs, QsKey, QsValue);
            }

            allQs = allQs.Replace("&&", "");

            //Build New Url
            var newUrl = string.Format("{0}?{1}{2}", baseUrl, allQs, newAnchor);
            newUrl = newUrl.Replace("?&", "?"); //Cleanup if all QS have been removed
            //uri.Rewrite(newUrl);

            return newUrl;
        }

        /// <summary>
        /// Updates a URL removing a specific query string value 
        /// </summary>
        /// <param name="OriginalUrl">Url to edit</param>
        /// <param name="QsKeyToRemove">Query String Key name to remove, if present</param>
        /// <returns>URL</returns>
        public static string RemoveQueryStringKeyFromUrl(string OriginalUrl, string QsKeyToRemove)
        {
            var url = new Uri(OriginalUrl);
            return RemoveQueryStringKeyFromUrl(url, QsKeyToRemove);
        }

        /// <summary>
        /// Updates a URL removing a specific query string value
        /// </summary>
        /// <param name="OriginalUri">Uri to edit</param>
        /// <param name="QsKeyToRemove">Query String Key name to remove, if present</param>
        /// <returns>URL</returns>
        public static string RemoveQueryStringKeyFromUrl(Uri OriginalUri, string QsKeyToRemove)
        {
            var uri = OriginalUri;

            var baseUrl = uri.AbsoluteUri.Replace(uri.Query, "");
            // var basePath = uri.AbsolutePath;

            //Anchor Tag
            var currAnchor = uri.Fragment.Replace("#", "");
            //var providedAnchor = NewAnchor.Replace("#", "");
            var newAnchor = "";

            if (currAnchor != "")
            {
                //    if (NewAnchor != "")
                //    {
                //        newAnchor = providedAnchor;
                //    }
                //    else
                //    {
                newAnchor = currAnchor;
                //}

                newAnchor = "#" + newAnchor;
            }
            //else
            //{
            //newAnchor = providedAnchor;
            //}

            //Query String Values
            var qs = GetQueryStringDictionary(uri.Query);
            if (qs.ContainsKey(QsKeyToRemove))
            {
                qs.Remove(QsKeyToRemove);
            }

            var allQs = AssembleQueryString(qs);

            //Build New Url
            var newUrl = string.Format("{0}?{1}{2}", baseUrl, allQs, newAnchor);
            newUrl = newUrl.Replace("?&", "?"); //Cleanup if all QS have been removed

            return newUrl;
        }

        public static string AssembleQueryString(Dictionary<string, string> QueryStringDictionary)
        {
            var allQs = "&";
            foreach (var qs in QueryStringDictionary)
            {
                if (qs.Value != "")
                {
                    //add qs with value
                    allQs = string.Format("{0}&{1}={2}", allQs, qs.Key, qs.Value);
                }
            }
            allQs = allQs.Replace("&&", "");

            return allQs;
        }


        /// <summary>
        /// Rewrites the path of uri.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="path">The new path, which must begin with a slash.</param>
        /// <returns>The rewritten uri.</returns>
        /// <remarks>Everything else remains unchanged, except for the fragment which is removed.</remarks>
        public static Uri Rewrite(this Uri uri, string path)
        {
            //Copied from https://github.com/umbraco/Umbraco-CMS/blob/d50e49ad37fd5ca7bad2fd6e8fc994f3408ae70c/src/Umbraco.Core/UriExtensions.cs

            if (path.StartsWith("/") == false)
                throw new ArgumentException("Path must start with a slash.", "path");

            return uri.IsAbsoluteUri
                ? new Uri(uri.GetLeftPart(UriPartial.Authority) + path + uri.Query)
                : new Uri(path + uri.GetSafeQuery(), UriKind.Relative);
        }

        /// <summary>
        /// Rewrites the path and query of a uri.
        /// </summary>
        /// <param name="uri">The uri.</param>
        /// <param name="path">The new path, which must begin with a slash.</param>
        /// <param name="query">The new query, which must be empty or begin with a question mark.</param>
        /// <returns>The rewritten uri.</returns>
        /// <remarks>Everything else remains unchanged, except for the fragment which is removed.</remarks>
        public static Uri Rewrite(this Uri uri, string path, string query)
        {
            //Copied from https://github.com/umbraco/Umbraco-CMS/blob/d50e49ad37fd5ca7bad2fd6e8fc994f3408ae70c/src/Umbraco.Core/UriExtensions.cs

            if (path.StartsWith("/") == false)
                throw new ArgumentException("Path must start with a slash.", "path");
            if (query.Length > 0 && query.StartsWith("?") == false)
                throw new ArgumentException("Query must start with a question mark.", "query");
            if (query == "?")
                query = "";

            return uri.IsAbsoluteUri
                ? new Uri(uri.GetLeftPart(UriPartial.Authority) + path + query)
                : new Uri(path + query, UriKind.Relative);
        }

        #endregion

    }
}
