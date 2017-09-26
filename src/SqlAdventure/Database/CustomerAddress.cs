using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlAdventure.Database
{
    [Table("CustomerAddress", Schema = "SalesLT")]
    public partial class CustomerAddress
    {
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("AddressID")]
        public int AddressId { get; set; }
        [Required]
        [Column(TypeName = "Name")]
        public string AddressType { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("AddressId")]
        [InverseProperty("CustomerAddress")]
        public Address Address { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CustomerAddress")]
        public Customer Customer { get; set; }
    }
}
