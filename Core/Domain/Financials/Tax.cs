using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Financials
{
    public class Tax : BaseEntity
    {
        public string TaxName { get; set; }
        public string TaxType { get; set; }
        public string TaxCode { get; set; }
        public string TaxRate { get; set; }
        public bool IsActive { get; set; }
    }
}
