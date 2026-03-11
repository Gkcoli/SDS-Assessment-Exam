using System;
using System.Web.Mvc;
using SDSExam_Colinares.DAL;
using SDSExam_Colinares.Models;

namespace SDSExam_Colinares.Controllers
{
    public class RecyclableItemController : Controller
    {
        private readonly RecyclableItemRepository _repo = new RecyclableItemRepository();
        private readonly RecyclableTypeRepository _typeRepo = new RecyclableTypeRepository();

        private void PopulateDropdown(int? selectedId = null)
        {
            var types = _typeRepo.GetAll();
            ViewBag.RecyclableTypes = new SelectList(types, "Id", "Type", selectedId);
        }

        public ActionResult Index()
        {
            var data = _repo.GetAll();
            return View(data);
        }

        public ActionResult Create()
        {
            PopulateDropdown();
            return View(new RecyclableItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecyclableItem model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdown(model.RecyclableTypeId);
                return View(model);
            }
            try
            {
                _repo.Insert(model);
                TempData["Success"] = "Recyclable Item added successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                PopulateDropdown(model.RecyclableTypeId);
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var model = _repo.GetById(id);
            if (model == null) return HttpNotFound();
            PopulateDropdown(model.RecyclableTypeId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecyclableItem model)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdown(model.RecyclableTypeId);
                return View(model);
            }
            try
            {
                _repo.Update(model);
                TempData["Success"] = "Recyclable Item updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                PopulateDropdown(model.RecyclableTypeId);
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                _repo.Delete(id);
                TempData["Success"] = "Recyclable Item deleted.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetRate(int typeId)
        {
            var type = _typeRepo.GetById(typeId);
            if (type == null) return Json(new { rate = 0 }, JsonRequestBehavior.AllowGet);
            return Json(new { rate = type.Rate }, JsonRequestBehavior.AllowGet);
        }
    }
}