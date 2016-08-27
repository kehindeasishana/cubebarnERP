using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Financials
{
    public class CompanySetUp : BaseEntity
    {
        public string CompanyName { get; set; }
        public string BusinessType { get; set; }
        public string ShortName { get; set; }
        public string Logo { get; set; }
    }
}
