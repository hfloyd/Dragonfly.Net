namespace Dragonfly.NetHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    //using System.Web;
    //using System.Web.Script;
    //using System.Web.Script.Serialization;


    public static class Extensions
    {
        private const string ThisClassName = "Dragonfly.NetHelpers.Extensions";

        //Custom Extension Methods

        #region ======= IEnumerable<T> 

        /// <summary>
        /// Find the index of an item in the IEnumerable collection similar to List&lt;T&gt;.FindIndex()
        /// </summary>
        /// <param name="finder">The Item to locate</param>
        /// <returns>Integer representing the Index position</returns>
        public static int FindIndex<T>(this IEnumerable<T> list, Predicate<T> finder)
        {
            return list.ToList().FindIndex(finder);
        }
        #endregion
    }
}