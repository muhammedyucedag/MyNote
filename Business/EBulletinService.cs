using MyNoteSampleApp.Models;
using MyNoteSampleApp.Models.Context;
using MyNoteSampleApp.Models.Entities;

namespace MyNoteSampleApp.Business
{
    public class EBulletinService
    {
        private DatabaseContext _db = new DatabaseContext();

        public ServiceResult<object> Create(string email)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            email = email.Trim().ToLower();


            if (_db.EBulletins.Any(x => x.Email.ToLower() == email))
            {
                // kayıt işlemi sırasında o email eşit email varsa  ıserror true olacak ve  adresi zaten kayıtlıdır! adında hata dönecek

                result.AddError($"'{email}' adresi zaten kayıtlıdır!");
                return result;
            }

            EBulletin bulletin = new EBulletin
            {
                Email= email,
                Banned=false,
                CreatedAt = DateTime.Now,   
            };

            _db.EBulletins.Add(bulletin);

            if (_db.SaveChanges() == 0)
                // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                result.AddError("Kayıt yapılamadı.");

            return result;
        }

        public ServiceResult<List<EBulletin>> List()
        {
            List<EBulletin> ebulletins = _db.EBulletins.ToList();

            ServiceResult<List<EBulletin>> result = new ServiceResult<List<EBulletin>>();
            result.Data = ebulletins;

            return result;

        }

        public ServiceResult<List<EBulletin>> ListExceptBanned()
        {
            List<EBulletin> ebulletins = _db.EBulletins.Where(x => x.Banned == false).ToList();

            ServiceResult<List<EBulletin>> result = new ServiceResult<List<EBulletin>>();
            result.Data = ebulletins;

            return result;

        }

        public ServiceResult<EBulletin> Find(int id)
        {
            ServiceResult<EBulletin> result = new ServiceResult<EBulletin>()
            {
                Data = _db.EBulletins.Find(id)
            };

            if (result.Data == null)
                result.AddError("Kayıt Bulunamadı");

            return result;
        }

        public ServiceResult<EBulletin> Update(int id, EBulletinEditViewModel model)
        {
            ServiceResult<EBulletin> result = new ServiceResult<EBulletin>();

            model.Email = model.Email.Trim().ToLower();

            
            if (_db.EBulletins.Any(x => x.Email.ToLower() == model.Email && x.Id != id))
            {
                // kayıt işlemi sırasında o email eşit email varsa  ıserror true olacak ve  adresi zaten kayıtlıdır! adında hata dönecek

                result.AddError($"'{model.Email}' adresi zaten kayıtlıdır!");
                return result;
            }

            EBulletin ebulletin = _db.EBulletins.Find(id);

            ebulletin.Email = model.Email;
            ebulletin.Banned = model.Banned;


            if (_db.SaveChanges() == 0)
                // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                result.AddError("Güncelleme yapılamadı.");
            else
                result.Data = ebulletin;

            return result;
        }

        public ServiceResult<object> Remove(int id)
        {
            ServiceResult<object> result = new ServiceResult<object>();

            EBulletin eBulletin = _db.EBulletins.Find(id);   // db e bak bakalım o id ait kayıt var mı 

            if (eBulletin != null)   // o id ait kayıt varsa 
            {
                _db.EBulletins.Remove(eBulletin);   // o id ait kaydı sil 

                if (_db.SaveChanges() == 0)   // kaydet ama 0 gelirse 

                    // kayıt işlemi eşitse sıfıra o zaman ıserror true olacak ve kayıt yapılamadı adında hata dönecek
                    result.AddError("Silme işlemi yapılamadı.");   // silme işlemi olmayacak
            }
            return result;

        }
    }

}
