using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using TaskList.DataAccess.DataContext;
using TaskList.DataAccess.Service;
using Microsoft.EntityFrameworkCore;
using AutoFixture;

namespace TaskList.DataAccess.Integration;

[TestFixture]
public class UserServiceTests 
{
    private readonly IFixture fixture;
    private readonly CheckListDataContext _dbContext;
    private readonly IUserService userService; 

    public UserServiceTests()
    {
        var serviceCollection = new ServiceCollection();

        ServiceCollectionRegistration.SetupWithoutContext(serviceCollection);

        var options = new DbContextOptionsBuilder<CheckListDataContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabaseName")
            .Options;

        _dbContext = new CheckListDataContext(options);

        serviceCollection.AddScoped<CheckListDataContext>(_ => _dbContext);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        userService = serviceProvider.GetRequiredService<IUserService>();

        fixture = new Fixture();
    }

    [Test]
    public async Task Test_We_Can_Find_User_In_Db_Context_After_Adding_Them()
    {
        var user = fixture.Create<User>();

        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        
        var result = await userService.CheckHasUser(user.Email, user.Password);

        Assert.IsTrue(result);
    }
} 