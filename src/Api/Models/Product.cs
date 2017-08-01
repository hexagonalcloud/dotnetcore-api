using Microsoft.AspNetCore.Mvc;

namespace Api.Models
{
    public class Product
    {
        // ReSharper disable once InconsistentNaming
        public int ProductID { get; set; }
        public string Name { get; set; }
    }
}
