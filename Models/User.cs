using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RottenPotatoes.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int User_ID { get; set; }        
        public string Login_Hash { get; set; } //Unique (PotatoContex.cs)
        public string Password_Hash { get; set; }
        public string? Email_Hash { get; set; }

        [ForeignKey(nameof(Permission))]
        public int Permission_ID { get; set; }
        public Permission? Permission { get; set; }
    }
}
