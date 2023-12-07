using Microsoft.EntityFrameworkCore;

namespace TaskList.DataAccess.DataContext;

internal class CheckListDataContext : DbContext
{
    public CheckListDataContext(DbContextOptions<CheckListDataContext> options)
        : base(options)
    { }
    public DbSet<User> Users { get; set; }

    public DbSet<TaskItem> TaskItems { get; set; }
}