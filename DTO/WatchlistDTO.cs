using RottenPotatoes.Models;

namespace RottenPotatoes.DTO
{
    public class WatchlistDTO
    {
        public int Watchlist_ID { get; set; }
        public int User_ID { get; set; }
        public int Movie_ID { get; set; }
        public DateTime Added_Date { get; set; }
        public int Priority { get; set; }

        public WatchlistDTO() { }

        public WatchlistDTO(RottenPotatoes.Models.Watchlist w)
        {
            Watchlist_ID = w.Watchlist_ID;
            User_ID = w.User_ID;
            Movie_ID = w.Movie_ID;
            Added_Date = w.Added_Date;
            Priority = w.Priority;
        }

        public static Watchlist GetWatchlistObject(WatchlistDTO dTO)
        {
            Watchlist w = new Watchlist()
            {
                Watchlist_ID = dTO.Watchlist_ID,
                User_ID = dTO.User_ID,
                Movie_ID = dTO.Movie_ID,
                Added_Date = dTO.Added_Date,
                Priority = dTO.Priority
            };
            return w;
        }
    }
}
