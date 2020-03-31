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
    }
}