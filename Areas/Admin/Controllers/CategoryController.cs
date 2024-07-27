using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_BookStore.DataAccess.Data;
using Online_BookStore.DataAccess.Repository;
using Online_BookStore.DataAccess.Repository.IRepository;
using Online_BookStore.Models;
using System.Drawing;

namespace Online_BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {

        private readonly ApplicationDbContext _db;
        //New version of using IcategoryRepository**************
        private readonly IUnitOfWork unitOfWork;


		//Constructor when using ApplicationDbContext
		/*public CategoryController(ApplicationDbContext db)
        {

            _db = db;
        }
        /*/

		public CategoryController(IUnitOfWork db, ApplicationDbContext new_db)
        {
            unitOfWork = db;
            _db = new_db;

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

                //_db.Categories.Add(obj);
                //_db.SaveChanges();

                unitOfWork.Category.Add(obj);
                unitOfWork.Save();
                TempData["success"] = "Created successfully";

                return RedirectToAction("Index");

            }
            return View(obj);

        }
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

			//Category? obj= _db.Categories.Find(Id);


			Category? obj = unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Id, tracked: false);


			if (obj == null)
            {

                return NotFound();
            }

            return View(obj);
        }



        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
				var originalCategory = _db.Categories.AsNoTracking().FirstOrDefault(c => c.Category_ID == obj.Category_ID);
				if (originalCategory == null)
                {
                    return NotFound();
                }
                if (originalCategory.Category_Name != obj.Category_Name)
                {
					originalCategory.Category_Name = obj.Category_Name;

					unitOfWork.Category.Update(obj);
                    unitOfWork.Save();
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
        [HttpDelete]
        public IActionResult Delete(int? Id)
        {
            Category? obj_todelete = unitOfWork.Category.Get(x => x.Category_ID == Id, includeProperties:"books");

            if (obj_todelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });

            }

			


			unitOfWork.Category.Remove(obj_todelete);
            unitOfWork.Save();
            //TempData["success"] = "Record deleted successfully";

            return Json(new { success = true, message = "Record deleted successfully" });
        }

        //This region fetch the API DATA from my database
        [HttpGet]
        public IActionResult GetAll()
        {

            List<Category> _categories = unitOfWork.Category.GetAll().ToList();

            return Json(new { data = _categories });
        }
    }
}
