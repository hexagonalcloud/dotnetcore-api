using System;
using Dapper.Contrib.Extensions;
using Newtonsoft.Json;

namespace Api.Models
{
    [Table("[SalesLT].[Product]")]
    public class Product: IWeakEntityTag
    {
        // ReSharper disable once InconsistentNaming
        [Key]
        public int ProductID { get; set; }

        public string Name { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
