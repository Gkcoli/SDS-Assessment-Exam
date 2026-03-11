using System;
using System.Web.Mvc;
using SDSExam_Colinares.DAL;
using SDSExam_Colinares.Models;

namespace SDSExam_Colinares.Controllers
{
    public class RecyclableTypeController : Controller
    {
        private readonly RecyclableTypeRepository _repo = new RecyclableTypeRepository();

        public ActionResult Index()
        {
            var data = _repo.GetAll();
            return View(data);
        }

        public ActionResult Create()
        {
            return View(new RecyclableType());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecyclableType model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                _repo.Insert(model);
                TempData["Success"] = "Recyclable Type added successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            var model = _repo.GetById(id);
            if (model == null) return HttpNotFound();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecyclableType model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                _repo.Update(model);
                TempData["Success"] = "Recyclable Type updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
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
                TempData["Success"] = "Recyclable Type deleted.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }
    }
}