using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_BookStore.Data;
using Online_BookStore.Models;

namespace Online_BookStore.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {

            _db = db;
        }
        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

		[HttpPost]

		public IActionResult Create(Category obj)
		{
			if (ModelState.IsValid)
			{

				_db.Categories.Add(obj);
				_db.SaveChanges();
				TempData["success"] = "Created successfully";

				return RedirectToAction("Index");

			}
			return View(obj);

		}
		public IActionResult Edit(int? Id)
        {
            if((Id == null) || (Id==0))
            {
                return NotFound();
            }
            
            Category? obj= _db.Categories.Find(Id);
            if(obj == null)
            {

                return NotFound();
            }

            return View(obj);
        }



        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if(ModelState.IsValid)
            {
                var originalCategory = _db.Categories.AsNoTracking().FirstOrDefault(c => c.Category_ID == obj.Category_ID);
                if (originalCategory == null)
                {
                    return NotFound();
                }
                if (originalCategory.Category_Name != obj.Category_Name)
                {
                    _db.Categories.Update(obj);
                    _db.SaveChanges();
                    TempData["success"] = $"Record {obj.Category_ID} updated successfully";
                }
                else
                {
                    TempData["info"] = $"There is no changes has been made";
                }



                return RedirectToAction("Index");   
            }


            return View();
          

        }



     

        //This action will check the Category ID if it is valid or not
        public IActionResult Delete(int?id)
        {
            Category? obj_todelete= _db.Categories.Find(id);

			if (obj_todelete == null)
			{
				return Json(new { success = false, message = "Error while deleting" });

			}

			_db.Categories.Remove(obj_todelete);
			_db.SaveChanges();
			//TempData["success"] = "Record deleted successfully";

			return Json(new { success = true, message = "Record deleted successfully" });
		}

		//This region fetch the API DATA from my database
		[HttpGet]
        public IActionResult GetAll()
        {

            List<Category> _categories = _db.Categories.ToList();

            return Json(new { data =_categories });
        }
    }
}
