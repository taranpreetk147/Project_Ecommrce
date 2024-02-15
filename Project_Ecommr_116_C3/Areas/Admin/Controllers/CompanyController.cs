using Microsoft.AspNetCore.Mvc;
using Project_Ecommr_116_C3.DataAccess.Repository.IRepository;
using Project_Ecommr_116_C3.Models;
using System.Drawing.Text;

namespace Project_Ecommr_116_C3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _unitOfWork.Company.GetAll() });
        }
        [HttpDelete]
        public IActionResult Delete(int id) 
        {
           var productInDb = _unitOfWork.Company.Get(id);
            if (productInDb == null)
                return Json(new { success = false, message = "Something went wrong while delete data!!!" });
            _unitOfWork.Company.Remove(productInDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Data deleted successfully" });
        }
        #endregion
        public IActionResult Upsert(int? id)
        {
            Company company = new Company();
            if (id == null) return View(company);
            company=_unitOfWork.Company.Get(id.GetValueOrDefault());
            if (company == null) return BadRequest();
            return View(company);
        }
        [HttpPost]
        public IActionResult Upsert(Company company)
        {
            if (company==null) return BadRequest();
            if (!ModelState.IsValid) return View(company);
            if(company.Id==0)
                _unitOfWork.Company.Add(company);
            _unitOfWork.Company.Update(company);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
