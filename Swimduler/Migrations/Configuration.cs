namespace Swimduler.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Swimduler.Models;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            // Do debugowania metody seed
            // if (System.Diagnostics.Debugger.IsAttached == false)
            // System.Diagnostics.Debugger.Launch();

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            SeedRoles(context);
            SeedUsers(context);
            SeedTrainers(context);
            SeedClients(context);
            SeedGroups(context);
            SeedClient_Groups(context);
            SeedLessons(context);

        }

        private void SeedRoles(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }
        }

        private void SeedUsers(ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            if (!context.Users.Any(u => u.UserName == "Admin"))
            {
                var user = new ApplicationUser { UserName = "Admin" };
                var adminresult = manager.Create(user, "666777");
                if (adminresult.Succeeded)
                {
                    manager.AddToRole(user.Id, "Admin");
                }
            }
        }

        private void SeedTrainers(ApplicationDbContext context)
        {
            var trainer = new Trainer
            {
                Id = 0,
                FirstName = "TrainerName",
                SecondName = "TrainerSurname",
                Street = "TrainerStreet",
                ApartmentNumber = "666",
                PostCode = "66-666 Hell",
                PhoneNumber = "666666666"
            };
            context.Trainers.AddOrUpdate(trainer);
            context.SaveChanges();
        }

        private void SeedClients(ApplicationDbContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                var client = new Client
                {
                    Id = i,
                    FirstName = "ClientName" + i.ToString(),
                    SecondName = "ClientSurname" + i.ToString(),
                    Street = "ClientStreet" + i.ToString(),
                    ApartmentNumber = "77" + i.ToString(),
                    PostCode = "77-77" + i.ToString() + " Hell",
                    PhoneNumber = "77777777" + i.ToString(),
                    Gender = (i % 2 == 0 ? Client.GenderType.Mê¿czyzna : Client.GenderType.Kobieta),
                    BirthDate = DateTime.Now.AddYears(-30 + i)
                };
                context.Clients.AddOrUpdate(client);
            }
            context.SaveChanges();
        }

        private void SeedGroups(ApplicationDbContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                var group = new Group
                {
                    Id = i,
                    Name = "GroupName" + i.ToString()
                };
                context.Groups.AddOrUpdate(group);
            }
            context.SaveChanges();
        }

        private void SeedClient_Groups(ApplicationDbContext context)
        {
            var clientId = context.Clients.FirstOrDefault().Id;
            var groupId = context.Groups.FirstOrDefault().Id;

            for (int i = 0; i < 10; i++)
            {
                var clinet_group = new Client_Group
                {
                    Id = i,
                    ClientId = clientId,
                    GroupId = groupId
                };
                context.Client_Groups.AddOrUpdate(clinet_group);
            }
            context.SaveChanges();
        }

        private void SeedLessons(ApplicationDbContext context)
        {
            var groupId = context.Groups.FirstOrDefault().Id;

            for (int i = 0; i < 10; i++)
            {
                var lesson = new Lesson
                {
                    Beginning = DateTime.Now.AddDays(i),
                    Duration = TimeSpan.FromHours(1.5),
                    Cycle = (i % 2 == 0 ? Lesson.LessonCycle.Tygodniowy : Lesson.LessonCycle.Dwutygodniowy),
                    Status = Lesson.LessonStatus.Zaplanowana,
                    GroupId = groupId
                };
                context.Lessons.AddOrUpdate(lesson);
            }
            context.SaveChanges();
        }
    }
}
