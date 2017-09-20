using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace Api.Models
{
    [Table("[SalesLT].[Product]")]
    public class Product : BaseProduct, IWeakEntityTag
    {
        [JsonIgnore]
        public DateTime ModifiedDate { get; set; }
    }
}
