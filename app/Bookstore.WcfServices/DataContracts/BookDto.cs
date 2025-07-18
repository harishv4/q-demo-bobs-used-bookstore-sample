using System.Runtime.Serialization;

namespace Bookstore.WcfServices.DataContracts
{
    [DataContract]
    public class BookDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; } = string.Empty;

        [DataMember]
        public string Author { get; set; } = string.Empty;

        [DataMember]
        public string ISBN { get; set; } = string.Empty;

        [DataMember]
        public int PublisherId { get; set; }

        [DataMember]
        public string PublisherName { get; set; } = string.Empty;

        [DataMember]
        public int BookTypeId { get; set; }

        [DataMember]
        public string BookTypeName { get; set; } = string.Empty;

        [DataMember]
        public int GenreId { get; set; }

        [DataMember]
        public string GenreName { get; set; } = string.Empty;

        [DataMember]
        public int ConditionId { get; set; }

        [DataMember]
        public string ConditionName { get; set; } = string.Empty;

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public int Quantity { get; set; }

        [DataMember]
        public int Year { get; set; }

        [DataMember]
        public string? Summary { get; set; }

        [DataMember]
        public string? CoverImageUrl { get; set; }

        [DataMember]
        public bool IsInStock { get; set; }

        [DataMember]
        public bool IsLowInStock { get; set; }

        [DataMember]
        public DateTime CreatedOn { get; set; }

        [DataMember]
        public DateTime? UpdatedOn { get; set; }
    }
}