using System.ComponentModel.DataAnnotations;

namespace SqlAdventure.Db
{
    public abstract class BaseProduct
    {
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
    }
}
