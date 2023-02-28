using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyNoteSampleApp.Models.Entities
{
    [Table("EmailMembership")]
    public class EmailMembership : EntityLogBase
    {
        public int Id { get; set; }

        [Required, EmailAddress, StringLength(60), Display(Name = "E-Posta Adresi")]
        public string EmailAddress { get; set; }
    }

}
