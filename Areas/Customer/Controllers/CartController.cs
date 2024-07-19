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
                ShoppingCartlist= _unitOfWork.ShoppingCart.GetAll(u=>u.ApplicationUserId == userId, includeProperties: "Book_Product"),


            };

            return View(ShoppingCartVM);


        }
    }   
    
}
    

