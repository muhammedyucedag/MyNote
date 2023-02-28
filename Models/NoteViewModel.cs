using System.ComponentModel.DataAnnotations;

namespace MyNoteSampleApp.Models
{
    public class NoteViewModel
    {

        [Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(60, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir!"),
            Display(Name = "Başlık")]
        public string Title { get; set; }


        [Required(ErrorMessage = "{0} alanı boş geçilemez!"),
            StringLength(250, ErrorMessage = "{0} alanı maksimum {1} karakter olabilir!"),
            Display(Name = "Özet")]
        public string Summary { get; set; }


        [Display(Name = "Detay")]
        public string Detail { get; set; }

        [Display(Name = "Taslak")]
        public bool IsDraft { get; set; }

        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }
    }
}
