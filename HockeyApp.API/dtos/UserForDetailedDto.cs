using System;
using System.Collections.Generic;
using HockeyApp.API.Models;

namespace HockeyApp.API.dtos
{
    public class UserForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public string Description { get; set; }
        public string RinkInquiries { get; set; }
        public string RinkAmenities{ get; set; }
        public string KnownAs { get; set; }
        public string PlayerPosition { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotosForDetailedDto> Photos { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
    }
}