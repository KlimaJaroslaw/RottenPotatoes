using RottenPotatoes.Models;

namespace RottenPotatoes.DTO
{
    public class MovieDTO
    {
        public int Movie_ID { get; set; }
        public string Title { get; set; }
        public DateTime Production_Date { get; set; }
        public string Director { get; set; }
        public string Producer { get; set; }
        public string ScreenWriter { get; set; }
        public string Synopsis { get; set; }

        public MovieDTO() { }
        public MovieDTO(RottenPotatoes.Models.Movie m)
        {
            this.Movie_ID = m.Movie_ID;
            this.Title = m.Title;
            this.Producer = m.Producer;
            this.ScreenWriter = m.ScreenWriter;
            this.Production_Date = m.Production_Date;
            this.Director = m.Director;
            this.Synopsis = m.Synopsis;
        }

        public static Movie GetMovieObject(MovieDTO dto)
        {
            Movie m = new Movie()
            {
                Movie_ID = dto.Movie_ID,
                Title = dto.Title,
                Producer = dto.Producer,
                ScreenWriter = dto.ScreenWriter,
                Production_Date = dto.Production_Date,
                Director = dto.Director,
                Synopsis = dto.Synopsis,
            };
            return m;
        }
    }
}
