using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RottenPotatoes.Models
{
    public class Permission
    {
        [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Permission_ID { get; set; }
        public string Description { get; set; } //Unique (PotatoContex.cs)
    }
}
