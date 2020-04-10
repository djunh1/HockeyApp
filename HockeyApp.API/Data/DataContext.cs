using HockeyApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HockeyApp.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}
        //Name of table when scaffold database
        public DbSet<Value> Values {get; set;}        
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }

        // FOLLOWER 3/9 - Configuring the relationships (many to many)
        public DbSet<Follow> Follows { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder){
            builder.Entity<Follow>()
                .HasKey(k => new {k.FollowerId, k.FollowedId});
                // For Primary Key

            builder.Entity<Follow>()
                .HasOne(u => u.Followed)
                .WithMany(u => u.Followers)
                .HasForeignKey(u => u.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follow>()
                .HasOne(u => u.Follower)
                .WithMany(u => u.Followeds)
                .HasForeignKey(u => u.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

             builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);   
        }

            
    }
}