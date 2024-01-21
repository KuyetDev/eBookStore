using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<BookAuthor> BookAuthors { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookAuthor>().HasKey(sc => new { sc.BookId, sc.AuthorId });

            modelBuilder.Entity<BookAuthor>()
                        .HasOne(sc => sc.Book)
                        .WithMany(s => s.bookAuthors)
                        .HasForeignKey(sc => sc.BookId);

            modelBuilder.Entity<BookAuthor>()
                        .HasOne<Author>(sc => sc.Author)
                        .WithMany(s => s.BookAuthorList)
                        .HasForeignKey(sc => sc.AuthorId);
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleDesc = "Admin" },
                new Role { Id = 2, RoleDesc = "User" }
            ); 
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { PubId = 1, PublisherName = "Admin1", City="HN", State="VN", Country="VN" },
                new Publisher { PubId = 2, PublisherName = "User1", City="HP", State="VN", Country="VN" }
            );
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, RoleId = 1 , Email="admin1@gmail.com", Password="admin123456", Source="123", FirstName="Nguyen", LastName="Quyet", MiddleName="Huu", HireDate=DateTime.Now, PubId=1},
                new User { Id = 2, RoleId = 2 , Email= "user1@gmail.com", Password="user123456", Source="ok", FirstName="Nguyen", LastName="Quyet2", MiddleName="Huu2", HireDate=DateTime.Now, PubId=2}
            );
        }
    }
}
