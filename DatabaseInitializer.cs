using MyNoteSampleApp.Models.Context;
using MyNoteSampleApp.Models.Entities;

namespace EFCoreWithMvcCore
{
    public class DatabaseInitializer
    {

        private DatabaseContext _context;
        private AppSettings _settings;

        public DatabaseInitializer(DatabaseContext context, AppSettings settings)
        {
            _context = context;
            _settings = settings;
        }

        public void Seed()
        {
            // insert işlemi admin yoksa admin yükleme işlemi

            if (_context.Users.Any(x => x.Username == _settings.AdminUsername) == false)
            {
                _context.Users.Add(new User
                {
                    Username = _settings.AdminUsername,
                    Password = _settings.AdminPassword,
                    FullName = _settings.AdminFullName,
                    EMail = _settings.AdminEmail,
                    IsActive = true,
                    IsAdmin = true,
                    CreatedUser = "default",
                    CreatedAt = DateTime.Now,
                    ModifiedUser = "default",
                    ModifiedAt = DateTime.Now
                });
                _context.SaveChanges();
            }
        }
    }
}