using System;
using System.Collections.Generic;

namespace Expenses.Application.Common
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(IList<T> items, QueryStringParameter parameter, int count)
        {
            TotalCount = count;
            PageSize = parameter.PageSize;
            CurrentPage = parameter.PageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)parameter.PageSize);

            AddRange(items);
        }
    }
}
