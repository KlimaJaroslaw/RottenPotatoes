using RottenPotatoes.Models;

namespace RottenPotatoes.DTO
{
    public class ReviewDTO
    {
        public int Review_ID { get; set; }
        public int User_ID { get; set; }
        public int Movie_ID { get; set; }
        public int Rating { get; set; }
        public string? Description { get; set; }

        public ReviewDTO() { }
        public ReviewDTO(Review r)
        {
            this.Description = r.Description;
            this.Rating = r.Rating;
            this.Review_ID = r.Review_ID;
            this.User_ID = r.User_ID;
            this.Movie_ID = r.Movie_ID;
        }

        public static Review GetReviewObject(ReviewDTO dto)
        {
            Review r = new Review()
            {
                Review_ID = dto.Review_ID,
                User_ID = dto.User_ID,
                Movie_ID = dto.Movie_ID,
                Rating = dto.Rating,
                Description = dto.Description,
            };
            return r;
        }
    }
}
