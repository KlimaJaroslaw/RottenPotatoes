using System.ComponentModel.DataAnnotations;
using RottenPotatoes.Models;

public class Movie
{
    [Key]
    public int Movie_ID { get; set; }

    [Display(Name = "Title")]
    public string Title { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Production Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Production_Date { get; set; }

    [Display(Name = "Director")]
    public string Director { get; set; }

    [Display(Name = "Producer")]
    public string Producer { get; set; }

    [Display(Name = "ScreenWriter")]
    public string ScreenWriter { get; set; }

    [Display(Name = "Synopsis")]
    public string Synopsis { get; set; }

    public ICollection<Watchlist>? Watchlists { get; set; }
    public ICollection<Review>? Reviews { get; set; }
}