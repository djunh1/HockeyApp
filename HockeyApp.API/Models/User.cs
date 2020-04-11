using System;
using System.Collections.Generic;

namespace HockeyApp.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        // Stripe
        public string StripeId { get; set; }
        public string MerchantId { get; set; }

        // Twilio 
        public string PhoneNumber { get; set; }
        public string PhonePin { get; set; }
        public bool? PhoneVerified { get; set; }

        // Only for verified rink
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public float? Latitude { get; set; }
        public float? Longitude { get; set; }
        public string Description { get; set; }
        public string RinkInquiries { get; set; }
        public string RinkAmenities{ get; set; }

        // All Users
        public string KnownAs { get; set; }
        public string PlayerPosition { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }

        // FOLLOWERS 2/9

        public virtual ICollection<Follow> Followers { get; set; } // Likers
        public virtual ICollection<Follow> Followeds { get; set; } // Likees


        // Dates
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        // Messages
        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesReceived { get; set; }

        
    }
}