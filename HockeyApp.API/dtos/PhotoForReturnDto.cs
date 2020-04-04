using System;

namespace HockeyApp.API.dtos
{
    public class PhotoForReturnDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        // Step 10 CLOUD STORAGE , create a new id to return
        public string PublicId { get; set; }
    }
}