namespace TaskList.DataAccess.DataContext.Seeder;
internal class Seeder : ISeeder
{
    private readonly CheckListDataContext context;

    public Seeder(CheckListDataContext context) {
        this.context = context;
    }

    public void Seed()
    {
        var user = new User() {
             Id = 1,
             Name = "test",
             Email = "test",
             Password = "test"
        };

        context.Users.Add(user);
        context.SaveChanges();
    }
}