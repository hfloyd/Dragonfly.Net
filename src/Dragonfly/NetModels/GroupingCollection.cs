using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dragonfly.NetModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GroupingCollection<T> : IEnumerable<T>
    {
        private const string ThisClassName = "Dragonfly.NetModels.GroupingCollection<T>";

        #region properties

        /// <summary>
        /// Original collection of elements
        /// </summary>
        public IEnumerable<T> InitialCollection { get; set; }

        /// <summary>
        /// Grouped collection of elements - if Grouping has been performed
        /// </summary>
        public IEnumerable<Group<T>> Groups { get; set; }

        /// <summary>
        /// Gets groups count
        /// </summary>
        public int GroupsCount
        {
            get
            {
                return this.Groups.Count();
            }
        }

        //public IEnumerable<Group<T>> Groups
        //{
        //    get { return this.GroupedCollection; }
        //}

        #endregion

        #region ctor

        /// <summary>
        /// Emptry Constructor to allow for Serialization/Deserialization
        /// </summary>
        public GroupingCollection()
        {
        }

        /// <summary>
        /// Creates initial collection - YOU MUST CALL THE "GroupItems" METHOD IN ORDER TO GROUP
        /// </summary>
        public GroupingCollection(IEnumerable<T> InitialCollection)
        {
            if (InitialCollection == null)
            {
                throw new ArgumentNullException("InitialCollection");
            }

            this.InitialCollection = InitialCollection; //.ToArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Groups Items in initial collection by provided predicate (replaces current grouping)
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="GroupingProperty"></param>
        public void GroupItems<TKey>(Func<T, TKey> GroupingProperty)
        {
            var query = (from item in this.InitialCollection select item).GroupBy(GroupingProperty);

            var groups = new List<Group<T>>();

            foreach (var nameGroup in query)
            {
                var thisGroup = new Group<T>();
                if (nameGroup.Key.ToString().Contains("{"))
                {
                    //multiple keys
                    thisGroup.GroupName = nameGroup.Key.ToDictionary();
                }
                else
                {
                    //single key
                    thisGroup.GroupName = new Dictionary<string, object>();
                    thisGroup.GroupName.Add("Key", nameGroup.Key);
                }

                var items = new List<T>();

                foreach (var spec in nameGroup)
                {
                    items.Add(spec);
                }

                thisGroup.Collection = items;

                groups.Add(thisGroup);
            }

            Groups = groups;
        }

        /// <summary>
        /// Returns group by Key Value
        /// </summary>
        public Group<T> GetGroup(object GroupValue)
        {
            //if (GroupsCount == 0)
            //{
            //    this.GroupItems(GroupingProperty);
            //}
            var returnGroup = Groups.Where(g => g.GroupName.Values.Contains(GroupValue)).FirstOrDefault();

            return returnGroup;
        }

        public int GetGroupIndex(Group<T> group)
        {

            return this.Groups.ToList().FindIndex(g => g == group);

        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through initial collection
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return this.InitialCollection.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through collection
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }


    /// <summary>
    /// Collection of Grouped elements
    /// </summary>
    /// <typeparam name="T">Type of elements in collection</typeparam>
    public class Group<T>
    {
        //public int GroupIndex { get; set; }

        //public string GroupKey { get; set; }

        public IDictionary<string, object> GroupName { get; set; }

        public IEnumerable<T> Collection { get; set; }
    }

    internal static class ObjectExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object o)
        {
            var asObjectDictionary = o as IDictionary<string, object>;
            if (asObjectDictionary != null)
                return asObjectDictionary;
            var asStringDictionary = o as IDictionary<string, string>;
            if (asStringDictionary != null)
                return asStringDictionary.ToDictionary(x => x.Key, x => (object)x.Value);

            if (o != null)
            {
                var props = TypeDescriptor.GetProperties(o);
                var d = new Dictionary<string, object>();
                foreach (var prop in props.Cast<PropertyDescriptor>())
                {
                    var val = prop.GetValue(o);
                    if (val != null)
                    {
                        d.Add(prop.Name, val);
                    }
                }
                return d;
            }
            return new Dictionary<string, object>();
        }
    }
}
