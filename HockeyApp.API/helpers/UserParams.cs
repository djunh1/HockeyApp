namespace HockeyApp.API.helpers
{
    public class UserParams
    {
        private const int MAXPAGESIZE = 50;
        public int PageNumber { get; set; } = 1 ;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize ;}
            set { pageSize = (value) > MAXPAGESIZE ? MAXPAGESIZE : value; }
        }

        // For filtering
        public int UserId { get; set; }
        public string PlayerPosition { get; set; }

        public int MinTime { get; set; } = 0; // default time one year
        public int MaxTime { get; set; } = 6; 
        public string OrderBy { get; set; }

        // FOLLOWERS 7/9

        public bool Followeds { get; set; } = false; // Likee
        public bool Followers { get; set; } = false; // Liker
        
    }
}