using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Data;

namespace Web.Models
{
    public class ListHelper
    {
        public static IEnumerable<SelectListItem> GetCategoryList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.Accounts
                           orderby l.AccountName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.AccountName };

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetSubCategoryList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.AccountSubCategory
                           orderby l.AccountSubCategoryName
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.AccountSubCategoryName };

                return list.ToList();
            }
        }

        public static IEnumerable<SelectListItem> GetAccountClassList()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var list = from l in db.AccountClasses
                           orderby l.Name
                           select new SelectListItem { Value = l.Id.ToString(), Text = l.Name };

                return list.ToList();
            }
        }
    }
}