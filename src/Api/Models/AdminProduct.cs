using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace Api.Models
{
    [Table("[SalesLT].[Product]")]
    public class AdminProduct : BaseProduct, IWeakEntityTag
    {
        [Key]
        [JsonProperty("Id", Order = 1)]
        public Guid RowGuid { get; set; }

        [JsonProperty(Order = 11)]
        public string ThumbnailPhotoFileName { get; set; }

        [JsonProperty(Order = 12)]
        public decimal StandardCost { get; set; }

        [JsonProperty(Order = 13)]
        public DateTime SellStartDate { get; set; }

        [JsonProperty(Order = 15)]
        public DateTime? SellEndDate { get; set; }

        [JsonProperty(Order = 16)]
        public DateTime? DiscontinuedDate { get; set; }

        [JsonProperty(Order = 17)]
        public DateTime ModifiedDate { get; set; }
    }
}
