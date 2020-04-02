using System.Collections.Generic;
using System.Threading.Tasks;
using HockeyApp.API.Models;

namespace HockeyApp.API.Data
{
    public interface IRinkRepository
    {
        // Adding a User and Adding a Photo.  Specify the type as a class
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         // When we save changes to DB, we save either 0 or more than 0 changes to the Database
         Task<bool> SaveAll();
         Task<IEnumerable<User>> GetUsers();
         Task<User> GetUser(int id);
    }
}