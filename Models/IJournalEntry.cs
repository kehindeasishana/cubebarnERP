using Core.Domain.Financials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Models
{
    interface IJournalEntry
    {
        List<SelectListItem> GetCategoryOptions();
        JournalEntryLine GetAccount();
    }
}
