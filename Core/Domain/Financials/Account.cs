using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Financials
{
    public partial class Account : BaseEntity
    {
        public Account()
        {
           
            JournalEntryLines = new HashSet<JournalEntryLine>();
          
        }
        public int AccountClassId { get; set; }
      
        [Required]
        [StringLength(50)]
        public string AccountCode { get; set; }
        [Required]
        [StringLength(200)]
        public string AccountName { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public virtual ICollection< AccountSubCategory> SubCategory { get; set; }
        public virtual AccountClass AccountClass { get; set; }
        public virtual ICollection<JournalEntryLine> JournalEntryLines { get; set; }

        [NotMapped]
        public decimal Balance { get { return GetBalance(); } }
        [NotMapped]
        public decimal DebitBalance { get { return GetDebitCreditBalance(DrOrCrSide.Dr); } }
        [NotMapped]
        public decimal CreditBalance { get { return GetDebitCreditBalance(DrOrCrSide.Cr); } }
        [NotMapped]
        public string BalanceSide { get; set; }

        private decimal GetDebitCreditBalance(DrOrCrSide side)
        {
            decimal balance = 0;

            if (side == DrOrCrSide.Dr)
            {
                var dr = from d in JournalEntryLines
                         where d.DrCr == DrOrCrSide.Dr
                         select d;

                balance = dr.Sum(d => d.Amount);
            }
            else
            {
                var cr = from d in JournalEntryLines
                         where d.DrCr == DrOrCrSide.Cr
                         select d;

                balance = cr.Sum(d => d.Amount);
            }

            return balance;
        }

        public decimal GetBalance()
        {
            decimal balance = 0;

            var dr = from d in JournalEntryLines
                     where d.DrCr == DrOrCrSide.Dr
                     select d;

            var cr = from c in JournalEntryLines
                     where c.DrCr == DrOrCrSide.Cr
                     select c;

            decimal drAmount = dr.Sum(d => d.Amount);
            decimal crAmount = cr.Sum(c => c.Amount);

            if (AccountClass.NormalBalance == "Dr")
            {
                balance = drAmount - crAmount;
            }
            else
            {
                balance = crAmount - drAmount;
            }

            return balance;
        }


        /// <summary>
        /// Used to indicate the increase or decrease on account. When there is a change in an account, that change is indicated by either debiting or crediting that account.
        /// </summary>
        /// <param name="amount">The amount to enter on account.</param>
        /// <returns></returns>
        public DrOrCrSide DebitOrCredit(decimal amount)
        {
            var side = DrOrCrSide.Dr;

            if (this.AccountClassId == (int)AccountClasses.Assets || this.AccountClassId == (int)AccountClasses.Expense)
            {
                if (amount > 0)
                    side = DrOrCrSide.Dr;
                else
                    side = DrOrCrSide.Cr;
            }

            if (this.AccountClassId == (int)AccountClasses.Liabilities || this.AccountClassId == (int)AccountClasses.Equity || this.AccountClassId == (int)AccountClasses.Revenue)
            {
                if (amount < 0)
                    side = DrOrCrSide.Dr;
                else
                    side = DrOrCrSide.Cr;
            }

            return side;
        }
    }
}
