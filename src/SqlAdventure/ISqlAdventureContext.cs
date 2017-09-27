using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace SqlAdventure.Database
{
    public interface ISqlAdventureContext
    {
        DbSet<Address> Address { get; set; }
        DbSet<Customer> Customer { get; set; }
        DbSet<CustomerAddress> CustomerAddress { get; set; }
        DbSet<Product> Product { get; set; }
        DbSet<ProductCategory> ProductCategory { get; set; }
        DbSet<ProductDescription> ProductDescription { get; set; }
        DbSet<ProductModel> ProductModel { get; set; }
        DbSet<ProductModelProductDescription> ProductModelProductDescription { get; set; }
        DbSet<SalesOrderDetail> SalesOrderDetail { get; set; }
        DbSet<SalesOrderHeader> SalesOrderHeader { get; set; }
    }
}
