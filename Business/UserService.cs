using MyNoteSampleApp.Core;
using MyNoteSampleApp.Models;
using MyNoteSampleApp.Models.Context;
using MyNoteSampleApp.Models.Entities;
using NETCore.Encrypt.Extensions;
using System.Linq;
using System.Reflection.Metadata;

namespace MyNoteSampleApp.Business
{

    public class UserService
    {
        private DatabaseContext _db = new DatabaseContext();


        public ServiceResult<object> Register(RegisterViewModel model)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            model.Username = model.Username.Trim().ToLower();
            model.Email = model.Email.Trim().ToLower();


            if (_db.Users.Any(x => x.EMail.ToLower() == model.Email.ToLower()))
            {
                // kayıt işlemi sırasında o email eşit email varsa  ıserror true olacak ve  adresi zaten kayıtlıdır! adında hata dönecek

                result.AddError($"'{model.Email}' adresi zaten kayıtlıdır!");
                return result;
            }

            if (_db.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
            {
                // kayıt işlemi sırasında o kullanıcıadına eşit kullancı adı varsa  ıserror true olacak ve kullanıcı adı zaten kayıtlıdır! adında hata dönecek


                result.AddError($"'{model.Username}' kullanıcı adı zaten kayıtlıdır!");
                return result;
            }

            _db.Users.Add(new User
            {
                Username = model.Username,
                EMail = model.Email,
                Password = $"{Constants.EncrpytionSalt}{model.Password}".MD5(),
                IsActive = true,
                IsAdmin = false,
                CreatedUser = "register",
                CreatedAt = DateTime.Now,

            });

            if (_db.SaveChanges() == 0)
            {
                // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                result.AddError("Kayıt yapılamadı.");
            }

            return result;
        }





        public ServiceResult<User> Login(LoginViewModel model)
        {
            ServiceResult<User> result = new ServiceResult<User>();

            model.Username = model.Username.Trim().ToLower();

            string passwordHashed = $"{Constants.EncrpytionSalt}{model.Password}".MD5();

            User user = _db.Users.FirstOrDefault(x => 
            x.Username.ToLower() == model.Username.Trim().ToLower() && x.Password == passwordHashed);

            if (user == null)
            {
                result.AddError("Hatalı kullanıcı adı ya da şifre!");
            }
            else
            {
                user.Password = string.Empty;
                result.Data = user;
            }
            return result;
        }



        public ServiceResult<object> ChangePassword(string newPass, HttpContext httpContext)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            newPass= newPass?.Trim();

            if (string.IsNullOrEmpty(newPass))
            {
                result.AddError("Şifre alanı boş olamaz.");
            }

            else
            {
                int userId = httpContext.Session.GetInt32(Constants.UserId).Value;
                string passwordHashed = $"{Constants.EncrpytionSalt}{newPass}".MD5();

                User user = _db.Users.Find(userId);
                user.Password = passwordHashed;


                if (_db.SaveChanges() == 0)
                    result.AddError("İşlem yapılamadı.");
            }
            

            return result;
        }


        public ServiceResult<List<User>> List()
        {
            List<User> users = _db.Users.ToList();

            users.ForEach(user => user.Password = string.Empty);

            ServiceResult<List<User>> result = new ServiceResult<List<User>>();
            result.Data = users;

            return result;

        }





        public ServiceResult<User> Create(UserViewModel model, HttpContext httpContext)
        {
            ServiceResult<User> result = new ServiceResult<User>();

            model.Username = model.Username.Trim().ToLower();
            model.Email = model.Email.Trim().ToLower();


            if (_db.Users.Any(x => x.EMail.ToLower() == model.Email.ToLower()))
            {
                // kayıt işlemi sırasında o email eşit email varsa  ıserror true olacak ve  adresi zaten kayıtlıdır! adında hata dönecek

                result.AddError($"'{model.Email}' adresi zaten kayıtlıdır!");
                return result;
            }

            if (_db.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
            {
                // kayıt işlemi sırasında o kullanıcıadına eşit kullancı adı varsa  ıserror true olacak ve kullanıcı adı zaten kayıtlıdır! adında hata dönecek


                result.AddError($"'{model.Username}' kullanıcı adı zaten kayıtlıdır!");
                return result;
            }

            User user = new User
            {
                FullName = model.Fullname,
                Username = model.Username,
                EMail = model.Email,
                Password = model.Password,
                IsActive = model.IsActive,
                IsAdmin = model.IsAdmin,
                CreatedAt = DateTime.Now,
                CreatedUser = httpContext.Session.GetString(Constants.Username)
            };

            _db.Users.Add(user);

            if (_db.SaveChanges() == 0)
                // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                result.AddError("Kayıt yapılamadı.");
            else
                result.Data = user;

            return result;
        }




        public ServiceResult<User> Find(int id)
        {
            ServiceResult<User> result = new ServiceResult<User>()
            {
                Data = _db.Users.Find(id)
            };

            if (result.Data == null)
                result.AddError("Kayıt Bulunamadı");
            else
                result.Data.Password = string.Empty;

            return result;
        }




        public ServiceResult<User> Update(int id, UserEditViewModel model, HttpContext httpContext)
        {
            ServiceResult<User> result = new ServiceResult<User>();

            model.Username = model.Username.Trim().ToLower();
            model.Email = model.Email.Trim().ToLower();


            if (_db.Users.Any(x => x.EMail.ToLower() == model.Email.ToLower() && x.Id != id))
            {

                result.AddError($"'{model.Email}' adresi zaten kayıtlıdır!");
                return result;
            }

            if (_db.Users.Any(x => x.Username.ToLower() == model.Username.ToLower() && x.Id != id))
            {

                result.AddError($"'{model.Username}' kullanıcı adı zaten kayıtlıdır!");
                return result;
            }
            User user = _db.Users.Find(id);

            user.FullName = model.Fullname;
            user.Username = model.Username;
            user.EMail = model.Email;
            user.IsActive = model.IsActive;
            user.IsAdmin = model.IsAdmin;
            user.ModifiedUser = httpContext.Session.GetString(Constants.Username);
            user.ModifiedAt = DateTime.Now;


            if (_db.SaveChanges() == 0)
                // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                result.AddError("Güncelleme yapılamadı.");
            else
                result.Data = user;

            return result;
        }



        public ServiceResult<object> Remove(int id)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            User user = _db.Users.Find(id);   // db e bak bakalım o id ait kayıt var mı 

            if (user != null)   // o id ait kayıt varsa 
            {
                _db.Users.Remove(user);   // o id ait kaydı sil 

                if (_db.SaveChanges() == 0)   // kaydet ama 0 gelirse 

                    // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                    result.AddError("Silme işlemi yapılamadı.");   // silme işlemi olmayacak
            }
            return result;

        }



        public ServiceResult<object> UpdateUserInfo(ProfileInfoEditViewModel model, HttpContext httpContext)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            model.Email = model.Email.Trim().ToLower();
            model.Fullname = model.Fullname.Trim();

            int userId = httpContext.Session.GetInt32(Constants.UserId).Value;


            if (_db.Users.Any(x => x.EMail.ToLower() == model.Email.ToLower() && x.Id != userId))
            {

                result.AddError($"'{model.Email}' adresi zaten kayıtlıdır!");
                return result;
            }

            User user = _db.Users.Find(userId);

            user.FullName = model.Fullname;
            user.EMail = model.Email;
            user.ModifiedUser = httpContext.Session.GetString(Constants.Username);
            user.ModifiedAt = DateTime.Now;


            if (_db.SaveChanges() == 0)
                // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                result.AddError("Güncelleme yapılamadı.");
            else
                result.Data = user;

            return result;
        }
    }

}
