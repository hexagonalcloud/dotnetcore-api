using System;
using Dapper.Contrib.Extensions;

namespace SqlAdventure.Db
{
    [Table("[SalesLT].[Product]")]
    public class Product : BaseProduct
    {
        [Key]
        public Guid RowGuid { get; set; }

        public DateTime ModifiedDate { get; set; }

        public DateTime SellStartDate { get; set; }

        public DateTime? DiscontinuedDate { get; set; }
    }
}
