using Microsoft.EntityFrameworkCore;
using MockLogic.Models;

namespace MockLogic
{
    public class MockServiceDbContext : DbContext
    {
        public DbSet<Subdomain> Subdomains { get; set; }
        public DbSet<Mock> Mocks { get; set; }

        public MockServiceDbContext(DbContextOptions<MockServiceDbContext> options) : base(options) { }
    }
}
