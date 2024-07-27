using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Online_BookStore.DataAccess.Repository;
using Online_BookStore.DataAccess.Repository.IRepository;
using Online_BookStore.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Online_BookStore.Controllers
{
    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork db)
        {
            _logger = logger;
            _unitOfWork = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Books> booksList = _unitOfWork.Book.GetAll(includeProperties: "category,Book_Images");
            return View(booksList);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {

            return View();
        }

        public IActionResult Details(int Id)
        {

            ShoppingCart cart = new()
            {

                Book_Product = _unitOfWork.Book.Get(x => x.Book_Id == Id, includeProperties: "category,Book_Images"),
                count = 1,
                ProductId = Id


            };

            return View(cart);
        }

      
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            //It will claim or retrieve user identity
            var claimUserIdentity = (ClaimsIdentity)User.Identity;
            //
            var userId = claimUserIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId=userId;
            //Retrieve the userId from the ShoppingCart table and the productId(Book Table)
            var cartFromDb= _unitOfWork.ShoppingCart.Get(x=>x.ApplicationUserId==userId && x.ProductId==shoppingCart.ProductId);

            if(shoppingCart.count <=0)
            {
                TempData["error"] = "Qty should not be less than 1";
                return RedirectToAction(nameof(Index));

            }


            if (cartFromDb!=null)
            {
               
                    cartFromDb.count += shoppingCart.count;
                    _unitOfWork.ShoppingCart.Update(cartFromDb);

                    TempData["success"] = "Order updated Successfully";

                
               

            }
            else
            {

                _unitOfWork.ShoppingCart.Add(shoppingCart);
                TempData["success"] = "Order placed Successfully";
            }

            _unitOfWork.Save();


            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
