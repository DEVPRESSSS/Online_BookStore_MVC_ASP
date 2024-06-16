using Microsoft.EntityFrameworkCore;
using Online_BookStore.Models;

namespace Online_BookStore.Data
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){
        
        }

        public DbSet<Category> Categories { get; set; }



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


                
        }


    }
}
