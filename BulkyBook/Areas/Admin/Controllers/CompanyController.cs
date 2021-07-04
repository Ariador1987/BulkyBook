using BulkyBook.Models;
using BulkyBooky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
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

        public IActionResult Upsert(int? id)
        {
            var company = new Company();
            if (id == null)
            {
                // CREATE
                return View(company);
            }
            // EDIT
            company = _unitOfWork.Company.Get(id.GetValueOrDefault());
            // if supplied incorrect ID
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                // CREATE
                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                }
                else
                {
                    // U UPDATE CATEGORY REPA METODI POZIVAMO SAVECHANGES / REMOVE-ALI SMO TO , DA BUDEMO CONSISTENT
                    _unitOfWork.Company.Update(company);
                }

                _unitOfWork.Save();
                // DA BI IZBJEGLI MAGIC STRING VRACANJE
                // return RedirectToAction("Index");
                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        #region API 
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Company.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Company.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleteing" });
            }
            _unitOfWork.Company.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
