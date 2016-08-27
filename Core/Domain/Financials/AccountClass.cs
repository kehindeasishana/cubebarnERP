using Core.Domain.Financials;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Financials
{

    public class AccountClass : BaseEntity
    {
        public AccountClass()
        {
            Accounts = new HashSet<Account>();
        }

        public string Name { get; set; }
        public string NormalBalance { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        //public virtual ICollection<AccountSubCategory> SubCategory { get; set; }
    }
}
