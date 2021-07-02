using BulkyBook.Models;
using BulkyBook.Utility;
using BulkyBooky.DataAccess.Repository.IRepository;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var coverType = new CoverType();


            if (id == null)
            {
                // CREATE
                return View(coverType);
            }
            // EDIT 
            //coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            coverType = _unitOfWork.SP_Call.OneRecord<CoverType>(StaticDetails.Proc_CoverType_Get, parameter);
            // if incorrect ID 
            if (coverType == null)
            {
                return NotFound();
            }

            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", coverType.Name);

                // CREATE
                if (coverType.Id == 0)
                {
                    //_unitOfWork.CoverType.Add(coverType);
                    _unitOfWork.SP_Call.Execute(StaticDetails.Proc_CoverType_Create, parameter);
                }
                else
                {
                    // Update
                    //_unitOfWork.CoverType.Update(coverType);
                    parameter.Add("@Id", coverType.Id);
                    _unitOfWork.SP_Call.Execute(StaticDetails.Proc_CoverType_Update, parameter);
                }

                _unitOfWork.Save();

                return RedirectToAction(nameof(Index));
            }

            // ako je invalid ModelState vračamo isti view
            return View(coverType);
        }

        #region API Calls
        [HttpGet]
        public IActionResult GetAll()
        {
            // sad radimo refaktoring na SP Callove
            // var allObj = _unitOfWork.CoverType.GetAll();

            // da nakon <CoverType> ne unosimo Magic stringove, u Utility Class library ćemo napraviti konstante za njih.
            var allObj = _unitOfWork.SP_Call.List<CoverType>(StaticDetails.Proc_CoverType_GetAll, null);
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            var objFromDb = _unitOfWork.SP_Call.OneRecord<CoverType>(StaticDetails.Proc_CoverType_Get, parameter);

            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.SP_Call.Execute(StaticDetails.Proc_CoverType_Delete, parameter);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });

            //var objFromDb = _unitOfWork.CoverType.Get(id);
            //if (objFromDb == null)
            //{
            //    return Json(new { success = false, message = "Error while deleting" });
            //}

            //_unitOfWork.CoverType.Remove(objFromDb);
            //_unitOfWork.Save();

            //return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
