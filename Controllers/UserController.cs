using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyNoteSampleApp.Business;
using MyNoteSampleApp.Core;
using MyNoteSampleApp.Core.Filters;
using MyNoteSampleApp.Models;
using MyNoteSampleApp.Models.Entities;
using System.Collections.Generic;

namespace MyNoteSampleApp.Controllers
{
    [LoginFilter]
    [AdminFilter]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController()
        {
            _userService = new UserService();
        }

        public IActionResult Index()
        {
            ServiceResult<List<User>> result = _userService.List();

            return View(result.Data);
        }

        public IActionResult Details(int id)
        {
            ServiceResult<User> result = _userService.Find(id);

            if (result.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                ServiceResult<User> result = _userService.Create(model, HttpContext);

                if (!result.IsError) // eğer hata yoksa index e gönder varsa hata ver
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(model);

        }

        // User/Edit/1 
        public IActionResult Edit(int id) // ıd ile olmalı 
        {
            //var role = HttpContext.Session.GetString(Constants.UserRole);

            //if (role == null || role == "member")
            //{
            //    return RedirectToAction("Login", "Home");
            //}

            ServiceResult<User> result = _userService.Find(id);

            if (result.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Id = id;

            UserEditViewModel model = new UserEditViewModel
            {
                Fullname = result.Data.FullName,
                Username = result.Data.Username,
                Email = result.Data.EMail,
                IsActive = result.Data.IsActive,
                IsAdmin = result.Data.IsAdmin
            };
            return View(model);
        }


        [HttpPost]
        public IActionResult Edit(int id, UserEditViewModel model) // ıd ile olmalı 
        {
            if (ModelState.IsValid)
            {
                ServiceResult<User> result = _userService.Update(id, model, HttpContext);

                if (!result.IsError) // eğer hata yoksa index e gönder varsa hata ver
                {
                    return RedirectToAction(nameof(Index));
                }

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(model);
        }

        public IActionResult Delete(int id) // ıd ile olmalı 
        {
            ServiceResult<User> result = _userService.Find(id);

            if (result.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data); ;
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            ServiceResult<object> result = _userService.Remove(id);

            if (!result.IsError) // eğer hata yoksa index e gönder varsa hata ver
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);
            }
            return View(_userService.Find(id).Data);
        }
    }
}
