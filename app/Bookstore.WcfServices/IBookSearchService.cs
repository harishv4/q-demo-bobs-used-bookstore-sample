using Bookstore.WcfServices.DataContracts;
using CoreWCF;

namespace Bookstore.WcfServices
{
    [ServiceContract(Namespace = "http://tempuri.org/")]
    public interface IBookSearchService
    {
        [OperationContract(Action = "http://tempuri.org/IBookSearchService/GetBook")]
        Task<BookDto> GetBookAsync(int id);
        
        [OperationContract(Action = "http://tempuri.org/IBookSearchService/GetBooks")]
        Task<PaginatedListDto<BookDto>> GetBooksAsync(string searchString, string sortBy, int pageIndex, int pageSize);
        
        [OperationContract(Action = "http://tempuri.org/IBookSearchService/GetBooksByFilters")]
        Task<PaginatedListDto<BookDto>> GetBooksByFiltersAsync(BookFiltersDto filters, int pageIndex, int pageSize);
        
        [OperationContract(Action = "http://tempuri.org/IBookSearchService/ListBestSellingBooks")]
        Task<IEnumerable<BookDto>> ListBestSellingBooksAsync(int count);
        
        [OperationContract(Action = "http://tempuri.org/IBookSearchService/GetStatistics")]
        Task<BookStatisticsDto> GetStatisticsAsync();
    }
}