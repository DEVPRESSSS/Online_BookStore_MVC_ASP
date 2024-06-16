using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Online_BookStore.Models
{
    public class Category
    {

        [Key] 
        public int Category_ID { get; set; }
        [Required]
        [DisplayName("Category Name")]
        public string? Category_Name { get; set; }
    }
}
