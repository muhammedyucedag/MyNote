using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNoteSampleApp.Models.Entities
{
    [Table("Comments")]
    public class Comment : EntityLogBase
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(250), Display(Name = "Yorum")]
        public string Text { get; set; }

        [Display(Name = "Yorum Yapan")]
        public int? UserId { get; set; } // yorumun kimin yazdığı 

        [Display(Name = "Not")]
        public int? NoteId { get; set; } // ve ne yazdığı

        public virtual User User { get; set; }
        public virtual Note Note { get; set; }
    }
}
