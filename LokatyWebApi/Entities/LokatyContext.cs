using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LokatyWebApi.Entities
{
    public class LokatyContext : IdentityDbContext<ApplicationUser>
    {
        public LokatyContext(DbContextOptions<LokatyContext> options): base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Client> Clients { get; set; }

        public DbSet<Deposit> Deposits { get; set; }

    }
}
