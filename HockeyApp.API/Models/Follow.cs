namespace HockeyApp.API.Models
{
    public class Follow
    {
        // FOLLOW SYSTEM 1/9
        public int FollowerId { get; set; } //LikerId
        public int FollowedId { get; set; } //LikeeId
        public User Follower { get; set; } // Liker
        public User Followed { get; set; }// Likee
    }
}