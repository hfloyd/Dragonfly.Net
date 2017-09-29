namespace Dragonfly.NetHelpers
{
    using System;
    using System.Text;
    using System.Web;

    public static class Randomize
    {
        private const string ThisClassName = "Dragonfly.NetHelpers.Randomize";

        /// <summary>
        /// Quick way to get an already instantiated Random
        /// To Use: Add this code to the UmbracoEvents.cs file in ApplicationStarting() method:
        /// System.Web.HttpContext.Current.Application.Lock();
        /// System.Web.HttpContext.Current.Application["AppRandom"] = new Random();
        /// System.Web.HttpContext.Current.Application.UnLock();
        /// LogHelper.Info&lt;UmbracoEvents&gt;("System.Web.HttpContext.Current.Application[\"AppRandom\"] has been instantiated.");
        /// </summary>
        /// <returns></returns>
        public static Random GetAppRandomizer()
        {
            return (Random)HttpContext.Current.Application["AppRandom"];
        }

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <param name="InstantiatedRandom">An instance of 'Random' for the application. 
        /// If you add to Application_Started():
        /// System.Web.HttpContext.Current.Application["AppRandom"] = new Random();
        /// Then you can pass-in:
        /// "System.Web.HttpContext.Current.Application["AppRandom"] as Random"</param>
        /// <returns>Random string</returns>
        public static string RandomString(int size, bool lowerCase, Random InstantiatedRandom = null)
        {
            //Random randSeed = new Random();
            //var seed = randSeed.Next(1, Int32.MaxValue);
            //LogHelper.Info<string>("RandomString SEED = " + seed);
            char ch;
            StringBuilder builder = new StringBuilder();

            Random random = InstantiatedRandom != null ? InstantiatedRandom : new Random();

            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }


        public static int GetRandomInteger()
        {
            Random RandomObj = new Random();
            int RandomNumber = RandomObj.Next();

            return RandomNumber;
        }

        public static string GetRandomNumericString()
        {
            int RandomNumber = GetRandomInteger();

            return Convert.ToString(RandomNumber);
        }
    }
}
