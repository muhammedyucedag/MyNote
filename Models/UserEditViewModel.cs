using System.ComponentModel.DataAnnotations;

namespace MyNoteSampleApp.Models
{
    public class UserEditViewModel
    {

        [StringLength(60, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir!"),
           Display(Name = "Ad Soyad")]
        public string Fullname { get; set; }


        [Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(30, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir!"),
            Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }



        [Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(60, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir!"),
            EmailAddress(ErrorMessage = "{0} alanı geçerli bir e-posta adresi giriniz!"),
            Display(Name = "E-Posta Adresi")]
        public string Email { get; set; }


        [Display(Name = "Aktif")]
        public bool IsActive { get; set; }

        [Display(Name = "Yönetici")]
        public bool IsAdmin { get; set; }

    }
}
