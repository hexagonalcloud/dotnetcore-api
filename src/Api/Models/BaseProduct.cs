using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Api.Models
{
    public abstract class BaseProduct
    {
        [MaxLength(50)]
        [Required]
        [JsonProperty(Order = 2)]
        public string Name { get; set; }

        [MaxLength(15)]
        [JsonProperty(Order = 3)]
        public string Color { get; set; }

        [Required]
        [JsonProperty(Order = 4)]
        public decimal? ListPrice { get; set; }

        [MaxLength(25)]
        [Required]
        [JsonProperty(Order = 5)]
        public string ProductNumber { get; set; }

        [MaxLength(5)]
        [JsonProperty(Order = 6)]
        public string Size { get; set; }

        [JsonProperty(Order = 7)]
        public decimal? Weight { get; set; }

        [JsonProperty(Order = 8)]
        public int? ProductCategoryId { get; set; }

        [JsonProperty(Order = 9)]
        public int? ProductModelId { get; set; }

        [JsonProperty(Order = 10)]
        public byte[] ThumbNailPhoto { get; set; }
    }
}
