using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyNoteSampleApp.Models.Entities
{
    public class EntityLogBase
    {

        [Required, StringLength(60),Display(Name = "Oluşuturan Kullanıcı")]
        public string CreatedUser { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [StringLength(60), Display(Name = "Güncelleyen Kullanıcı")]
        public string? ModifiedUser { get; set; }

        [Display(Name = "Güncelleme Tarihi")]
        public DateTime? ModifiedAt { get; set; }
    }
}
