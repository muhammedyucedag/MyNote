using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyNoteSampleApp.Models
{
    public class CategoryViewModel
    {

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"), 
            StringLength(40, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir!"),
            Display(Name = "Kategori Adı")]
        public string Name { get; set; }

        [StringLength(160, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir!"),
            Display(Name = "Açıklama")]
        public string Description { get; set; }

    }
}
