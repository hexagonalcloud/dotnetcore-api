using System;

namespace Api.Models.Public
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public decimal? ListPrice { get; set; }

        public string ProductNumber { get; set; }

        public string Size { get; set; }

        public decimal? Weight { get; set; }

        public string Category { get; set; }

        public string Model { get; set; }

        public byte[] ThumbNailPhoto { get; set; }

        public DateTime SellStartDate { get; set; }

        public DateTime? DiscontinuedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
