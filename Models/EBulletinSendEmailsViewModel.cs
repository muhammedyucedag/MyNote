using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MyNoteSampleApp.Models
{
    public class EBulletinSendEmailsViewModel
    {
        [Display(Name = "E-Posta Adresleri")]
        public List<string> Emails { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), Display(Name = "Konu")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), Display(Name = "Mesaj")]
        public string Text { get; set; }

        public SelectList EmailAddresses { get; set; }
    }
}
