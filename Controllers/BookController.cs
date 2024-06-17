using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Online_BookStore.Data;
using Online_BookStore.Models;

namespace Online_BookStore.Controllers
{
    public class BookController : Controller
    {
		private readonly ApplicationDbContext _db;

		public BookController(ApplicationDbContext db)
        {
            _db= db;
        }
        public IActionResult Index()
        {
            return View();
        }


        //Display the view as well as the List of items
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories.Select(c => new SelectListItem
            {

                Value= c.Category_ID.ToString(),
                Text=c.Category_Name


            }).ToList();

            return View();

        }

		//This method function is for inserting data

		[HttpPost]
		public IActionResult Create(Books obj)
		{

            if (obj.Stock <=0)
            {
                ModelState.AddModelError("stock", "The stock can't be less than or equal to 0");

            }

            if (obj.Price <= 0)
            {
				ModelState.AddModelError("price", "The price can't be less than or equal to 0");


			}
			if (ModelState.IsValid)
            {

                _db.Books_List.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "New book has been added successfully";
                return RedirectToAction("Index");
            }
			ViewBag.Categories = _db.Categories.Select(c => new SelectListItem
			{

				Value = c.Category_ID.ToString(),
				Text = c.Category_Name


			}).ToList();

			return View(obj);

		}

        //This will display the edit view
        public IActionResult Edit(int? Id)
        {
			if ((Id == null) || (Id == 0))
			{
				return NotFound();
			}

			Books? obj = _db.Books_List.Find(Id);
			if (obj == null)
			{

				return NotFound();
			}
			ViewBag.Categories = _db.Categories.Select(c => new SelectListItem
			{

				Value = c.Category_ID.ToString(),
				Text = c.Category_Name,
                Selected= (c.Category_ID==obj.Category_ID)

			}).ToList();

			return View(obj);

        }


		//This will delete the  Book specific book record
		public IActionResult Delete(int?Id)
        {
            Books? obj_todelete = _db.Books_List.Find(Id);
			if (obj_todelete == null)
			{
				return Json(new { success = false, message = "Error while deleting" });

			}
            _db.Books_List.Remove(obj_todelete);
            _db.SaveChanges();


			return Json(new { success = true, message = "Record deleted successfully" });
		}




        //API CALL for the data of the Book Table
		[HttpGet]
        public IActionResult GetALLBooks()
        {

            List<Books> _books = _db.Books_List.ToList();

            return Json(new {data=_books});
        }
    }
}
