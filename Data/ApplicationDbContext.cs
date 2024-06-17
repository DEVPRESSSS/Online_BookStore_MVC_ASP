using Microsoft.EntityFrameworkCore;
using Online_BookStore.Models;

namespace Online_BookStore.Data
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){
        
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Books> Books_List { get; set; }
        public DbSet<BookImages> Books_List_Images { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Category_ID= 1 , Category_Name="Fiction"},
                new Category { Category_ID = 2, Category_Name = "Romance" },
                new Category { Category_ID = 3, Category_Name = "Mystery/Thriller" },
                new Category { Category_ID = 4, Category_Name = "Classics" },
                new Category { Category_ID = 5, Category_Name = "Fiction" },
                new Category { Category_ID = 6, Category_Name = "Romance" },
                new Category { Category_ID = 7, Category_Name = "Mystery/Thriller" },
                new Category { Category_ID = 8, Category_Name = "Classics" }





                );

            modelBuilder.Entity<Books>().HasData(
              new Books { Book_Id = 1, Title = "DS101", Description="Programming subject", ISBN="KS454574JDHFBS", Price= 50,
              
              PublishDate= DateOnly.Parse("2023-12-10"), Publisher ="Sir Ervin Santos",Stock= 15, Category_ID= 2}
          





              );



        }





    }
}
