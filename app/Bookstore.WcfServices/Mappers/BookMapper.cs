using Bookstore.Domain;
using Bookstore.Domain.Books;
using Bookstore.WcfServices.DataContracts;

namespace Bookstore.WcfServices.Mappers
{
    internal static class BookMapper
    {
        public static BookDto ToDto(Book book)
        {
            return new BookDto
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                ISBN = book.ISBN,
                PublisherId = book.PublisherId,
                PublisherName = book.Publisher?.Text ?? string.Empty,
                BookTypeId = book.BookTypeId,
                BookTypeName = book.BookType?.Text ?? string.Empty,
                GenreId = book.GenreId,
                GenreName = book.Genre?.Text ?? string.Empty,
                ConditionId = book.ConditionId,
                ConditionName = book.Condition?.Text ?? string.Empty,
                Price = book.Price,
                Quantity = book.Quantity,
                Year = book.Year ?? 0,
                Summary = book.Summary,
                CoverImageUrl = book.CoverImageUrl,
                IsInStock = book.IsInStock,
                IsLowInStock = book.IsLowInStock,
                CreatedOn = book.CreatedOn,
                UpdatedOn = book.UpdatedOn
            };
        }

        public static BookFilters ToEntity(BookFiltersDto dto)
        {
            return new BookFilters
            {
                Name = dto.SearchString,
                PublisherId = dto.PublisherId,
                BookTypeId = dto.BookTypeId,
                GenreId = dto.GenreId,
                ConditionId = dto.ConditionId,
                LowStock = dto.InStockOnly ?? false
            };
        }

        public static BookStatisticsDto ToDto(BookStatistics stats)
        {
            return new BookStatisticsDto
            {
                TotalBooks = stats.StockTotal,
                TotalQuantity = stats.StockTotal,
                LowStockBooks = stats.LowStock,
                OutOfStockBooks = stats.OutOfStock,
                TotalValue = 0 // Not available in the domain model
            };
        }

        public static PaginatedListDto<BookDto> ToDto<T>(IPaginatedList<T> paginatedList, Func<T, BookDto> converter)
        {
            return new PaginatedListDto<BookDto>
            {
                Items = paginatedList.Select(converter).ToList(),
                PageIndex = paginatedList.PageIndex,
                TotalPages = paginatedList.TotalPages,
                TotalCount = paginatedList.Count,
                HasPreviousPage = paginatedList.HasPreviousPage,
                HasNextPage = paginatedList.HasNextPage
            };
        }
    }
}