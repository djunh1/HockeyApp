using System.Collections.Generic;
using System.Threading.Tasks;
using HockeyApp.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using HockeyApp.API.helpers;
using System;

namespace HockeyApp.API.Data
{
    public class RinkRepository : IRinkRepository
    {
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

        public async Task<Follow> GetFollow(int userId, int recipientId)
        {
            // FOLLOW 5/x

            return await _context.Follows
                .FirstOrDefaultAsync(u => u.FollowerId == userId && u.FollowedId == recipientId);
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

        public async Task<PagedList<User>> GetUsers(UserParams userParams)

        {
            // Add the AsQueryable so can use the Where caluse
            var users = _context.Users.Include(p => p.Photos).OrderByDescending(u => u.LastActive).AsQueryable();

            // Modify what we return
            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.PlayerPosition == userParams.PlayerPosition);

            // FOLLOWERS 8/9
            if (userParams.Followers){
                var userFollowers = await GetUserFollows(userParams.UserId, userParams.Followers);
                users = users.Where(u => userFollowers.Contains(u.Id)); // Filter out multiple Ids here.
            }

            if (userParams.Followeds){
                var userFolloweds = await GetUserFollows(userParams.UserId, userParams.Followers);
                users = users.Where(u => userFolloweds.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy)){
                switch (userParams.OrderBy){
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        // FOLLOWERS 9/9 - returning integers, use the Select call for that.
        private async Task<IEnumerable<int>> GetUserFollows(int id, bool followers){
            var user = await _context.Users
                .Include(x => x.Followers)
                .Include(x => x.Followeds)
                .FirstOrDefaultAsync(u => u.Id == id);

                if (followers) {
                    return user.Followers.Where(u => u.FollowedId == id).Select(i => i.FollowerId);
                } else {
                    return user.Followeds.Where(u => u.FollowerId == id).Select(i => i.FollowedId);
                }
        }

        public async Task<bool> SaveAll()
        {
            // returns true if something is saved, returns false if nothing is saved
            return await _context.SaveChangesAsync() > 0;
        }

        // Messages
        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams)
        {
           var messages = _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
                                           .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                                           .AsQueryable();

           switch(messageParams.MessageContainer){
               case "Inbox":
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                    break;
               case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId  && u.SenderDeleted == false);
                    break;
               default:
                    messages = messages.Where(u => u.RecipientId == messageParams.UserId 
                                              && u.RecipientDeleted == false  && u.IsRead == false );
                    break;
           }

           messages = messages.OrderByDescending(d => d.MessageSent);
           return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            var messages = await _context.Messages
                                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                                .Include(u => u.Recipient).ThenInclude(p => p.Photos)
                                .Where(m => m.RecipientId == userId && m.RecipientDeleted == false 
                                    && m.SenderId == recipientId 
                                    || m.RecipientId == recipientId && m.SenderDeleted == false && m.SenderId == userId)
                                .OrderByDescending(m => m.MessageSent).ToListAsync();

            return messages;
        }
    }
}