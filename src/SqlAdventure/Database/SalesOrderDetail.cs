﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SqlAdventure.Database
{
    [Table("SalesOrderDetail", Schema = "SalesLT")]
    public partial class SalesOrderDetail
    {
        [Column("SalesOrderID")]
        public int SalesOrderId { get; set; }
        [Column("SalesOrderDetailID")]
        public int SalesOrderDetailId { get; set; }
        public short OrderQty { get; set; }
        [Column("ProductID")]
        public int ProductId { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPrice { get; set; }
        [Column(TypeName = "money")]
        public decimal UnitPriceDiscount { get; set; }
        [Column(TypeName = "numeric(, 6)")]
        public decimal LineTotal { get; set; }
        [Column("rowguid")]
        public Guid Rowguid { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedDate { get; set; }

        [ForeignKey("ProductId")]
        [InverseProperty("SalesOrderDetail")]
        public Product Product { get; set; }
        [ForeignKey("SalesOrderId")]
        [InverseProperty("SalesOrderDetail")]
        public SalesOrderHeader SalesOrder { get; set; }
    }
}
