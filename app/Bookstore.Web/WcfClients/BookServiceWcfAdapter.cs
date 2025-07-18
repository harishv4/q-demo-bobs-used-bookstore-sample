using Bookstore.Domain;
using Bookstore.Domain.Books;
using Bookstore.Domain.ReferenceData;
using Bookstore.WcfServices.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookstore.Web.WcfClients
{
    public class BookServiceWcfAdapter : IBookService
    {
        private readonly IBookSearchServiceClient _bookSearchService;

        public BookServiceWcfAdapter(IBookSearchServiceClient bookSearchService)
        {
            _bookSearchService = bookSearchService;
        }

        public async Task<Book> GetBookAsync(int id)
        {
            var bookDto = await _bookSearchService.GetBookAsync(id);
            return MapToEntity(bookDto);
        }

        public async Task<IPaginatedList<Book>> GetBooksAsync(BookFilters filters, int pageIndex, int pageSize)
        {
            var filtersDto = MapToDto(filters);
            var booksDto = await _bookSearchService.GetBooksByFiltersAsync(filtersDto, pageIndex, pageSize);
            return MapToPaginatedList(booksDto);
        }

        public async Task<IPaginatedList<Book>> GetBooksAsync(string searchString, string sortBy, int pageIndex, int pageSize)
        {
            var booksDto = await _bookSearchService.GetBooksAsync(searchString, sortBy, pageIndex, pageSize);
            return MapToPaginatedList(booksDto);
        }

        public async Task<IEnumerable<Book>> ListBestSellingBooksAsync(int count)
        {
            var booksDto = await _bookSearchService.ListBestSellingBooksAsync(count);
            return booksDto.Select(MapToEntity).ToList();
        }

        public async Task<BookStatistics> GetStatisticsAsync()
        {
            var statsDto = await _bookSearchService.GetStatisticsAsync();
            return MapToEntity(statsDto);
        }

        public Task<BookResult> AddAsync(CreateBookDto createBookDto)
        {
            // This method is not implemented in the WCF service
            throw new NotImplementedException("Adding books is not supported through the WCF service");
        }

        public Task<BookResult> UpdateAsync(UpdateBookDto updateBookDto)
        {
            // This method is not implemented in the WCF service
            throw new NotImplementedException("Updating books is not supported through the WCF service");
        }

        #region Mapping Methods

        private Book MapToEntity(BookDto dto)
        {
            var book = new Book(
                dto.Name,
                dto.Author,
                dto.ISBN,
                dto.PublisherId,
                dto.BookTypeId,
                dto.GenreId,
                dto.ConditionId,
                dto.Price,
                dto.Quantity,
                dto.Year,
                dto.Summary,
                dto.CoverImageUrl);

            // Set properties that aren't in the constructor
            book.Id = dto.Id;
            book.CreatedOn = dto.CreatedOn;
            book.UpdatedOn = dto.UpdatedOn ?? DateTime.MinValue;

            // Initialize navigation properties
            book.Publisher = new ReferenceDataItem(ReferenceDataType.Publisher, dto.PublisherName) { Id = dto.PublisherId };
            book.BookType = new ReferenceDataItem(ReferenceDataType.BookType, dto.BookTypeName) { Id = dto.BookTypeId };
            book.Genre = new ReferenceDataItem(ReferenceDataType.Genre, dto.GenreName) { Id = dto.GenreId };
            book.Condition = new ReferenceDataItem(ReferenceDataType.Condition, dto.ConditionName) { Id = dto.ConditionId };

            return book;
        }

        private BookFiltersDto MapToDto(BookFilters filters)
        {
            return new BookFiltersDto
            {
                SearchString = filters.Name,
                PublisherId = filters.PublisherId,
                BookTypeId = filters.BookTypeId,
                GenreId = filters.GenreId,
                ConditionId = filters.ConditionId,
                InStockOnly = !filters.LowStock
            };
        }

        private BookStatistics MapToEntity(BookStatisticsDto dto)
        {
            return new BookStatistics
            {
                StockTotal = dto.TotalBooks,
                LowStock = dto.LowStockBooks,
                OutOfStock = dto.OutOfStockBooks
            };
        }

        private IPaginatedList<Book> MapToPaginatedList(PaginatedListDto<BookDto> dto)
        {
            var items = dto.Items.Select(MapToEntity).ToList();
            return new PaginatedList<Book>(
                items,
                dto.TotalCount,
                dto.PageIndex,
                dto.TotalPages);
        }

        #endregion
    }
}