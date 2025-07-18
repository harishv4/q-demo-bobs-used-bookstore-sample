using Bookstore.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Web.WcfClients
{
    public class PaginatedList<T> : List<T>, IPaginatedList<T>
    {
        public PaginatedList(List<T> items, int count, int pageIndex, int totalPages)
        {
            PageIndex = pageIndex;
            TotalPages = totalPages;
            
            AddRange(items);
            
            HasPreviousPage = PageIndex > 1;
            HasNextPage = PageIndex < TotalPages;
        }

        public int PageIndex { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage { get; }
        public bool HasNextPage { get; }

        public Task PopulateAsync()
        {
            // No need to populate as data is already loaded
            return Task.CompletedTask;
        }

        public IEnumerable<int> GetPageList(int count)
        {
            int start = Math.Max(1, PageIndex - count / 2);
            int end = Math.Min(TotalPages, start + count - 1);
            start = Math.Max(1, end - count + 1);

            return Enumerable.Range(start, end - start + 1);
        }

        public IPaginatedList<TConvertTo> ConvertTo<TConvertTo>(Func<T, TConvertTo> expression)
        {
            var convertedItems = this.Select(expression).ToList();
            return new PaginatedList<TConvertTo>(convertedItems, Count, PageIndex, TotalPages);
        }
    }
}