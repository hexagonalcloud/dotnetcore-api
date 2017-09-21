using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace Api.Models
{
    [Table("[SalesLT].[Product]")]
    public class Product : BaseProduct, IWeakEntityTag
    {
        [Key]
        [JsonProperty("Id", Order = 1)]
        public Guid RowGuid { get; set; }

        [JsonIgnore]
        public DateTime ModifiedDate { get; set; }

        [JsonIgnore]
        public DateTime SellStartDate { get; set; }

        [JsonIgnore]
        public DateTime? DiscontinuedDate { get; set; }
    }
}
