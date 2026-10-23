using Microsoft.EntityFrameworkCore;
using TeamDesk.Api.Models;

namespace TeamDesk.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<WorkRequest> WorkRequests { get; set; }
}
