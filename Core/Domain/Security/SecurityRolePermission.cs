using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Security
{
//    [Table("SecurityRolePermission", Schema = "dbo")]
    public class SecurityRolePermission : BaseEntity
    {
        public int SecurityRoleId { get; set; }
        public int SecurityPermissionId { get; set; }

        public virtual SecurityRole SecurityRole { get; set; }
        public virtual SecurityPermission SecurityPermission { get; set; }
    }
}
