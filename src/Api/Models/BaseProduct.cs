using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace Api.Models
{
    public abstract class BaseProduct
    {
        [Key]
        [JsonProperty("Id", Order = 1)]
        public Guid RowGuid { get; set; }

        [JsonIgnore]
        public int ProductId { get; set; }

        [JsonProperty(Order = 2)]
        public string Name { get; set; }

        [JsonProperty(Order = 3)]
        public string Color { get; set; }

        [JsonProperty(Order = 4)]
        public decimal ListPrice { get; set; }

        [JsonProperty(Order = 5)]
        public string ProductNumber { get; set; }

        [JsonProperty(Order = 6)]
        public string Size { get; set; }

        [JsonProperty(Order = 7)]
        public decimal Weight { get; set; }

        [JsonProperty(Order = 8)]
        public int ProductCategoryId { get; set; }

        [JsonProperty(Order = 9)]
        public int ProductModelId { get; set; }

        [JsonProperty(Order = 10)]
        public byte[] ThumbNailPhoto { get; set; }
    }
}
