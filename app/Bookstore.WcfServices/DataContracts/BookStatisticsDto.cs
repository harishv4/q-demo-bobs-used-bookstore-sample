using System.Runtime.Serialization;

namespace Bookstore.WcfServices.DataContracts
{
    [DataContract]
    public class BookStatisticsDto
    {
        [DataMember]
        public int TotalBooks { get; set; }

        [DataMember]
        public int TotalQuantity { get; set; }

        [DataMember]
        public int LowStockBooks { get; set; }

        [DataMember]
        public int OutOfStockBooks { get; set; }

        [DataMember]
        public decimal TotalValue { get; set; }
    }
}