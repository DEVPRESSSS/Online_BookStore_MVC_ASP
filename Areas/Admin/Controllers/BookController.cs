using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Online_BookStore.DataAccess.Data;
using Online_BookStore.DataAccess.Repository;
using Online_BookStore.DataAccess.Repository.IRepository;
using Online_BookStore.Models;
using Online_BookStore.Models.ViewModel;
using System.Collections.Generic;
using System.Net;

namespace Online_BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(SD.Role_Admin)]
    public class BookController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(IUnitOfWork db, IWebHostEnvironment webHostEnvironment)
        {
            unitOfWork = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //List<Books> list= unitOfWork.Book.GetAll().ToList();
            List<Books> booksList = unitOfWork.Book.GetAll(includeProperties: "category").ToList();
            return View(booksList);
         

        }


        //Display the view as well as the List of items
        public IActionResult Create()
        {

            BookVM vm = new()
            {

                CategoryList = unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {

                    Text = u.Category_Name,
                    Value = u.Category_ID.ToString()
                }),

                Books = new Books()
            };

    
            return View(vm);

        }

        //This method function is for inserting data

        [HttpPost]
        public IActionResult Create(BookVM bookVM,List<IFormFile>? files)
        {

			if (bookVM.Books.Stock <= 0)
			{
				ModelState.AddModelError("stock", "The stock can't be less than or equal to 0");
			}

			if (bookVM.Books.Price <= 0)
			{
				ModelState.AddModelError("price", "The price can't be less than or equal to 0");
			}

			if (ModelState.IsValid)
			{
				if (bookVM.Books.Book_Id == 0)
				{
					unitOfWork.Book.Add(bookVM.Books);
					unitOfWork.Save();
				}

				string wwwRootPath = _webHostEnvironment.WebRootPath;
				string bookPath = @"Images\Product-" + bookVM.Books.Book_Id;
				string finalPath = Path.Combine(wwwRootPath, bookPath);

				if (!Directory.Exists(finalPath))
				{
					Directory.CreateDirectory(finalPath);
				}

				if (files != null)
				{
					foreach (IFormFile file in files)
					{
						string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
						using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
						{
							file.CopyTo(fileStream);
						}

						BookImages bookImages = new()
						{
							Image_Url = @"\" + bookPath + @"\" + fileName,
							Book_Id = bookVM.Books.Book_Id,
						};

						if (bookVM.Books.Book_Images == null)
						{
							bookVM.Books.Book_Images = new List<BookImages>();
						}

						bookVM.Books.Book_Images.Add(bookImages);
						unitOfWork.Book_list_images.Add(bookImages);
					}

					unitOfWork.Save();
				}

				TempData["success"] = "New book has been added successfully";
				return RedirectToAction("Index");
			}

			bookVM.CategoryList = unitOfWork.Category.GetAll().Select(u => new SelectListItem
			{
				Text = u.Category_Name,
				Value = u.Category_ID.ToString()
			}).ToList();

			return View(bookVM);

		}

        //This will display the edit view
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }

            Books? obj = unitOfWork.Book.Get(x=>x.Book_Id==Id,includeProperties: "Book_Images");
            if (obj == null)
            {

                return NotFound();
            }

            //IT populates the fields with the data
            BookVM vm = new()
            {

                CategoryList = unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {


                    Text = u.Category_Name,
                    Value = u.Category_ID.ToString(),
                }),

                Books = obj
            };


            return View(vm);

        }
        [HttpPost]
        public IActionResult Edit(BookVM bookVM, List<IFormFile>? files)
        {

            if (bookVM == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Retrieve the existing book data
                Books? existingBook = unitOfWork.Book.Get(b => b.Book_Id == bookVM.Books.Book_Id, includeProperties: "Book_Images");
                if (existingBook == null)
                {
                    return NotFound();
                }

                // Update book details
                existingBook.Title = bookVM.Books.Title;
                existingBook.Description = bookVM.Books.Description;
                existingBook.ISBN = bookVM.Books.ISBN;
                existingBook.Price = bookVM.Books.Price;
                existingBook.PublishDate = bookVM.Books.PublishDate;
                existingBook.Publisher = bookVM.Books.Publisher;
                existingBook.Stock = bookVM.Books.Stock;
                existingBook.Category_ID = bookVM.Books.Category_ID;

                // Handle image upload
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string bookPath = @"Images\Product-" + bookVM.Books.Book_Id;
                        string finalPath = Path.Combine(wwwRootPath, bookPath);

                        if (!Directory.Exists(finalPath))
                        {
                            Directory.CreateDirectory(finalPath);
                        }

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        BookImages bookImages = new()
                        {
                            Image_Url = @"\" + bookPath + @"\" + fileName,
                            Book_Id = bookVM.Books.Book_Id,
                        };

                        if (existingBook.Book_Images == null)
                        {
                            existingBook.Book_Images = new List<BookImages>();
                        }

                        existingBook.Book_Images.Add(bookImages);
                        unitOfWork.Book_list_images.Add(bookImages);
                    }
                }

                // Update the book
                unitOfWork.Book.Update(existingBook);
                unitOfWork.Save();

                TempData["success"] = "Updated Successfully";
                return RedirectToAction("Index");

            }

            return View(bookVM);

        }
        //This will delete the  Book specific book record

        public IActionResult Delete(int? Id)
        {
			Books? objToDelete = unitOfWork.Book.Get(x => x.Book_Id == Id, includeProperties: "Book_Images");
			if (objToDelete == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

			// Delete images from the file system
			string wwwRootPath = _webHostEnvironment.WebRootPath;
			foreach (var image in objToDelete.Book_Images)
			{
				string imagePath = Path.Combine(wwwRootPath, image.Image_Url.TrimStart('\\'));
				if (System.IO.File.Exists(imagePath))
				{
					System.IO.File.Delete(imagePath);
				}
			}

			// Remove the directory if it exists
			string bookPath = @"Images\Product-" + objToDelete.Book_Id;
			string finalPath = Path.Combine(wwwRootPath, bookPath);
			if (Directory.Exists(finalPath))
			{
				Directory.Delete(finalPath, true);
			}

			// Remove the book and its images from the database
			unitOfWork.Book.Remove(objToDelete);
			unitOfWork.Save();
			//TempData["success"] = "Image removed successfully";

			return Json(new { success = true, message = "Record deleted successfully" });
		}




        //API CALL for the data of the Book Table
        [HttpGet]
        public IActionResult GetALLBooks()
        {
            
                List<Books> booksList = unitOfWork.Book.GetAll(includeProperties: "category").ToList();
                return Json(new { data = booksList });
            
            
        }

        [HttpPost]
        public IActionResult DeleteImage(int imageId, int bookId)
        {
			var image = unitOfWork.Book_list_images.Get(i => i.Image_ID == imageId);
			if (image == null)
			{
				return Json(new { success = false, message = "Image not found" });
			}

			// Delete the image file from the file system
			string wwwRootPath = _webHostEnvironment.WebRootPath;
			string imagePath = Path.Combine(wwwRootPath, image?.Image_Url.TrimStart('\\'));

			if (System.IO.File.Exists(imagePath))
			{
				System.IO.File.Delete(imagePath);
			}

			// Remove the image record from the database
			unitOfWork.Book_list_images.Remove(image);
			unitOfWork.Save();


			TempData["success"] = "Image removed successfully";
			return RedirectToAction("Edit", new { id = bookId });

		}
	}
}
