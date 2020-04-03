namespace HockeyApp.API.dtos
{
    public class UserForUpdateDto
    {
        //STEP 1 - updating on API (next autoMapper)
        
        public string Description { get; set; }
        public string RinkAmenities{ get; set; }
        public string RinkInquiries { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}