using Microsoft.EntityFrameworkCore;
using BE_ASP.Models;

namespace BE_ASP.Data
{
  public class AppDBContext : DbContext
  {
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options){ }

    public DbSet<User> Users { get; set; }

    public DbSet<UserTask> Tasks { get; set; }
  }
}