using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Financials
{
    public partial class Bank : BaseEntity
    {
        public BankTypes Type { get; set; }
        public string Name { get; set; }
        public int? AccountId { get; set; }
        public string BankBranch{ get; set; }
        public string Number { get; set; }
        public string Address { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public virtual Account Account { get; set; }
    }
}
