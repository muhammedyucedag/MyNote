using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyNoteSampleApp.Business;
using MyNoteSampleApp.Core;
using MyNoteSampleApp.Core.Filters;
using MyNoteSampleApp.Models;
using MyNoteSampleApp.Models.Context;
using MyNoteSampleApp.Models.Entities;
using System.Diagnostics;

namespace MyNoteSampleApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly NoteService _noteService;
        private readonly EBulletinService _eBulletinService;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _userService = new UserService();
            _noteService = new NoteService();
            _eBulletinService = new EBulletinService();
        }

        public IActionResult Index(int? categoryId, string mode)
        {
            int userId = HttpContext.Session.GetInt32(Constants.UserId).GetValueOrDefault();

            if (userId > 0)
            {
                List<int> likedNoteIds = _noteService.GetUserLikedNoteIds(userId).Data;
                ViewData["likedNoteIds"] = likedNoteIds;
            }

            if (categoryId == null && string.IsNullOrEmpty(mode))
                return View(_noteService.List(null, false).Data);

            else
                return View(_noteService.List(categoryId, mode, false).Data);

        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                ServiceResult<User> result = _userService.Login(model);

                if (!result.IsError) // eğer hata yoksa index e gönder varsa hata ver
                {
                    HttpContext.Session.SetInt32(Constants.UserId, result.Data.Id);
                    HttpContext.Session.SetString(Constants.Username, result.Data.Username);
                    HttpContext.Session.SetString(Constants.UserEmail, result.Data.EMail);
                    HttpContext.Session.SetString(Constants.UserRole, result.Data.IsAdmin ? "admin" : "member");

                    return RedirectToAction(nameof(Index));
                }

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(model);

        }



        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                ServiceResult<object> result = _userService.Register(model);

                if (!result.IsError)
                    return RedirectToAction(nameof(Login));

                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }
            return View(model);
        }


        [LoginFilter]
        public IActionResult ProfileShow()
        {
            ProfileInfoEditViewModel model = GetProfileEditViewModel();

            return View(model);
        }


        [LoginFilter]
        [HttpPost]
        public IActionResult ProfilePasswordChange(string newPassword)
        {
            ServiceResult<object> result = _userService.ChangePassword(newPassword, HttpContext);

            if (!result.IsError) // eğer hata yoksa 
            {
                ViewData["SuccessMessage"] = "Şifre değişiminiz yapılmıştır.";
            }
            else
            {
                foreach (string error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            ProfileInfoEditViewModel model = new ProfileInfoEditViewModel();


            return View(nameof(ProfileEdit), model);

        }


        [LoginFilter]
        [HttpPost]
        public IActionResult ProfileImageChange(IFormFile profileImage)
        {
            try
            {
                string uploadPath = Path.Combine(Environment.CurrentDirectory,"wwwroot","uploads");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;
                string fileName = $"profile_{userId}.jpg";
                string filePath = Path.Combine(uploadPath, fileName);

                using (FileStream stream = System.IO.File.Create(filePath))
                {
                    profileImage.CopyTo(stream);
                }

                ViewData["SuccessMessage"] = "Profil resminiz güncellenmiştir.";
            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, "Hata oluştu. Profil resmi güncellemedi");
            }

            ProfileInfoEditViewModel model = new ProfileInfoEditViewModel();


            return View(nameof(ProfileEdit), model);
        }


        [LoginFilter]
        [HttpPost]
        public IActionResult ProfileInfoSave(ProfileInfoEditViewModel info)
        {
            if (ModelState.IsValid) // herşey başarlı ise 
            {
                ServiceResult<object> result = _userService.UpdateUserInfo(info, HttpContext);

                if (!result.IsError) // eğer hata yoksa 
                {
                    ViewData["SuccessMessage"] = "Bilgileriniz kaydedilmiştir.";
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
            }
            else
            {
                return View(nameof(ProfileEdit), info);
            }

            ProfileInfoEditViewModel model = new ProfileInfoEditViewModel();
            return View(nameof(ProfileEdit), model);
        }


        [LoginFilter]
        public IActionResult DeleteProfile()
        {
            ProfileInfoEditViewModel model = GetProfileEditViewModel();
            return View(model);
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [LoginFilter]
        public IActionResult ProfileEdit()
        {
            ProfileInfoEditViewModel model = GetProfileEditViewModel();

            return View(model);
        }



        [LoginFilter]
        private ProfileInfoEditViewModel GetProfileEditViewModel()
        {
            int userId = HttpContext.Session.GetInt32(Constants.UserId).Value;
            User user = _userService.Find(userId).Data;

            ProfileInfoEditViewModel model = new ProfileInfoEditViewModel();
            model.Email = user.EMail;
            model.Fullname = user.FullName;
            model.Username = user.Username;
            return model;
        }


        [LoginFilter]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Index));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult GetNoteDetail(int id)
        {
            //Thread.Sleep(2000);

            ServiceResult<Note> result = _noteService.Find(id);

            if (result.Data == null)
            {
                return NotFound(); // StatusCode : 404
            }

            return PartialView("_NoteDetailPartial", result.Data);
        }


        public IActionResult GetNoteComments(int id)
        {
            //Thread.Sleep(2000);

            ServiceResult<Note> result = _noteService.Find(id);

            if (result.Data == null)
            {
                return NotFound(); // StatusCode : 404
            }

            result.Data.Comments.ForEach(c => c.ModifiedAt = c.ModifiedAt != null ? c.ModifiedAt : c.CreatedAt);
            result.Data.Comments = result.Data.Comments.OrderBy(c => c.ModifiedAt).ToList();

            return PartialView("_NoteCommentsPartial", result.Data);
        }

        [HttpPost]
        public IActionResult SaveEBulletinEmail(string eBulletinEmail)
        {
            ServiceResult<object> result = _eBulletinService.Create(eBulletinEmail);

            return RedirectToAction(nameof(Index));

        }
    }
}