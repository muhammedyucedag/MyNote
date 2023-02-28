using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyNoteSampleApp.Business;
using MyNoteSampleApp.Core;
using MyNoteSampleApp.Core.Filters;
using MyNoteSampleApp.Models;
using MyNoteSampleApp.Models.Entities;
using static System.Net.Mime.MediaTypeNames;

namespace MyNoteSampleApp.Controllers
{
    [LoginFilter]
    public class NoteController : Controller
    {
        private readonly CategoryService _categoryService;
        private readonly NoteService _noteService;

        public NoteController()
        {
            _categoryService = new CategoryService();
            _noteService = new NoteService();
        }

        public IActionResult Index()
        {
            int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;

            return View(_noteService.List(userId,true).Data);
        }

        public IActionResult FavoriteNote()
        {
            int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;

            return View(_noteService.ListLikedNotes(userId).Data);
        }

        public IActionResult Details(int id)
        {
            ServiceResult<Note> result = _noteService.Find(id);

            int loggedUserId = HttpContext.Session.GetInt32(Constants.UserId).Value;

            if (result.Data == null || (result.Data != null && result.Data.OwnerId != loggedUserId))
            {
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        public IActionResult Create()
        {
            LoadCategorySelectListDataToViewData();

            return View();
        }


        private void LoadCategorySelectListDataToViewData()
        {
            List<Category> categories = _categoryService.List().Data;

            List<SelectListItem> selectListItems =
                categories.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();

            ViewData["categories"] = selectListItems;
        }

        [HttpPost]
        public IActionResult Create(NoteViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResult<Note> result = _noteService.Create(model, HttpContext);

                if (!result.IsError) // eğer hata yoksa index e gönder varsa hata ver
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            List<Category> categories = _categoryService.List().Data;

            List<SelectListItem> selectListItems =
                categories.Select(c => new SelectListItem(c.Name, c.Id.ToString())).ToList();

            ViewData["categories"] = selectListItems;

            return View(model);
        }


        public IActionResult Edit(int id) // ıd ile olmalı 
        {
            ServiceResult<Note> result = _noteService.Find(id);

            int loggedUserId = HttpContext.Session.GetInt32(Constants.UserId).Value;

            if (result.Data == null || (result.Data != null && result.Data.OwnerId != loggedUserId))
            {
                return RedirectToAction(nameof(Index));
            }

            NoteViewModel model = new NoteViewModel
            {
                Title = result.Data.Title,
                Summary = result.Data.Summary,
                Detail = result.Data.Detail,
                IsDraft = result.Data.IsDraft,
                CategoryId = result.Data.CategoryId
            };

            LoadCategorySelectListDataToViewData();

            return View(model);

        }

        [HttpPost]
        public IActionResult Edit(int id, NoteViewModel model) // ıd ile olmalı 
        {
            if (ModelState.IsValid)
            {
                ServiceResult<Note> result = _noteService.Update(id, model, HttpContext);

                if (!result.IsError) // eğer hata yoksa index e gönder varsa hata ver
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            LoadCategorySelectListDataToViewData();

            return View(model);
        }


        public IActionResult Delete(int id) // ıd ile olmalı 
        {
            ServiceResult<Note> result = _noteService.Find(id);

            if (result.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);

        }


        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            ServiceResult<object> result = _noteService.Remove(id);

            if (!result.IsError) // eğer hata yoksa index e gönder varsa hata ver
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View(_noteService.Find(id).Data);
        }


        [HttpPost]
        public IActionResult AddCommentToNote(int id, string text)
        {
            ServiceResult<Note> result = _noteService.AddComment(id, text, HttpContext);

            return Json(new {hasError = result.IsError});
        }

        [HttpPost]
        public IActionResult RemoveComment(int id)
        {
            ServiceResult<Note> result = _noteService.RemoveComment(id);
            return Json(new { hasError = result.IsError });
        }

        [HttpPost]
        public IActionResult UpdateComment(int id, string text)
        {
            ServiceResult<Note> result = _noteService.UpdateComment(id, text, HttpContext);
            return Json(new { hasError = result.IsError });
        }

        [HttpPost]
        public IActionResult LikeNote(int id)
        {
            ServiceResult<int> result = _noteService.ChangeNoteLikeState(id,HttpContext);
            return Json(new { hasError = result.IsError, likeCount = result.Data });
        }

    }
}
