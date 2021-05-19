using System;

namespace Expenses.Application.Common
{
    public abstract class QueryStringParameter
    {
        private const int _pageSizeMax = 5;
        private int _pageSize = _pageSizeMax;

        public int PageIndex { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Min(_pageSizeMax, Math.Abs(value));
        }
    }
}
