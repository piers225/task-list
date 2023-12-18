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

    public async Task AddTaskItem(int userId, TaskItemDDL taskItemDDL)
    {
        var user = await this._userRepository.FindOne(userId);
        this._taskItemRepository.Add(new TaskItem() {
            User = user,
            Name = taskItemDDL.Name,
            Status= TaskItemStatus.Pending
        });
        await this._taskItemRepository.SaveChanges();
    }

    public async Task UpdateTaskItem(int userId, int taskId, TaskItemDDL taskItemDDL)
    {
        var taskItem = await this._taskItemRepository.FindOne(taskId);
        taskItem.Status = Enum.Parse<TaskItemStatus>(taskItemDDL.Status);
        await this._taskItemRepository.SaveChanges();
    }
} 