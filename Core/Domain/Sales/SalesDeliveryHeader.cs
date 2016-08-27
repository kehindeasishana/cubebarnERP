using Core.Domain.Financials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Sales
{
//    [Table("SalesDeliveryHeader")]
    public partial class SalesDeliveryHeader : BaseEntity
    {
        public SalesDeliveryHeader()
        {
            SalesDeliveryLines = new HashSet<SalesDeliveryLine>();
        }

        public int? PaymentTermId { get; set; }
        public int? CustomerId { get; set; }
        public int? GeneralLedgerHeaderId { get; set; }
        public int? SalesOrderHeaderId { get; set; }
        public string No { get; set; }
        public DateTime Date { get; set; }
        public virtual PaymentTerm PaymentTerm { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual SalesOrderHeader SalesOrderHeader { get; set; }

        public virtual ICollection<SalesDeliveryLine> SalesDeliveryLines { get; set; }
    }
}
