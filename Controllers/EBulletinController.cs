using Microsoft.AspNetCore.Mvc;
using MyNoteSampleApp.Business;
using MyNoteSampleApp.Models.Entities;
using MyNoteSampleApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Net;
using MyNoteSampleApp.Core;
using MyNoteSampleApp.Core.Filters;

namespace MyNoteSampleApp.Controllers
{
    [LoginFilter]
    [AdminFilter]
    public class EBulletinController : Controller
    {

        private readonly EBulletinService _bulletinService;
        public EBulletinController()
        {
            _bulletinService = new EBulletinService();
        }

        public IActionResult Index()
        {
            return View(_bulletinService.List().Data);
        }  // verileri listele

        public IActionResult Edit(int id) // ıd ile olmalı 
        {
            ServiceResult<EBulletin> result = _bulletinService.Find(id);

            if (result.Data == null)
            {
                return RedirectToAction(nameof(Index));
            }

            EBulletinEditViewModel model = new EBulletinEditViewModel
            {
                Email = result.Data.Email,
                Banned = result.Data.Banned
            };
            return View(model);
        } // verileri düzenle

        [HttpPost]
        public IActionResult Edit(int id, EBulletinEditViewModel model) // ıd ile olmalı 
        {
            if (ModelState.IsValid)
            {
                ServiceResult<EBulletin> result = _bulletinService.Update(id, model);

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
            ServiceResult<EBulletin> result = _bulletinService.Find(id);

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
            ServiceResult<object> result = _bulletinService.Remove(id);

            if (!result.IsError) // eğer hata yoksa index e gönder varsa hata ver
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (string error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error);

            }
            return View(_bulletinService.Find(id).Data);
        }


        public IActionResult SendEmails()
        {
            List<EBulletin> bulletins = _bulletinService.ListExceptBanned().Data;
            var list = bulletins.Select(x => new SelectListItem(x.Email, x.Email)).ToList();

            EBulletinSendEmailsViewModel model = new EBulletinSendEmailsViewModel();
            model.EmailAddresses = new SelectList(list);

            return View(model);
        }

        [HttpPost]
        public IActionResult SendEmails(EBulletinSendEmailsViewModel model)
        {
            MailHelper mailHelper = new MailHelper();
            mailHelper.SendMail(model.Subject, model.Subject, model.Emails.ToArray());

            return RedirectToAction(nameof(Index));
        }
    }
}
