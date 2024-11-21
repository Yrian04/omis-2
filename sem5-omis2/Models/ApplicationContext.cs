using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace sem5_omis2.Models
{
    public class ApplicationContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Event> Events { get; set; } = null!;
        public DbSet<EventGroup> EventGroups { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}