using System;
using Microsoft.AspNetCore.Http;

namespace HockeyApp.API.helpers
{
    // Don't need to create a new instance of this class, use static
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message){
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateAge(this DateTime theDateTime){
            var age = DateTime.Today.Year - theDateTime.Year;
            if (theDateTime.AddYears(age) > DateTime.Today) {
                age--;
            }
            return age;
        }
        
    }
}