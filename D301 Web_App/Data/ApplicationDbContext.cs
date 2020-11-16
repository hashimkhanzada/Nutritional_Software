using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace D301_WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Intake> Intakes { get; set; }
        public DbSet<FoodVariation> FoodVariations { get; set; }
        public DbSet<RDI> Rdis { get; set; }
        public DbSet<CustomFood> CustomFoods { get; set; }
        public DbSet<CustomFoodVariation> CustomFoodVariations { get; set; }
        public DbSet<FoodBookmark> FoodBookmarks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Food>().ToTable("Food");
            modelBuilder.Entity<Intake>().ToTable("Intake");
            modelBuilder.Entity<FoodVariation>().ToTable("FoodVariation");
            modelBuilder.Entity<RDI>().ToTable("Rdi");
            modelBuilder.Entity<CustomFood>().ToTable("CustomFood");
            modelBuilder.Entity<FoodBookmark>().ToTable("FoodBookmark");
            modelBuilder.Entity<CustomFoodVariation>().ToTable("CustomFoodVariation");
        }

        public void CreateDatabase()
        {
            Database.EnsureCreated();


            if (!Users.Any())
            {
                ApplicationUser user1 = new ApplicationUser
                {
                    Id = "1",
                    UserName = "test1@gmail.com",
                    NormalizedUserName = "TEST1@GMAIL.COM",
                    Email = "test1@gmail.com",
                    NormalizedEmail = "TEST1@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEK+glfc5ssMG/BhbHABtzdSXHuIIty1lMqU1ZIPXu5BHVxYdbypJ+Cf0jNV76Z5ktQ==",
                    SecurityStamp = "HCSPCGALY3WQ3YAFTPXSVQ3RDRJODSTZ",
                    ConcurrencyStamp = "f8f6dfe9-3ede-4550-8f8f-eb35673e548a",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    Macros = "1,2,3",
                    FirstName = "John",
                    LastName = "Doe",
                    TrainerId = null,
                    Weight = 100,
                    Height = 1800,
                    EnegryGoal = 10000,
                    ProteinGoal = 50,
                    FatGoal = 100,
                    CarbohydratesGoal = 200,
                    MedicalConditions = "None"
                };
                ApplicationUser user2 = new ApplicationUser
                {
                    Id = "2",
                    UserName = "test2@gmail.com",
                    NormalizedUserName = "TEST2@GMAIL.COM",
                    Email = "test2@gmail.com",
                    NormalizedEmail = "TEST2@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEB1pudzxUmYNf1BllnOB8AzqAE+tJgpnP1J/54LMqUGCvbU6xgt9bJzw5/7mvrBl1A==",
                    SecurityStamp = "MQ5OO5WTWFEF5QA6N376HC5BT32E55LC",
                    ConcurrencyStamp = "570a9d84-d0d8-44f9-883e-423c02b2b7eb",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    Macros = "1,2,3",
                    FirstName = "Ethan",
                    LastName = "Kiehn",
                    TrainerId = "1",
                    Weight = 100,
                    Height = 1800,
                    EnegryGoal = 10000,
                    ProteinGoal = 500,
                    FatGoal = 10,
                    CarbohydratesGoal = 100,
                    MedicalConditions = "None"
                };
                ApplicationUser user3 = new ApplicationUser
                {
                    Id = "3",
                    UserName = "test3@gmail.com",
                    NormalizedUserName = "TEST3@GMAIL.COM",
                    Email = "test3@gmail.com",
                    NormalizedEmail = "TEST3@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEB1pudzxUmYNf1BllnOB8AzqAE+tJgpnP1J/54LMqUGCvbU6xgt9bJzw5/7mvrBl1A==",
                    SecurityStamp = "MQ5OO5WTWFEF5QA6N376HC5BT32E55LC",
                    ConcurrencyStamp = "570a9d84-d0d8-44f9-883e-423c02b2b7eb",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    Macros = "1,2,3",
                    FirstName = "Reva",
                    LastName = "Collins",
                    TrainerId = "1",
                    Weight = 100,
                    Height = 1800,
                    EnegryGoal = 10000,
                    ProteinGoal = 250,
                    FatGoal = 200,
                    CarbohydratesGoal = 300,
                    MedicalConditions = "None"
                };
                ApplicationUser user4 = new ApplicationUser
                {
                    Id = "4",
                    UserName = "test4@gmail.com",
                    NormalizedUserName = "TEST4@GMAIL.COM",
                    Email = "test4@gmail.com",
                    NormalizedEmail = "TEST4@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEB1pudzxUmYNf1BllnOB8AzqAE+tJgpnP1J/54LMqUGCvbU6xgt9bJzw5/7mvrBl1A==",
                    SecurityStamp = "MQ5OO5WTWFEF5QA6N376HC5BT32E55LC",
                    ConcurrencyStamp = "570a9d84-d0d8-44f9-883e-423c02b2b7eb",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    Macros = "1,2,3",
                    FirstName = "Jenn",
                    LastName = "Jones",
                    TrainerId = null,
                    Weight = 100,
                    Height = 1800,
                    EnegryGoal = 10000,
                    ProteinGoal = 250,
                    FatGoal = 200,
                    CarbohydratesGoal = 300,
                    MedicalConditions = "None"
                };
                ApplicationUser user5 = new ApplicationUser
                {
                    Id = "5",
                    UserName = "test5@gmail.com",
                    NormalizedUserName = "TEST5@GMAIL.COM",
                    Email = "test5@gmail.com",
                    NormalizedEmail = "TEST5@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEB1pudzxUmYNf1BllnOB8AzqAE+tJgpnP1J/54LMqUGCvbU6xgt9bJzw5/7mvrBl1A==",
                    SecurityStamp = "MQ5OO5WTWFEF5QA6N376HC5BT32E55LC",
                    ConcurrencyStamp = "570a9d84-d0d8-44f9-883e-423c02b2b7eb",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnd = null,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    Macros = "1,2,3",
                    FirstName = "Glenn",
                    LastName = "Brown",
                    TrainerId = null,
                    Weight = 100,
                    Height = 1800,
                    EnegryGoal = 10000,
                    ProteinGoal = 250,
                    FatGoal = 200,
                    CarbohydratesGoal = 300,
                    MedicalConditions = "None"
                };

                Users.AddRange(new ApplicationUser[]
                    {user1,user2,user3,user4,user5}
                );
                SaveChanges();

                if (!Foods.Any())
                {
                    foreach (string file in Directory.GetFiles("Migrations/SeedData"))
                    {
                        Database.ExecuteSqlRaw(
                            File.ReadAllText(file)
                        );
                    }
                    foreach (string file in Directory.GetFiles("Migrations/TempFix"))
                    {
                        Database.ExecuteSqlRaw(
                            File.ReadAllText(file)
                        );
                    }
                }

                Intakes.AddRange(new Intake[]
                {
                    new Intake{
                        Amount=204,
                        Measure="204",
                        Quantity = 1,
                        Date = DateTime.Now,
                        User = user1,
                        Food = Foods.Find("H8"),
                        VariationId = "678592-7"
                    },
                    new Intake{
                        Amount=69,
                        Measure="69",
                        Quantity=1,
                        Date=DateTime.Now,
                        User=user1,
                        Food=this.Foods.Find("H1042"),
                        VariationId="bb86b8-7"
                    },
                    new Intake{
                        Amount=450,
                        Measure="450",
                        Quantity=1,
                        Date=DateTime.Now,
                        User=user1,
                        Food=this.Foods.Find("C46"),
                        VariationId=null
                    },
                    new Intake{
                        Amount=442.200012207031F,
                        Measure="442.2",
                        Quantity=1,
                        Date=DateTime.Now,
                        User=user1,
                        Food=this.Foods.Find("B1016"),
                        VariationId="5946f0-9"
                    },
                    new Intake{
                        Amount=300,
                        Measure="300",
                        Quantity=1,
                        Date=DateTime.Now,
                        User=user1,
                        Food=this.Foods.Find("F111"),
                        VariationId="0"
                    },
                    new Intake{
                        Amount=19,
                        Measure="19",
                        Quantity=1,
                        Date=DateTime.Now,
                        User=user2,
                        Food=this.Foods.Find("A1076"),
                        VariationId="15b52c-0"
                    },
                });
                SaveChanges();

            }                     
        }
    }
}
