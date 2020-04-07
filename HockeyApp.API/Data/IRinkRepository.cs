using System.Collections.Generic;
using System.Threading.Tasks;
using HockeyApp.API.helpers;
using HockeyApp.API.Models;

namespace HockeyApp.API.Data
{
    public interface IRinkRepository
    {
        // Adding a User and Adding a Photo.  Specify the type as a class
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         // When we save changes to DB, we save either 0 or > 0 changes to the Database
         Task<bool> SaveAll();
         Task<PagedList<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id);
         // Step 7 CLOUD STORAGE - create signature (next implement method in the rink repo)
         Task<Photo> GetPhoto(int id);
         Task<Photo> GetMainPhotoForUser(int userId);

        // FOLLOW 4/9
         Task<Follow> GetFollow(int UserId, int recipientId);
    }
}