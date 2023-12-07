using TaskList.DataAccess.Repository;
using TaskList.DataAccess.DataContext;
using TaskList.DataAccess.DataContext.Seeder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TaskList.DataAccess.Service;

namespace TaskList.DataAccess;

public static class ServiceCollectionRegistration { 

    public static void Setup(IServiceCollection serviceCollection) 
    {
        serviceCollection.AddDbContext<CheckListDataContext>(options => options.UseInMemoryDatabase("MyDatabase"));
        ServiceCollectionRegistration.SetupWithoutContext(serviceCollection);
    }

    internal static void SetupWithoutContext(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<ITaskListService, TaskListService>();
        serviceCollection.AddScoped<ISeeder, Seeder>();
    }
}