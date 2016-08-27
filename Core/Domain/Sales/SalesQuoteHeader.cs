using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Sales
{
//    [Table("SalesQuoteHeader")]
    public partial class SalesQuoteHeader : BaseEntity
    {
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<SalesQuoteLine> SalesQuoteLines { get; set; }
        public SalesQuoteHeader()
        {
            SalesQuoteLines = new HashSet<SalesQuoteLine>();
        }
    }
}
