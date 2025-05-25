using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Library.Shared;
using Library.Shared.Model;

namespace PawsistantAPI.Repository.config
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> Users { get; set; }
    }

}
