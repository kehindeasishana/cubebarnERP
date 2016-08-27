using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Domain.Financials
{
    public partial class JournalEntryLine : BaseEntity
    {
       
        //public int JournalEntryHeaderId { get; set; }
        public int AccountId { get; set; }
        public int? SubCategoryId { get; set; }
        public DrOrCrSide DrCr { get; set; }
        public decimal Amount { get; set; }
        public virtual AccountSubCategory SubCategory { get; set; }
        public virtual Account Account { get; set; }
        public DateTime Date { get; set; }
        public string Memo { get; set; }
        public string ReferenceNo { get; set; }
        public bool? Posted { get; set; }

    //    [NotMapped]
    //    public decimal Balance { get { return GetBalance(); } }
    //    [NotMapped]
    //    public decimal DebitBalance { get { return GetDebitCreditBalance(DrOrCrSide.Dr); } }
    //    [NotMapped]
    //    public decimal CreditBalance { get { return GetDebitCreditBalance(DrOrCrSide.Cr); } }
    //    [NotMapped]
    //    public string BalanceSide { get; set; }

    //    private decimal GetDebitCreditBalance(DrOrCrSide side)
    //    {
    //        decimal balance = 0;

    //        if (side == DrOrCrSide.Dr)
    //        {
    //            var dr = from d in GeneralLedgerLines
    //                     where d.DrCr == DrOrCrSide.Dr
    //                     select d;

    //            balance = dr.Sum(d => d.Amount);
    //        }
    //        else
    //        {
    //            var cr = from d in GeneralLedgerLines
    //                     where d.DrCr == DrOrCrSide.Cr
    //                     select d;

    //            balance = cr.Sum(d => d.Amount);
    //        }

    //        return balance;
    //    }

    //    public decimal GetBalance()
    //    {
    //        decimal balance = 0;

    //        var dr = from d in GeneralLedgerLines
    //                 where d.DrCr == DrOrCrSide.Dr
    //                 select d;

    //        var cr = from c in GeneralLedgerLines
    //                 where c.DrCr == DrOrCrSide.Cr
    //                 select c;

    //        decimal drAmount = dr.Sum(d => d.Amount);
    //        decimal crAmount = cr.Sum(c => c.Amount);

    //        if (AccountClass.NormalBalance == "Dr")
    //        {
    //            balance = drAmount - crAmount;
    //        }
    //        else
    //        {
    //            balance = crAmount - drAmount;
    //        }

    //        return balance;
    //    }

    //    //public bool CanPost()
    //    //{
    //    //    if (ChildAccounts != null && ChildAccounts.Count > 0)
    //    //        return false;
    //    //    return true;
    //    //}

    //    /// <summary>
    //    /// Used to indicate the increase or decrease on account. When there is a change in an account, that change is indicated by either debiting or crediting that account.
    //    /// </summary>
    //    /// <param name="amount">The amount to enter on account.</param>
    //    /// <returns></returns>
    //    public DrOrCrSide DebitOrCredit(decimal amount)
    //    {
    //        var side = DrOrCrSide.Dr;

    //        if (this.AccountClassId == (int)AccountClasses.Assets || this.AccountClassId == (int)AccountClasses.Expense)
    //        {
    //            if (amount > 0)
    //                side = DrOrCrSide.Dr;
    //            else
    //                side = DrOrCrSide.Cr;
    //        }

    //        if (this.AccountClassId == (int)AccountClasses.Liabilities || this.AccountClassId == (int)AccountClasses.Equity || this.AccountClassId == (int)AccountClasses.Revenue)
    //        {
    //            if (amount < 0)
    //                side = DrOrCrSide.Dr;
    //            else
    //                side = DrOrCrSide.Cr;
    //        }

    //        if (this.IsContraAccount)
    //        {
    //            if (side == DrOrCrSide.Dr)
    //                return DrOrCrSide.Cr;
    //            if (side == DrOrCrSide.Cr)
    //                return DrOrCrSide.Dr;
    //        }

    //        return side;
    //    }
    }
}
