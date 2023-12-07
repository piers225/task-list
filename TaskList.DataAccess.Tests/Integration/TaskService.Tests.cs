using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using TaskList.DataAccess.DataContext;
using TaskList.DataAccess.Service;
using Microsoft.EntityFrameworkCore;
using AutoFixture;

namespace TaskList.DataAccess.Integration;

[TestFixture]
public class TaskServiceTests 
{
    private readonly Fixture fixture;
    private readonly CheckListDataContext dbContext;
    private readonly ITaskListService taskListService; 

    public TaskServiceTests()
    {
        var serviceCollection = new ServiceCollection();

        fixture = new Fixture();

        ServiceCollectionRegistration.SetupWithoutContext(serviceCollection);

        var options = new DbContextOptionsBuilder<CheckListDataContext>()
            .UseInMemoryDatabase(databaseName: "InMemoryDatabaseName")
            .Options;

        dbContext = new CheckListDataContext(options);

        serviceCollection.AddScoped<CheckListDataContext>(_ => dbContext);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        taskListService = serviceProvider.GetRequiredService<ITaskListService>();

        dbContext.Users.Add(this.fixture.Build<User>().With(w => w.Id, 1).Create());

        dbContext.SaveChanges();
    }

    [Test]
    public async Task Test_We_Can_Find_Task_For_User_After_The_Task_Was_Added()
    {
        var taskItem = fixture.Create<TaskItem>();

        dbContext.TaskItems.Add(taskItem);
        dbContext.SaveChanges();
        
        var taskItems = await taskListService.GetTaskItems(taskItem.User.Id);

        var savedTaskItem = taskItems.Single();

        Assert.AreEqual(savedTaskItem.Name, taskItem.Name);
        Assert.AreEqual(savedTaskItem.Status, taskItem.Status.ToString());
    }

    [Test]
    public async Task Test_We_Cant_Find_Tasks_For_Another_User_After_They_Where_Added()
    {
        var taskItem = fixture.Create<TaskItem>();

        dbContext.TaskItems.Add(taskItem);
        dbContext.SaveChanges();
        
        var result = await taskListService.GetTaskItems(this.fixture.Create<int>());

        CollectionAssert.IsEmpty(result);
    }

    [Test]
    public async Task Test_Adding_A_New_Pending_Tasks_Saves_A_Single_Item_In_The_Context()
    {
        var taskItemName = fixture.Create<string>();

        await taskListService.AddPendingTaskItem(1, taskItemName);

        var savedTaskItem = dbContext.TaskItems.Single();

        Assert.AreEqual(savedTaskItem.Name, taskItemName);
        Assert.AreEqual(savedTaskItem.Status, TaskItemStatus.Pending);
        Assert.AreEqual(savedTaskItem.UserId, 1);
    }

    [Test]
    public async Task Test_Updating_Completed_Task_Item_To_Pending_Saves_Item_As_Pending()
    {
        var taskItem = fixture.Create<TaskItem>();
        taskItem.Status = TaskItemStatus.Completed;

        dbContext.TaskItems.Add(taskItem);
        dbContext.SaveChanges();
        
        await taskListService.SetItemStatusToPending(taskItem.UserId, taskItem.Id);

        Assert.AreEqual(taskItem.Status, TaskItemStatus.Pending);
    }

    [Test]
    public async Task Test_Updating_Pending_Task_Item_To_Completed_Saves_Item_As_Compelted()
    {
        var taskItem = fixture.Create<TaskItem>();
        taskItem.Status = TaskItemStatus.Pending;

        dbContext.TaskItems.Add(taskItem);
        dbContext.SaveChanges();
        
        await taskListService.SetItemStatusToComplete(taskItem.UserId, taskItem.Id);

        Assert.AreEqual(taskItem.Status, TaskItemStatus.Completed);
    }
} 