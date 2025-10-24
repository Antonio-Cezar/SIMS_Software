using Microsoft.EntityFrameworkCore;
using SmartInventory.Api.Models;

namespace SmartInventory.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Item> Items => Set<Item>();
}
