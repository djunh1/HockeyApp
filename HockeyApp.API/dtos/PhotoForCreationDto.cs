using System;
using Microsoft.AspNetCore.Http;

namespace HockeyApp.API.dtos
{
    // Step 5 CLOUD STORAGE  (next controller)
    public class PhotoForCreationDto
    {
        public string Url { get; set; }

        public IFormFile File { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        // We retrieve this from Cloudinary
        public string PublicId { get; set; }

        public PhotoForCreationDto()
        {
            DateAdded = DateTime.Now;
            
        }
    }
}