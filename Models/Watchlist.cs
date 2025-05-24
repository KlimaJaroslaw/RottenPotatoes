using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Watchlist
{
    public int User_ID { get; set; }

    public int Movie_ID { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Production Date")]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime Added_Date { get; set; }

    [Display(Name = "Priority")]
    public int Priority { get; set; }

    [ForeignKey("Movie_ID")]
    public Movie? movie { get; set; }
}