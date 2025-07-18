using Bookstore.Domain.Books;
using Bookstore.WcfServices.DataContracts;
using Bookstore.WcfServices.Mappers;

namespace Bookstore.WcfServices
{
    public class BookSearchService : IBookSearchService
    {
        private readonly IBookService _bookService;

        public BookSearchService(IBookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<BookDto> GetBookAsync(int id)
        {
            var book = await _bookService.GetBookAsync(id);
            return BookMapper.ToDto(book);
        }

        public async Task<PaginatedListDto<BookDto>> GetBooksAsync(string searchString, string sortBy, int pageIndex, int pageSize)
        {
            var books = await _bookService.GetBooksAsync(searchString, sortBy, pageIndex, pageSize);
            return BookMapper.ToDto(books, BookMapper.ToDto);
        }

        public async Task<PaginatedListDto<BookDto>> GetBooksByFiltersAsync(BookFiltersDto filters, int pageIndex, int pageSize)
        {
            var bookFilters = BookMapper.ToEntity(filters);
            var books = await _bookService.GetBooksAsync(bookFilters, pageIndex, pageSize);
            return BookMapper.ToDto(books, BookMapper.ToDto);
        }

        public async Task<IEnumerable<BookDto>> ListBestSellingBooksAsync(int count)
        {
            var books = await _bookService.ListBestSellingBooksAsync(count);
            return books.Select(BookMapper.ToDto);
        }

        public async Task<BookStatisticsDto> GetStatisticsAsync()
        {
            var stats = await _bookService.GetStatisticsAsync();
            return BookMapper.ToDto(stats);
        }
    }
}