using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public sealed class UpdateProduct: IBaseEntity
    {
        public UpdateProduct()
        {
        }

        public UpdateProduct(AdminProduct product)
        {
            Id = product.Id;
            DiscontinuedDate = product.DiscontinuedDate;
            SellEndDate = product.SellEndDate;
            SellStartDate = product.SellStartDate;
            StandardCost = product.StandardCost;
            ThumbnailPhotoFileName = product.ThumbnailPhotoFileName;
            Color = product.Color;
            ListPrice = product.ListPrice;
            Name = product.Name;
            ProductCategoryId = product.ProductCategoryId;
            ProductModelId = product.ProductModelId;
            ProductNumber = product.ProductNumber;
            Size = product.Size;
            ThumbNailPhoto = product.ThumbNailPhoto;
            Weight = product.Weight;
        }

        public Guid Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(15)]
        public string Color { get; set; }

        [Required]
        public decimal? ListPrice { get; set; }

        [MaxLength(25)]
        [Required]
        public string ProductNumber { get; set; }

        [MaxLength(5)]
        public string Size { get; set; }

        public decimal? Weight { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductModelId { get; set; }

        public byte[] ThumbNailPhoto { get; set; }

        [MaxLength(50)]
        public string ThumbnailPhotoFileName { get; set; }

        [Required]
        public decimal StandardCost { get; set; }

        [Required]
        public DateTime SellStartDate { get; set; }

        public DateTime? SellEndDate { get; set; }

        public DateTime? DiscontinuedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
