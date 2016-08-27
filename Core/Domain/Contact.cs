using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain
{
//    [Table("Contact")]
    public partial class Contact : BaseEntity
    {
        public Contact()
        {
        }
        /// <summary>
        /// Check ContactyType to determine whether CompanyNo is Customer No or Vendor No
        /// </summary>
        public ContactTypes ContactType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int PartyId { get; set; }
        public virtual Party Party { get; set; }
    }
}
