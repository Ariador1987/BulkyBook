using BulkyBook.Models;
using BulkyBooky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        // ovo nam treba za uploadat slike u root.
        private readonly IWebHostEnvironment _hostEnvironment;


        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var product = new Product();
            if (id == null)
            {
                // CREATE
                return View(product);
            }
            // EDIT
            product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            // if supplied incorrect ID
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Product product)
        {
            if (ModelState.IsValid)
            {
                // CREATE
                if (product.Id == 0)
                {
                    _unitOfWork.Product.Add(product);
                }
                else
                {
                    // U UPDATE CATEGORY REPA METODI POZIVAMO SAVECHANGES / REMOVE-ALI SMO TO , DA BUDEMO CONSISTENT
                    _unitOfWork.Product.Update(product);
                }

                _unitOfWork.Save();
                // DA BI IZBJEGLI MAGIC STRING VRACANJE
                // return RedirectToAction("Index");
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        #region API 
        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Product.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleteing" });
            }
            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
