using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RottenPotatoes.Models
{
    public class Permission
    {
        [Key]
        public int Permission_ID { get; set; }
        public string Description { get; set; }
    }
}
