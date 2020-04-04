using System;

namespace HockeyApp.API.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }

        // Cascade Delete
        public User User { get; set; }
        public int UserId { get; set; }

        // Step 3 - CLOUD STORAGE - we need an id column in our photo table (Next perform migration, Next PhotosController)
        public string PublicID { get; set; }
    }
}