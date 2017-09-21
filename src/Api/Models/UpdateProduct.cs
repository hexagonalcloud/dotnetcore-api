using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace Api.Models
{
    [Table("[SalesLT].[Product]")]
    public class UpdateProduct : CreateProduct
    {
        [Key]
        [JsonIgnore]
        public override Guid RowGuid { get; set; }
    }
}
