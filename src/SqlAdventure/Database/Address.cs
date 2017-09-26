using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlAdventure.Database
{
    [Table("Address", Schema = "SalesLT")]
    public partial class Address
    {
        public Address()
        {
            CustomerAddress = new HashSet<CustomerAddress>();
            SalesOrderHeaderBillToAddress = new HashSet<SalesOrderHeader>();
            SalesOrderHeaderShipToAddress = new HashSet<SalesOrderHeader>();
        }

        [Column("AddressID")]
        public int AddressId { get; set; }
        [Required]
        [StringLength(60)]
        public string AddressLine1 { get; set; }
        [StringLength(60)]
        public string AddressLine2 { get; set; }
        [Required]
        [StringLength(30)]
        public string City { get; set; }
        [Required]
        [Column(TypeName = "Name")]
        public string StateProvince { get; set; }
        [Required]
        [Column(TypeName = "Name")]
        public string CountryRegion { get; set; }
        [Required]
        [StringLength(15)]
        public string PostalCode { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [InverseProperty("Address")]
        public ICollection<CustomerAddress> CustomerAddress { get; set; }
        [InverseProperty("BillToAddress")]
        public ICollection<SalesOrderHeader> SalesOrderHeaderBillToAddress { get; set; }
        [InverseProperty("ShipToAddress")]
        public ICollection<SalesOrderHeader> SalesOrderHeaderShipToAddress { get; set; }
    }
}
