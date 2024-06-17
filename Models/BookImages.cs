using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_BookStore.Models
{
	public class BookImages
	{
		[Key]
		public int Image_ID { get; set; }

		[Required]

		public string? Image_Url { get; set;}


		public int ? Book_ID { get; set; }
		[ForeignKey("Book_ID")]

		public Books? Books { get; set; }
	}
}
