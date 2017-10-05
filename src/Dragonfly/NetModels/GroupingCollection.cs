using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Dragonfly.NetModels
{
    public class GroupingCollection<T> : IEnumerable<T>
    {
        private const string ThisClassName = "Dragonfly.NetModels.GroupingCollection<T>";

        #region fields

        //private const int DefaultPageSize = 10;

        private readonly IEnumerable<T> _collection;

        //private int _pageSize = DefaultPageSize;

        private IEnumerable<Group<T>> _groups;


        #endregion

        #region properties

        /// <summary>
        /// Gets groups count
        /// </summary>
        public int GroupsCount
        {
            get
            {
                //return (int)Math.Ceiling(this._collection.Count() / (decimal)this.PageSize);
                return this.Groups.Count();
            }
        }

        public IEnumerable<Group<T>> Groups
        {
            get { return this._groups; }
        }

        #endregion

        #region ctor

        /// <summary>
        /// Creates collection 
        /// </summary>
        public GroupingCollection(IEnumerable<T> InitialCollection)
        {
            if (InitialCollection == null)
            {
                throw new ArgumentNullException("InitialCollection");
            }

            this._collection = InitialCollection.ToArray();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Groups Items in initial collection by provided predicate
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="GroupingProperty"></param>
        public void GroupItems<TKey>(Func<T, TKey> GroupingProperty)
        {
            var query = (from item in this._collection select item).GroupBy(GroupingProperty);

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

            _groups = groups;
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
            var returnGroup = _groups.Where(g => g.GroupName.Values.Contains(GroupValue)).FirstOrDefault();

            return returnGroup;
        }

        public int GetGroupIndex(Group<T> group)
        {

            return this._groups.ToList().FindIndex(g => g == group);

        }

        #endregion

        #region IEnumerable<T> Members

        /// <summary>
        /// Returns an enumerator that iterates through collection
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return this._collection.GetEnumerator();
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



    //    #region public methods

    //    /// <summary>
    //    /// Returns data by page number
    //    /// </summary>
    //    public IEnumerable<T> GetData(int pageNumber)
    //    {
    //        if (pageNumber < 0 || pageNumber > this.PagesCount)
    //        {
    //            return new T[] { };
    //        }

    //        int offset = (pageNumber - 1) * this.PageSize;

    //        return this._collection.Skip(offset).Take(this.PageSize);
    //    }


    public class Group<T>
    {
        //public int GroupIndex { get; set; }

        //public string GroupKey { get; set; }

        public IDictionary<string, object> GroupName { get; internal set; }

        public IEnumerable<T> Collection { get; internal set; }
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
