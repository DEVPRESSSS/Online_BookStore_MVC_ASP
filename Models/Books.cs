using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_BookStore.Models
{
	public class Books
	{
		[Key]
		public int Book_Id { get; set; }

		[Required]

		
		public string? Title { get; set; }

	    [Required]

		public string? Description { get; set; }


		[Required]

		public string? ISBN { get; set; }


		[Required]

		public float? Price { get; set; }


		[Required]

		public DateOnly PublishDate { get; set; }

		[Required]

		public string? Publisher { get; set; }

		[Required]

		public int Stock {  get; set; }
		
		public int Category_ID { get; set; }
		[ForeignKey("Category_ID")]

		[ValidateNever]
		public Category? category { get; set; }


		[ValidateNever]

		List<BookImages>? Book_Images { get; set; }



	}
}
