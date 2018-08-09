namespace Dragonfly.NetModels
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class PagingCollection<T> : IEnumerable<T>
    {
        private const string ThisClassName = "Dragonfly.NetModels.PagingCollection<T>";

        #region fields

        private const int DefaultPageSize = 10;

        private readonly IEnumerable<T> _collection;

        private int _pageSize = DefaultPageSize;

        private IEnumerable<CollectionPage<T>> _pages;

        #endregion

        #region properties

        /// <summary>
        /// Gets or sets page size
        /// </summary>
        public int PageSize
        {
            get
            {
                return this._pageSize;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }
                this._pageSize = value;
            }
        }

        /// <summary>
        /// Gets pages count
        /// </summary>
        public int PagesCount
        {
            get
            {
                return (int)Math.Ceiling(this._collection.Count() / (decimal)this.PageSize);
            }
        }

        public IEnumerable<CollectionPage<T>> Pages
        {
            get
            {
                return this._pages;
            }
        }

        #endregion

        #region ctor

        /// <summary>
        /// Creates paging collection and sets page size
        /// </summary>
        public PagingCollection(IEnumerable<T> collection, int pageSize)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }
            this.PageSize = pageSize;
            this._collection = collection.ToArray();

            //divide results into pages
            var totalItems = collection.Count();
            //var numPages = totalResults % pageLength == 0 ? totalResults / pageLength : totalResults / pageLength + 1;
            var pagesList = new List<CollectionPage<T>>();
            if (pageSize > 0)
            {
                var toSkip = 0;
                for (int i = 0; i < this.PagesCount; i++)
                {
                    var page = new CollectionPage<T>();

                    page.PageNumber = i + 1;
                    toSkip = i * pageSize;
                    page.Collection = this.GetData(i);
                    page.ResultsOnPage = page.Collection.Count();
                    page.FirstResult = toSkip + 1;

                    var lastResult = toSkip + pageSize;
                    if (lastResult > totalItems)
                    {
                        lastResult = totalItems;
                    }

                    page.LastResult = lastResult;

                    pagesList.Add(page);
                }
            }


            //for (int i = 1; i <= this.PagesCount; i++)
            //{
            //    var newPage = new CollectionPage<T>();
            //    newPage.PageNumber = i;
            //    newPage.Collection = this.GetData(i);

            //    pagesList.Add(newPage);
            //}

            this._pages = pagesList;
        }

        /// <summary>
        /// Creates paging collection
        /// </summary>
        public PagingCollection(IEnumerable<T> collection)
            : this(collection, DefaultPageSize)
        {
        }

        #endregion

        #region public methods

        /// <summary>
        /// Returns data by page number
        /// </summary>
        public IEnumerable<T> GetData(int pageNumber)
        {
            if (pageNumber < 0 || pageNumber > this.PagesCount)
            {
                return new T[] { };
            }

            int offset = (pageNumber - 1) * this.PageSize;

            return this._collection.Skip(offset).Take(this.PageSize);
        }

        /// <summary>
        /// Returns number of items on page by number
        /// </summary>
        public int GetCount(int pageNumber)
        {
            return this.GetData(pageNumber).Count();
        }

        #endregion

        #region static methods

        /// <summary>
        /// Returns data by page number and page size
        /// </summary>
        public static IEnumerable<T> GetPaging(IEnumerable<T> collection, int pageNumber, int pageSize)
        {
            return new PagingCollection<T>(collection, pageSize).GetData(pageNumber);
        }

        /// <summary>
        /// Returns data by page number
        /// </summary>
        public static IEnumerable<T> GetPaging(IEnumerable<T> collection, int pageNumber)
        {
            return new PagingCollection<T>(collection, DefaultPageSize).GetData(pageNumber);
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

    public class CollectionPage<T>
    {
        public int PageNumber { get; set; }
        public int ResultsOnPage { get; set; }
        public int FirstResult { get; set; }
        public int LastResult { get; set; }
        public IEnumerable<T> Collection;
    }
}