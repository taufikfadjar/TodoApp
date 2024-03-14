using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Model.Entities;


namespace TodoApp.Model
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole
                    {
                        Name = "User",
                        NormalizedName = "USER",
                        Id = "8c92032e-75bd-42cb-b2db-9a43e3459390"
                    }
                );


            var hash = new PasswordHasher<ApplicationUser>();

            builder.Entity<ApplicationUser>().HasData(
                    new ApplicationUser
                    {
                        Id = "f53f4178-8849-47f8-8b02-f871d36e8e05",
                        Email = "taufikfadjar@live.com",
                        NormalizedEmail = "TAUFIKFADJAR@LIVE.COM",
                        PasswordHash = hash.HashPassword(null, "taufikfadjar@live.com"),
                        UserName = "taufikfadjar",
                        NormalizedUserName = "TAUFIKFADJAR",
                        FullName = "Taufik Fadjar"
                    }
                );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId  = "f53f4178-8849-47f8-8b02-f871d36e8e05",
                    RoleId = "8c92032e-75bd-42cb-b2db-9a43e3459390"

                });

        }

        public DbSet<TodoActivity> TodoActivities { get; set; }

    }
}
