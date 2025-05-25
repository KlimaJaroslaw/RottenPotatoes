using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RottenPotatoes.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Review_ID { get; set; }
        [ForeignKey(nameof(User))]
        public int User_ID { get; set; }
        [ForeignKey(nameof(Movie))]
        public int Movie_ID { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
        public User? User { get; set; }
        public Movie? Movie { get; set; }
    }
}
