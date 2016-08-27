using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Data
{
    public static class InitialData
    {
        public static void InitializeSeedData( )
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {

                UserName = "asishana4u@yahoo.com",
                Email = "asishana4u@yahoo.com",
                EmailConfirmed = true,
                FirstName = "Kehinde",
                LastName = "Asishana"
            };

            manager.Create(user, "Password");

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "General Manager" });
                roleManager.Create(new IdentityRole { Name = "Account Manager" });
                roleManager.Create(new IdentityRole { Name = "HR Manager" });
                roleManager.Create(new IdentityRole { Name = "Cashier" });
                roleManager.Create(new IdentityRole { Name = "Employee" });
                roleManager.Create(new IdentityRole { Name = "Inventory Manager" });
                roleManager.Create(new IdentityRole { Name = "HR User" });
                roleManager.Create(new IdentityRole { Name = "Item Manager" });
                roleManager.Create(new IdentityRole { Name = "Leave Approver" });
                roleManager.Create(new IdentityRole { Name = "Expense Approver" });
                roleManager.Create(new IdentityRole { Name = "Manufacturing Manager" });
                roleManager.Create(new IdentityRole { Name = "Stock User" });
                roleManager.Create(new IdentityRole { Name = "Sales Manager" });
                roleManager.Create(new IdentityRole { Name = "Stock Manager" });
                roleManager.Create(new IdentityRole { Name = "Quality Assurance Manager" });
                roleManager.Create(new IdentityRole { Name = "Purchase Manager" });
                roleManager.Create(new IdentityRole { Name = "Projects User" });
                roleManager.Create(new IdentityRole { Name = "Auditor"});
                roleManager.Create(new IdentityRole { Name = "Guest" });
                roleManager.Create(new IdentityRole { Name = "Newsletter Manager" });
                roleManager.Create(new IdentityRole { Name = "Blogger" });
                roleManager.Create(new IdentityRole { Name = "Report Manager" });
                roleManager.Create(new IdentityRole { Name = "Maintenance User" });
                roleManager.Create(new IdentityRole { Name = "Manufacturing User" });
            }

            var adminUser = manager.FindByName("asishana4u@yahoo.com");

            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "General Manager" });
        }
    }
}