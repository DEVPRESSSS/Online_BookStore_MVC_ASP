using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_BookStore.DataAccess.Repository.IRepository;
using Online_BookStore.Models;
using Online_BookStore.Models.ViewModel;
using System.Security.Claims;

namespace Online_BookStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        //Inject UnitOfWork interface
        private readonly IUnitOfWork _unitOfWork;

        //Add ShoppingCartVM
        public ShoppingCartVM ShoppingCartVM { get; set; }
        //Add constructor
        public CartController(IUnitOfWork unitOfWork) {

            _unitOfWork = unitOfWork;


        }



        public IActionResult Index()
        {
            var claimUserIdentity = (ClaimsIdentity)User.Identity;
            //
            var userId = claimUserIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartlist = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Book_Product"),
                OrderTotal = 0

            };

            foreach(var cart in ShoppingCartVM.ShoppingCartlist)
            {
                cart.ShoppingPrice = cart.Book_Product?.Price ?? 0; 
                ShoppingCartVM.OrderTotal += cart.ShoppingPrice * cart.count; 
            }
            return View(ShoppingCartVM);


        }

        public IActionResult AddQty(int shoppingId)
        {

            var cartFromDb = _unitOfWork.ShoppingCart.Get(x => x.ShoppingId == shoppingId);

            cartFromDb.count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }


    }   
    
}
    

