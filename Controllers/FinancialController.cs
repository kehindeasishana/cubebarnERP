using Services.Financial;
using System.Web.Mvc;
using System;
using System.Linq;
using Core.Domain.Financials;
using Web.Models.ViewModels.Financials;
using System.Collections.Generic;
using Web.Models.ViewModels.Administration;
using PagedList;
using PagedList.Mvc;
using System.Web;
using System.IO;

namespace Web.Controllers
{
    //[Authorize(Roles= "SuperAdmin, Account Manager, CFO, Accountant, Chief Financial Officer, Inventory Manager, Store Manager, Store Keeper")]
   [Authorize(Roles = "SuperAdmin")]
    public class FinancialController : BaseController
    {
        private readonly IFinancialService _financialService;
       
       [Audit]
        public ActionResult Accounts(string sortOn, string orderBy,
        string pSortOn, string keyword, int? page)
        {
            int recordsPerPage = 3;
            if (!page.HasValue)
            {
                page = 1; // set initial page value
                if (string.IsNullOrWhiteSpace(orderBy) || orderBy.Equals("asc"))
                {
                    orderBy = "desc";
                }
                else
                {
                    orderBy = "asc";
                }
            }

            if (!string.IsNullOrWhiteSpace(sortOn) && !sortOn.Equals(pSortOn,
StringComparison.CurrentCultureIgnoreCase))
            {
                orderBy = "asc";
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SortOn = sortOn;
            ViewBag.Keyword = keyword;
            var accounts = _financialService.GetAccounts();
            //var list = db.PersonalDetails.AsQueryable();

            switch (sortOn)
            {
                case "Category":
                    if (orderBy.Equals("desc"))
                    {
                        accounts = accounts.OrderByDescending(p => p.AccountName);
                        //list = list.OrderByDescending(p => p.FirstName);
                    }
                    else
                    {
                        accounts = accounts.OrderBy(p => p.AccountName);
                        //list = list.OrderBy(p => p.FirstName);
                    }
                    break;
                case "AccountName":
                    if (orderBy.Equals("desc"))
                    {
                        accounts = accounts.OrderByDescending(p => p.AccountClass.Name);
                        //list = list.OrderByDescending(p => p.LastName);
                    }
                    else
                    {
                        accounts = accounts.OrderBy(p => p.AccountClass.Name);
                        //list = list.OrderBy(p => p.LastName);
                    }
                    break;
                case "AccountCode":
                    if (orderBy.Equals("desc"))
                    {
                        accounts = accounts.OrderByDescending(p => p.AccountCode);
                        //list = list.OrderByDescending(p => p.Age);
                    }
                    else
                    {
                        accounts = accounts.OrderBy(p => p.AccountCode);
                       // list = list.OrderBy(p => p.Age);
                    }
                    break;
                default:
                    //list = list.OrderBy(p => p.AutoId);
                    accounts = accounts.OrderBy(p => p.Id);
                    break;
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                accounts = accounts.Where(f=>f.AccountClass.Name.StartsWith(keyword));
                //list = list.Where(f => f.FirstName.StartsWith(keyword));
            }
            var finalAccounts = accounts.ToPagedList(page.Value, recordsPerPage);
            //var finalList = list.ToPagedList(page.Value, recordsPerPage);
            //return View(finalList);
            return View(finalAccounts);
        }
        public FinancialController(IFinancialService financialService)
        {
            _financialService = financialService;
            
        }

        public ActionResult Company()
        {
            var company = _financialService.GetDefaultCompany();

            return View(company);
        }
        public ActionResult AddCompany()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult AddCompany(CompanySetUp model)
        {
            _financialService.AddCompany(new CompanySetUp()
            {
                CompanyName = model.CompanyName,
                BusinessType = model.BusinessType,
                ShortName = model.ShortName,
                Logo = model.Logo
            });
            return RedirectToAction("Company");
        }
        public ActionResult EditCompany()
        {
            return View();
        }
        [Audit]
        //[HttpPost]
        //public ActionResult EditCompany()
        //{
        //    return View();
        //}
        public ActionResult AddTax()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult AddTax(Tax model)
        {
            _financialService.AddNewTax(new Tax()
            {
                TaxCode = model.TaxCode,
                TaxName = model.TaxName,
                TaxRate = model.TaxRate,
                TaxType = model.TaxType
            });
            return View();
        }
        public ActionResult Tax()
        {
            var tax = _financialService.GetAllTax();
            return View(tax);
        }
        
        [Audit]
        public ActionResult Banks()
        {
            var banks = _financialService.Banks();
            return View(banks);
        }
        public ActionResult AddBank()
        {
            return View();
        }
        [Audit]
        [HttpPost]
        public ActionResult AddBank(Bank model)
        {
            _financialService.AddBank(new Bank()
            {
                BankBranch = model.BankBranch,
                AccountId = model.AccountId,
                IsActive = model.IsActive,
                Address = model.Address,
                Name = model.Name,
                Number = model.Number,
                Type = model.Type
            });
            return RedirectToAction("Banks");
        }
        public ActionResult EditBank()
        {
            return View();
        }

       [Audit]
       public ActionResult AddFinancialYear()
        {
            return View();
        }
       [HttpPost]
       public ActionResult AddFinancialYear(FinancialYear model)
       {
         
           _financialService.AddFiscalYear(new FinancialYear()
               {
                   FiscalYearName = model.FiscalYearName,
                   FiscalYearCode = model.FiscalYearCode,
                   StartDate = model.StartDate,
                   EndDate = model.EndDate,
                   IsActive = model.IsActive
               });
           return RedirectToAction("Index");
       }
        [Audit]
        //public ActionResult Accounts(string SearchName, string SearchCode, string SearchCategory)
        //{
          
        //    var accounts = _financialService.GetAccounts();
           
        //    var model = new Accounts();

           
        //    foreach(var account in accounts)
        //    {
        //        if (!String.IsNullOrEmpty(SearchName))
        //        {
        //            accounts = accounts.Where(s => s.AccountName.Contains(SearchName));
        //            //return View(accounts.Where(x => x.AccountClass.Name == SearchName || SearchName == null).ToList());
        //        }

        //        else if (!String.IsNullOrEmpty(SearchCode))
        //        {
        //            accounts = accounts.Where(s => s.AccountCode.Contains(SearchCode));
        //            //return View(accounts.Where(x => x.AccountCode == SearchCode || SearchCode == null).ToList());
        //        }
        //        else
        //        {
        //            accounts = accounts.Where(s => s.AccountClass.Name.Contains(SearchCategory));
        //            //return View(accounts.Where(x => x.AccountName == SearchName || SearchName == null).ToList());
        //        }
          
        //        model.AccountsListLines.Add(new AccountsListLine()
        //        {
        //            Id = account.Id,
        //            AccountCode = account.AccountCode,
        //            AccountName = account.AccountName,

        //            Balance = account.Balance,
        //            DebitBalance = account.DebitBalance,
        //            CreditBalance = account.CreditBalance
        //        });
        //    }

           
        //    return View(model);
        //}
        public ActionResult AddAccountClass()
        {
            return View();
        }
        [Audit]
        [HttpPost, ActionName("AddAccountClass")]
        public ActionResult AddAccountClass(AccountClass model)
        {
            _financialService.AddAccountClass(new AccountClass() 
            { 
                Name = model.Name,
                NormalBalance = model.NormalBalance,
            });
            
            return RedirectToAction("ViewAccountClass");
        }

        public ActionResult ViewAccountClass(AccountClass model)
        {
            var viewAccountClass = _financialService.ViewAccountClass();

            return View(viewAccountClass);
           
        }
        public ActionResult AddAccount()
        {
            return View(new AddAccountViewModel());
        }
        [Audit]
        [HttpPost, ActionName("AddAccount")]
        [FormValueRequiredAttribute("Create")]
        public ActionResult AddAccount(AddAccountViewModel model)
        {
            Account account = new Account()
            {
                AccountCode = model.AccountCode,
                AccountName = model.AccountName,
                AccountClassId = model.AccountClass,
                Description = model.Description
            };

            _financialService.AddAccount(account);

            return RedirectToAction("Accounts");
        }

        public ActionResult EditAccount(int id)
        {
            var account = _financialService.GetAccounts().Where(a => a.Id == id).FirstOrDefault();

            Models.ViewModels.Financials.EditAccountViewModel model = new Models.ViewModels.Financials.EditAccountViewModel()
            {
                Id = account.Id,
                AccountCode = account.AccountCode,
                AccountName = account.AccountName,
                AccountClass = account.AccountClass.Name,
               
                Balance = account.Balance
               
            };


            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("EditAccount")]
        [FormValueRequiredAttribute("Save")]
        public ActionResult EditAccount(Models.ViewModels.Financials.EditAccountViewModel model)
        {
            var account = _financialService.GetAccounts().Where(a => a.Id == model.Id).FirstOrDefault();

            account.AccountCode = model.AccountCode;
            account.AccountName = model.AccountName;
           
            _financialService.UpdateAccount(account);

            return RedirectToAction("Accounts");
        }
        [Audit]
        public ActionResult ViewAccountsPDF()
        {
            var accounts = _financialService.GetAccounts();
            var model = new Models.ViewModels.Financials.Accounts();
            foreach (var account in accounts)
            {
                model.AccountsListLines.Add(new Models.ViewModels.Financials.AccountsListLine()
                {
                    Id = account.Id,
                    AccountCode = account.AccountCode,
                    AccountName = account.AccountName,
                    Balance = account.Balance
                });
            }

            var html = base.RenderPartialViewToString("Accounts", model);
            HttpContext.Response.Clear();
            HttpContext.Response.AddHeader("Content-Type", "application/pdf");
            HttpContext.Response.Filter = new PdfFilter(HttpContext.Response.Filter, html);

            return Content(html);
        }
        //[Audit]
        //public ActionResult JournalEntries()
        //{
        //    var journalEntries = _financialService.GetJournalEntries();
        //    var model = new JournalEntries();
        //    foreach(var je in journalEntries)
        //    {
        //            model.JournalEntriesListLines.Add(new JournalEntriesListLine()
        //            {
        //                Id = je.Id,
        //                AccountId = je.AccountId,
        //                AccountCode = je.Account.AccountCode,
        //                AccountName = je.Account.AccountName,
        //                DrCr = (int)je.DrCr == 1 ? "Dr" : "Cr",
        //                Amount = je.Amount
        //        });
                
        //    }
        //    return View(model);
        //}
        [Audit]
        public ActionResult JournalEntries(string sortOn, string orderBy,
        string pSortOn, string keyword, int? page)
        {
            int recordsPerPage = 3;
            if (!page.HasValue)
            {
                page = 1; // set initial page value
                if (string.IsNullOrWhiteSpace(orderBy) || orderBy.Equals("asc"))
                {
                    orderBy = "desc";
                }
                else
                {
                    orderBy = "asc";
                }
            }

            if (!string.IsNullOrWhiteSpace(sortOn) && !sortOn.Equals(pSortOn,
            StringComparison.CurrentCultureIgnoreCase))
            {
                orderBy = "asc";
            }

            ViewBag.OrderBy = orderBy;
            ViewBag.SortOn = sortOn;
            ViewBag.Keyword = keyword;
            var journals = _financialService.GetJournalEntries();
            switch (sortOn)
            {
                case "Category":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.Account.AccountName);
                       
                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.Account.AccountName);
                       
                    }
                    break;
                case "SubCategory":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.SubCategory.AccountSubCategoryName);

                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.SubCategory.AccountSubCategoryName);

                    }
                    break;
             
                case "Date":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.Date);

                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.Date);

                    }
                    break;

                case "DrCr":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.DrCr);

                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.DrCr);

                    }
                    break;

                case "AccountCode":
                    if (orderBy.Equals("desc"))
                    {
                        journals = journals.OrderByDescending(p => p.Account.AccountCode);
                        //list = list.OrderByDescending(p => p.Age);
                    }
                    else
                    {
                        journals = journals.OrderBy(p => p.Account.AccountCode);
                        // list = list.OrderBy(p => p.Age);
                    }
                    break;
                default:
                    //list = list.OrderBy(p => p.AutoId);
                    journals = journals.OrderBy(p => p.Id);
                    break;
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                journals = journals.Where(f=>f.Account.AccountClass.Name.StartsWith(keyword));
                
            }
            var finalJournals = journals.ToPagedList(page.Value, recordsPerPage);
            
            return View(finalJournals);
        }
       public ActionResult AddAccountSubCategory()
        {
          
            return View();
        }

       [Audit]
       [HttpPost]
      
       public ActionResult AddAccountSubCategory(AccountSubCategory model)
       {
           _financialService.AddAccountSubCategory(new AccountSubCategory()
           {
               
               AccountSubCategoryName = model.AccountSubCategoryName,
               AccountId = model.AccountId,
              // AccountClassId = model.AccountClassId,
               Description = model.Description
                
               //AccountName = Model.AccountName,
               //SubCategory = Model.SubCategory
           });
           

          // return RedirectToAction("Accounts");
           return RedirectToAction("SubCategories");
       }

       [Audit]
       public ActionResult Subcategories()
       {
           var subCategories = _financialService.ListAccountSubCategory();
           
           return View(subCategories);
       }
        public ActionResult AddJournalEntry()
        {
            var model = new AddJournalEntry();
            return View(model);
        }
       

        [Audit]
        [HttpPost, ActionName("AddJournalEntry")]
        [FormValueRequiredAttribute("AddJournalEntry")]

        public ActionResult AddJournalEntry(AddJournalEntry model)
        {
            JournalEntryLine journal = new JournalEntryLine()
            {
                AccountId = model.AccountId,
                ReferenceNo = model.ReferenceNo,
                DrCr = model.DrCr,
                Date = model.Date,
                Amount= model.Amount,
                Memo = model.Memo,
                SubCategoryId = model.SubCategoryId
            };
           
            _financialService.AddJournalEntry(journal);

            return RedirectToAction("JournalEntries");
        }
        //public ActionResult AddJournalEntryLine(AddJournalEntry model)
        //{
        //    //if(model.AccountId != -1 && model.Amount > 0)
        //    if(model.Amount > 0)
        //    {
        //        //var rowId = Guid.NewGuid().ToString();
        //        model.AddJournalEntryLines.Add(new AddJournalEntryLine()
        //        {
        //            AccountId = model.AccountId,
        //            AccountName = _financialService.GetAccounts().Where(a => a.Id == model.AccountId).FirstOrDefault().AccountName,
        //            DrCr = model.DrCr,
        //            Amount = model.Amount,
        //            Memo = model.MemoLine
        //        });
        //    }
        //    return View(model);
        //}

        public ActionResult EditJournalEntry(int id)
        {
            // for now, use the same view model as add journal entry. nothing different
            var je = _financialService.GetJournalEntry(id);

            var model = new AddJournalEntry();
            model.Date = je.Date;
            model.Memo = je.Memo;
            model.ReferenceNo = je.ReferenceNo;
            model.Id = je.Id;
            //model.JournalEntryId = je.Id;
           // model.Posted = je.Posted.HasValue ? je.Posted.Value : false;

          
            return View(model);
        }
        [Audit]
        [HttpPost, ActionName("EditJournalEntry")]
        [FormValueRequiredAttribute("UpdateJournalEntry")]
        public ActionResult EditJournalEntry(AddJournalEntry model)
        {
            if (model.AddJournalEntryLines.Count < 2)
                return View(model);

            var journalEntry = _financialService.GetJournalEntry(model.Id);

            journalEntry.Date = model.Date;
            journalEntry.Memo = model.Memo;
            journalEntry.ReferenceNo = model.ReferenceNo;

            _financialService.UpdateJournalEntry(journalEntry);

            return RedirectToAction("JournalEntries");
        }


        [HttpPost, ActionName("AddJournalEntry")]
        [FormValueRequiredAttribute("DeleteJournalEntryLine")]
        public ActionResult DeleteJournalEntryLine(AddJournalEntry model)
        {
            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            
            return View(model);
        }

        [HttpPost, ActionName("EditJournalEntry")]
        [FormValueRequiredAttribute("DeleteJournalEntryLine")]
        public ActionResult UpdateJournalEntryLine(AddJournalEntry model)
        {
            var request = HttpContext.Request;
            var deletedItem = request.Form["DeletedLineItem"];
            //model.AddJournalEntryLines.Remove(model.AddJournalEntryLines.Where(i => i.RowId.ToString() == deletedItem.ToString()).FirstOrDefault());
            return View(model);
        }

        [HttpPost, ActionName("AddJournalEntry")]
        [FormValueRequiredAttribute("SaveJournalEntry")]
        public ActionResult SaveJournalEntry(AddJournalEntry model)
        {
            if(model.AddJournalEntryLines.Count < 2)
                return View(model);
            var journalEntry = new JournalEntryLine()
            {
                Date = model.Date,
                Memo = model.Memo,
                ReferenceNo = model.ReferenceNo,
                
            };
       
            _financialService.AddJournalEntry(journalEntry);
            return RedirectToAction("JournalEntries");
        }

        public ActionResult TrialBalance()
        {
            var model = _financialService.TrialBalance();
            return View(model);
        }
        [Audit]
        public ActionResult BalanceSheet()
        {
            var model = _financialService.BalanceSheet().ToList();
            var dt = Helpers.CollectionHelper.ConvertTo<BalanceSheet>(model);
            var incomestatement = _financialService.IncomeStatement();
            var netincome = incomestatement.Where(a => a.IsExpense == false).Sum(a => a.Amount) - incomestatement.Where(a => a.IsExpense == true).Sum(a => a.Amount);


            return View(model);
        }
        [Audit]
        public ActionResult IncomeStatement()
        {
            var model = _financialService.IncomeStatement();
            return View(model);
        }
        [Audit]
        public ActionResult PLStatement()
        {
            var model = _financialService.ProfitAndLoss();
            return View(model);
        }
        //public ActionResult Banks()
        //{
        //    var model = new Banks();
        //    var banks = _financialService.GetCashAndBanks();
        //    foreach (var bank in banks)
        //    {
        //        model.BankList.Add(new BankListLine()
        //        {
        //            Name = bank.Name,
        //            BankName = bank.BankBranch,
        //            AccountId = bank.AccountId,
        //            Number = bank.Number,
        //            Type = bank.Type,
        //            Address = bank.Address,
        //            IsActive = bank.IsActive,
        //            IsDefault = bank.IsDefault
        //        });
        //    }
        //    return View(model);
        //}

        //public ActionResult TaxGroups()
        //{
        //    var taxGroups = _financialService.GetTaxGroups();
        //    var model = new List<TaxGroupModel>();

        //    foreach(var group in taxGroups)
        //    {
        //        var groupTaxes = new List<TaxGroupTaxModel>();

        //        foreach (var groupTax in group.TaxGroupTax)
        //        {
        //            groupTaxes.Add(new TaxGroupTaxModel()
        //            {
        //                Id = groupTax.Id,
        //                TaxId = groupTax.TaxId,
        //                TaxGroupId = groupTax.TaxGroupId
        //            });
        //        }

        //        model.Add(new TaxGroupModel()
        //        {
        //            Id = group.Id,
        //            Description = group.Description,
        //            TaxAppliedToShipping = group.TaxAppliedToShipping,
        //            IsActive = group.IsActive,      
        //            TaxGroupTaxModel = groupTaxes
        //        });
        //    }

        //    return View(model);
        //}
    }
}
