using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        //[Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        //[Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        public virtual ICollection<IdentityUserRole> Roles { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ApplicationContext", throwIfV1Schema: false)
        {
            System.Data.Entity.Database.SetInitializer<ApplicationDbContext>(null); // uncomment this line to disable code first
        }

        public static ApplicationDbContext Create()
        {
            var context = new ApplicationDbContext();
            return context;
        }
    }
}
