using MyNoteSampleApp.Core;
using MyNoteSampleApp.Models;
using MyNoteSampleApp.Models.Context;
using MyNoteSampleApp.Models.Entities;

namespace MyNoteSampleApp.Business
{
    public class CategoryService
    {
        private DatabaseContext _db = new DatabaseContext();

        public ServiceResult<Category> Find(int id)
        {
            ServiceResult<Category> result = new ServiceResult<Category>()
            {
                Data = _db.Categories.Find(id)
            };

            if (result.Data == null)
                result.AddError("Kayıt Bulunamadı");

            return result;
        }


        public ServiceResult<Category> Update(int id, CategoryViewModel model, HttpContext httpContext)
        {
            ServiceResult<Category> result = new ServiceResult<Category>();

            model.Name = model.Name.Trim();

            if (_db.Categories.Any(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != id))
            {

                result.AddError($"'{model.Name}' isimli kategori zaten kayıtlıdır!");
                return result;
            }

            Category category = _db.Categories.Find(id);

            category.Name = model.Name;
            category.Description = model.Description;

            category.ModifiedUser = httpContext.Session.GetString(Constants.Username);
            category.ModifiedAt = DateTime.Now;


            if (_db.SaveChanges() == 0)
                // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                result.AddError("Güncelleme yapılamadı.");
            else
                result.Data = category;

            return result;
        }


        public ServiceResult<List<Category>> List()
        {
            List<Category> categories = _db.Categories.ToList();

            ServiceResult<List<Category>> result = new ServiceResult<List<Category>>();
            result.Data = categories;

            return result;

        }


        public ServiceResult<Category> Create(CategoryViewModel model, HttpContext httpContext)
        {
            ServiceResult<Category> result = new ServiceResult<Category>();

            model.Name = model.Name.Trim();


            if (_db.Categories.Any(x => x.Name.ToLower() == model.Name.ToLower()))
            {
                // kayıt işlemi sırasında o email eşit email varsa  ıserror true olacak ve  adresi zaten kayıtlıdır! adında hata dönecek

                result.AddError($"'{model.Name}' kategorisi zaten kayıtlıdır!");
                return result;
            }

            Category category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                CreatedAt = DateTime.Now,
                CreatedUser = httpContext.Session.GetString(Constants.Username)
            };

            _db.Categories.Add(category);

            if (_db.SaveChanges() == 0)
                // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                result.AddError("Kayıt yapılamadı.");
            else
                result.Data = category;

            return result;
        }


        public ServiceResult<object> Remove(int id)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            Category category = _db.Categories.Find(id);   // db e bak bakalım o id ait kayıt var mı 

            if (category != null)   // o id ait kayıt varsa 
            {
                _db.Categories.Remove(category);   // o id ait kaydı sil 

                if (_db.SaveChanges() == 0)   // kaydet ama 0 gelirse 

                    // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                    result.AddError("Silme işlemi yapılamadı.");   // silme işlemi olmayacak
            }
            return result;

        }
    } 

}
