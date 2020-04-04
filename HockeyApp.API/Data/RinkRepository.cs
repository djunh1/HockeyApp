using System.Collections.Generic;
using System.Threading.Tasks;
using HockeyApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HockeyApp.API.Data
{
    public class RinkRepository : IRinkRepository
    {
        // Need access to the context, 
        private readonly DataContext _context;
        public RinkRepository(DataContext context)
        {
            _context = context;
         
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        // STEP 8 - CLOUD STORAGE Implement interface method (Next, need a new photo dto)
        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<User> GetUser(int id)
        {
            // Need to get the photos for each user.
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            // returns true if something is saved, returns false if nothing is saved
            return await _context.SaveChangesAsync() > 0;
        }
    }
}