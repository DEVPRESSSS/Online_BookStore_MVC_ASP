using Microsoft.AspNetCore.Mvc;
using Online_BookStore.DataAccess.Repository;
using Online_BookStore.DataAccess.Repository.IRepository;
using Online_BookStore.Models;
using System.Diagnostics;

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


        public IActionResult Details(int? Id)
        {

            if(Id == null)
            {

                return NotFound();
            }

            Books book = _unitOfWork.Book.Get(x => x.Book_Id == Id, includeProperties: "category,Book_Images");
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
