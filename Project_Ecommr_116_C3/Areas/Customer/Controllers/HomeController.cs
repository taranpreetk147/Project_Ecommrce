using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_Ecommr_116_C3.DataAccess.Repository.IRepository;
using Project_Ecommr_116_C3.Models;
using Project_Ecommr_116_C3.Models.ViewModels;
using Project_Ecommr_116_C3.Utility;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace Project_Ecommr_116_C3.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string searchTerm,string searchBy)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim=claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            if(!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm=searchTerm.ToLower();
                if(searchBy=="Title")
                {
                    productList = productList.Where(s => s.Title.ToLower().Contains(searchTerm));
                }
                if(searchBy=="Author")
                {
                    productList = productList.Where(s => s.Author.ToLower().Contains(searchTerm));
                }
                else
                {
                    productList=productList.Where(s=>s.Title.ToLower().Contains(searchTerm) || s.Author.ToLower().Contains(searchTerm));
                }
            }
            if(productList.Count()==0)
            {
                ViewBag.nodata = "No Data Found";
            }
            ViewBag.searchTerm = searchTerm;
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Details(int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(sc => sc.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.Ss_CartSessionCount, count);
            }
            var productInDb = _unitOfWork.Product.FirstOrDefault
                (x => x.Id == id, includeProperties: "Category,CoverType");
            if (productInDb == null) return NotFound();
            var shoppingCart = new ShoppingCart()
            {
                ProductId = id,
                Product=productInDb
            };
            return View(shoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            if(ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claims == null) return NotFound();
                shoppingCart.ApplicationUserId = claims.Value;
                var shoppingCartFromDb = _unitOfWork.ShoppingCart.FirstOrDefault
                    (sc => sc.ApplicationUserId == claims.Value && sc.ProductId == shoppingCart.ProductId);
                if (shoppingCartFromDb == null)
                    _unitOfWork.ShoppingCart.Add(shoppingCart);
                else
                    shoppingCartFromDb.Count += shoppingCart.Count;
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }    
            else
            {
                var productIndb = _unitOfWork.Product.FirstOrDefault
                               (x => x.Id == shoppingCart.Id, includeProperties: "Category,CoverType");
                if (productIndb == null) return NotFound();
                var shoppingCartEdit = new ShoppingCart()
                {
                    ProductId = shoppingCart.Id,
                    Product = productIndb
                };
                return View(shoppingCartEdit);
            }
        }
    }
}