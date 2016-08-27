using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Financials
{
    public class AccountSubCategory : BaseEntity
    {
        [Display(Name="Account Sub Category Name")]
        public string AccountSubCategoryName { get; set; }
        public string Description { get; set; }
        public int AccountId { get; set; }
        //public int AccountClassId { get; set; }
        public virtual Account Accounts { get; set; }
        //public virtual AccountClass accountClass { get; set; }
        public virtual ICollection<JournalEntryLine> journalEntryLine { get; set; }
    }
}
