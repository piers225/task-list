using TaskList.DataAccess.Repository;
using TaskList.DataAccess.DataContext;
using TaskList.DataAccess.Models;

namespace TaskList.DataAccess.Service;

internal class TaskListService : ITaskListService {

    private readonly IRepository<TaskItem> _taskItemRepository;
    private readonly IRepository<User> _userRepository;

    public TaskListService(IRepository<TaskItem> taskItemRepository, IRepository<User> userRepository) 
    {
        this._taskItemRepository = taskItemRepository;
        this._userRepository = userRepository;
    }

    public async Task<TaskItemDDL[]> GetTaskItems(int userId) 
    {
        var tasks = await this._taskItemRepository.Where(taskItem => taskItem.User.Id == userId);

        return tasks.Select(s => new TaskItemDDL{
            Id = s.Id,
            Name = s.Name,
            Status = s.Status.ToString()
        }).ToArray();
    }

    public async Task AddPendingTaskItem(int userId, string taskDescription) {
        var user = await this._userRepository.FindOne(f => f.Id == userId);
        this._taskItemRepository.Add(new TaskItem() {
            User = user,
            Name = taskDescription,
            Status= TaskItemStatus.Pending
        });
        await this._taskItemRepository.SaveChanges();
    }

    public async Task SetItemStatusToPending(int userId, int taskItemId) 
    {
        var taskItem =  await this._taskItemRepository.FindOne(taskItem => taskItem.User.Id == userId && taskItem.Id == taskItemId);
        taskItem.Status = TaskItemStatus.Pending;
        await this._taskItemRepository.SaveChanges();
    }

    public async Task SetItemStatusToComplete(int userId, int taskItemId) 
    {
        var taskItem =  await this._taskItemRepository.FindOne(taskItem => taskItem.User.Id == userId && taskItem.Id == taskItemId);
        taskItem.Status = TaskItemStatus.Completed;
        await this._taskItemRepository.SaveChanges();
    }

} 