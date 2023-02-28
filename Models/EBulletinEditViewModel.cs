using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyNoteSampleApp.Models
{
    public class EBulletinEditViewModel
    {

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(50, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir!"),
            EmailAddress(ErrorMessage = "{0} geçerli bir e-posta adresi olmalıdır"),
            Display(Name = "E-Posta Adresi")]
        public string Email { get; set; }

        [Display(Name = "Yasaklı")]
        public bool Banned { get; set; }

    }
}
