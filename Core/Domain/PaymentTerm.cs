using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
//    [Table("PaymentTerm")]
    public partial class PaymentTerm : BaseEntity
    {
        public string Description { get; set; }
        public PaymentTypes PaymentType { get; set; }
        public int? DueAfterDays { get; set; }
        public bool IsActive { get; set; }
    }
}
