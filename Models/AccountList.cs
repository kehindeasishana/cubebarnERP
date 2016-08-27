using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Models.ViewModels.Financials;

namespace Web.Models
{
    public class AccountList
    {
        public List<Accounts> _accountlist;

        // [Display(Name = "Account Category")]
        public int AccountCategoryId { get; set; }

        public IEnumerable<SelectListItem> Accounts
        {
            get { return new SelectList(_accountlist, "Id", "AccountName"); }
        }
    }
}