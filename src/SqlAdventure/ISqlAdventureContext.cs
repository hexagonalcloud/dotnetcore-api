using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SqlAdventure.Database;

namespace SqlAdventure
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

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
