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
   // [Authorize(SD.Role_Admin)]

    public class CompanyController : Controller
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

		public CompanyController(IUnitOfWork db, ApplicationDbContext new_db)
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

        public IActionResult Create(Company obj)
        {
            if (ModelState.IsValid)
            {

                //_db.Categories.Add(obj);
                //_db.SaveChanges();

                unitOfWork.Company.Add(obj);
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


			Company? obj = unitOfWork.Company.Get(x => x.Id == Id);


			if (obj == null)
            {

                return NotFound();
            }

            return View(obj);
        }



        [HttpPost]
        public IActionResult Edit(Company obj)
        {
            if (ModelState.IsValid)
            {
				var SpecificCompany = _db.Company.AsNoTracking().FirstOrDefault(c => c.Id == obj.Id);
				if (SpecificCompany == null)
                {
                    return NotFound();
                }
                if (SpecificCompany.CompanyName != obj.CompanyName)
                {
                    SpecificCompany.CompanyName = obj.CompanyName;

					unitOfWork.Company.Update(obj);
                    unitOfWork.Save();
                    TempData["success"] = $"Record {obj.Id} updated successfully";
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
            Company? obj_todelete = unitOfWork.Company.Get(x => x.Id == Id);

            if (obj_todelete == null)
            {
                return Json(new { success = false, message = "Error while deleting" });

            }

			


			unitOfWork.Company.Remove(obj_todelete);
            unitOfWork.Save();
            //TempData["success"] = "Record deleted successfully";

            return Json(new { success = true, message = "Record deleted successfully" });
        }

        //This region fetch the API DATA from my database
        [HttpGet]
        public IActionResult GetAll()
        {

            List<Company> _companies = unitOfWork.Company.GetAll().ToList();

            return Json(new { data = _companies });
        }
    }
}
