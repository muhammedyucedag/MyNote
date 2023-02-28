using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNoteSampleApp.Models.Entities
{
    [Table("Notes")]
    public class Note : EntityLogBase
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(60), Display(Name = "Başlık")]
        public string Title { get; set; }

        [Required, StringLength(250), Display(Name = "Özet")]
        public string Summary { get; set; }

        [Display(Name = "Detay")]
        public string Detail { get; set; }

        [Display(Name = "Taslak")]
        public bool IsDraft { get; set; }

        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Display(Name = "Yazar")]
        public int OwnerId { get; set; }

        public virtual User Owner { get; set; } // sahip tektir 
        public virtual Category Category { get; set; } // kategori tektir 
        public virtual List<Comment> Comments { get; set; } // birden çok yorumu 
        public virtual List<LikedNote> Likes { get; set; } // birdek çok beğeneni 
    }
}
