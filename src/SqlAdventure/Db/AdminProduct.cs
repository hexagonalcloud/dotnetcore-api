using System;
using Dapper.Contrib.Extensions;

namespace SqlAdventure.Db
{
    [Table("[SalesLT].[Product]")]
    public class AdminProduct : BaseProduct
    {
        [Key]
        public Guid RowGuid { get; set; }

        public string ThumbnailPhotoFileName { get; set; }

        public decimal StandardCost { get; set; }

        public DateTime SellStartDate { get; set; }

        public DateTime? SellEndDate { get; set; }

        public DateTime? DiscontinuedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
