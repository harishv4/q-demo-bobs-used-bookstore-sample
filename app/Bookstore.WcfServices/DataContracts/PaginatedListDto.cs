using System.Runtime.Serialization;

namespace Bookstore.WcfServices.DataContracts
{
    [DataContract]
    public class PaginatedListDto<T>
    {
        [DataMember]
        public List<T> Items { get; set; } = new List<T>();

        [DataMember]
        public int PageIndex { get; set; }

        [DataMember]
        public int TotalPages { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public bool HasPreviousPage { get; set; }

        [DataMember]
        public bool HasNextPage { get; set; }
    }
}