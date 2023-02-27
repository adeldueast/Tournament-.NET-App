using LANPartyAPI_Utils;
using Microsoft.AspNetCore.Identity;
using LANPartyAPI_Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LANPartyAPI_Core.Enums;

namespace LANPartyAPI_DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }

        public DbSet<Team> Teams { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Dont want to seed during testing
            if (Database.IsInMemory())
                return;

            //SEED EVENTS
            modelBuilder.Entity<Event>()
               .HasData(
               new Event
               {
                   Id = 1,
                   Name = "LAN01",
                   StartDate = new DateTime(2023, 02, 20),
                   EndDate = new DateTime(2023, 02, 25),
                   MaxPlayerNumber = 500,
                   Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer turpis lorem, rhoncus at lacus ut, consectetur vehicula tellus. Integer at imperdiet mi, in feugiat nisl. Praesent a sem nec enim blandit cursus in nec velit. Vivamus accumsan tincidunt metus, ac commodo nibh ultrices at. Pellentesque id metus quis libero ultricies iaculis. Vivamus posuere gravida feugiat. Phasellus et sagittis velit. Curabitur eleifend sed ipsum eget congue. Cras augue libero, placerat pharetra sollicitudin quis, faucibus ut nibh.",
               });

            //SEED ROLES AND ADMIN USER
            #region
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = Utils.Roles.Admin, NormalizedName = Utils.Roles.Admin.ToUpper() });
            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "32ffcfa8-7337-4d05-a17e-99d6280a12d5", Name = Utils.Roles.Joueur, NormalizedName = Utils.Roles.Joueur.ToUpper() });


            // //a hasher to hash the password before seeding the user to the db
            PasswordHasher<ApplicationUser> hasher = new PasswordHasher<ApplicationUser>();

            //Seeding the User to AspNetUsers table
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "3HEiEUpHH1eTYNzsYOOkjmK1o7Z2", // primary key (firebase uid)
                    Email = "admin@gmail.com",
                    NormalizedEmail = "admin@gmail.com".ToUpper(),
                    FirstName = "Adel",
                    LastName = "Kouaou",
                    UserName = "admin-adel",
                    NormalizedUserName = "3HEiEUpHH1eTYNzsYOOkjmK1o7Z2".ToUpper(),
                    PasswordHash = hasher.HashPassword(null, "123456")
                }
            );

            // //Seeding the User to AspNetUsers table
            // modelBuilder.Entity<ApplicationUser>().HasData(
            //     new ApplicationUser
            //     {
            //         Id = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df", // primary key
            //         Email = "player1@hotmail.com",
            //         NormalizedEmail = "player1@hotmail.com".ToUpper(),
            //         FirstName = "Adel",
            //         LastName = "Kouaou",
            //         UserName = "s1mple",
            //         NormalizedUserName = "replace-by-firebaseUid1".ToUpper(),
            //         PasswordHash = hasher.HashPassword(null, "123456")
            //     }
            // );

            // modelBuilder.Entity("ApplicationUserEvent").HasData(
            //   new
            //   {
            //       PlayersId = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df",
            //       EventsId = 1,
            //   },
            //   new
            //   {
            //       PlayersId = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df",
            //       EventsId = 2,
            //   },
            //    new
            //    {
            //        PlayersId = "039e1e4b-fbd0-4638-b3ab-3567e6c944b6",
            //        EventsId = 1,
            //    },
            //   new
            //   {
            //       PlayersId = "039e1e4b-fbd0-4638-b3ab-3567e6c944b6",
            //       EventsId = 2,
            //   });

            // modelBuilder.Entity("ApplicationUserTeam").HasData(
            //     new
            //     {
            //         PlayersId = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df",
            //         TeamsId = 1,
            //     },
            //     new
            //     {
            //         PlayersId = "c036ad57-128c-4d61-94ef-ceb8a7c8a8df",
            //         TeamsId = 2,
            //     },
            //      new
            //      {
            //          PlayersId = "039e1e4b-fbd0-4638-b3ab-3567e6c944b6",
            //          TeamsId = 1,
            //      },
            //     new
            //     {
            //         PlayersId = "039e1e4b-fbd0-4638-b3ab-3567e6c944b6",
            //         TeamsId = 2,
            //     });

            //Seeding the relation between our user and role to AspNetUserRoles table
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                    UserId = "3HEiEUpHH1eTYNzsYOOkjmK1o7Z2"
                }
            );
            #endregion

        }
    }
}
