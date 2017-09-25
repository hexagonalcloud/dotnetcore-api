using System;

namespace Core.Entities
{
    public class Product : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Color { get; set; }

        public decimal? ListPrice { get; set; }

        public string ProductNumber { get; set; }

        public string Size { get; set; }

        public decimal? Weight { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductModelId { get; set; }

        public byte[] ThumbNailPhoto { get; set; }

        public DateTime SellStartDate { get; set; }

        public DateTime? DiscontinuedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
