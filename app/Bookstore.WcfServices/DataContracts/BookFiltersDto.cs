using System.Runtime.Serialization;

namespace Bookstore.WcfServices.DataContracts
{
    [DataContract]
    public class BookFiltersDto
    {
        [DataMember]
        public string? SearchString { get; set; }

        [DataMember]
        public int? PublisherId { get; set; }

        [DataMember]
        public int? BookTypeId { get; set; }

        [DataMember]
        public int? GenreId { get; set; }

        [DataMember]
        public int? ConditionId { get; set; }

        [DataMember]
        public decimal? MinPrice { get; set; }

        [DataMember]
        public decimal? MaxPrice { get; set; }

        [DataMember]
        public bool? InStockOnly { get; set; }

        [DataMember]
        public string? SortBy { get; set; }
    }
}