using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RottenPotatoes.Models;

namespace RottenPotatoes.Models
{
    public class Watchlist
    {
        [Key]
        public int Watchlist_ID { get; set; }

        [ForeignKey("User")]
        public int User_ID { get; set; }

        [ForeignKey("Movie")]
        public int Movie_ID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Production Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Added_Date { get; set; }

        [Display(Name = "Priority")]
        public int Priority { get; set; }

        public Movie? Movie { get; set; }

        public User? User { get; set; }
    }
}
