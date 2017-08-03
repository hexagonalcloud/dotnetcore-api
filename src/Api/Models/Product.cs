using System;

namespace Api.Models
{
    public class Product: IWeakEntityTag
    {
        // ReSharper disable once InconsistentNaming
        public int ProductID { get; set; }
        public string Name { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
