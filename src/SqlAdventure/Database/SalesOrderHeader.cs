﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlAdventure.Database
{
    [Table("SalesOrderHeader", Schema = "SalesLT")]
    public partial class SalesOrderHeader
    {
        public SalesOrderHeader()
        {
            SalesOrderDetail = new HashSet<SalesOrderDetail>();
        }

        [Key]
        [Column("SalesOrderID")]
        public int SalesOrderId { get; set; }
        public byte RevisionNumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OrderDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DueDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ShipDate { get; set; }
        public byte Status { get; set; }
        [Column(TypeName = "Flag")]
        public bool? OnlineOrderFlag { get; set; }
        [Required]
        [StringLength(25)]
        public string SalesOrderNumber { get; set; }
        [Column(TypeName = "OrderNumber")]
        public string PurchaseOrderNumber { get; set; }
        [Column(TypeName = "AccountNumber")]
        public string AccountNumber { get; set; }
        [Column("CustomerID")]
        public int CustomerId { get; set; }
        [Column("ShipToAddressID")]
        public int? ShipToAddressId { get; set; }
        [Column("BillToAddressID")]
        public int? BillToAddressId { get; set; }
        [Required]
        [StringLength(50)]
        public string ShipMethod { get; set; }
        [StringLength(15)]
        public string CreditCardApprovalCode { get; set; }
        [Column(TypeName = "money")]
        public decimal SubTotal { get; set; }
        [Column(TypeName = "money")]
        public decimal TaxAmt { get; set; }
        [Column(TypeName = "money")]
        public decimal Freight { get; set; }
        [Column(TypeName = "money")]
        public decimal TotalDue { get; set; }
        public string Comment { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("BillToAddressId")]
        [InverseProperty("SalesOrderHeaderBillToAddress")]
        public Address BillToAddress { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("SalesOrderHeader")]
        public Customer Customer { get; set; }
        [ForeignKey("ShipToAddressId")]
        [InverseProperty("SalesOrderHeaderShipToAddress")]
        public Address ShipToAddress { get; set; }
        [InverseProperty("SalesOrder")]
        public ICollection<SalesOrderDetail> SalesOrderDetail { get; set; }
    }
}
