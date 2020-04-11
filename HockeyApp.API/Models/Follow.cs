namespace HockeyApp.API.Models
{
    public class Follow
    {
        // FOLLOW SYSTEM 1/9
        public int FollowerId { get; set; } //LikerId
        public int FollowedId { get; set; } //LikeeId
        public virtual User Follower { get; set; } // Liker
        public virtual User Followed { get; set; }// Likee
    }
}