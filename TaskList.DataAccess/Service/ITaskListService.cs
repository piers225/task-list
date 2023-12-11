using TaskList.DataAccess.Models;

namespace TaskList.DataAccess.Service;

public interface ITaskListService 
{
    Task<TaskItemDDL[]> GetTaskItems(int userId);

    Task AddTaskItem(int userId, TaskItemDDL taskItemDDL);

    Task UpdateTaskItem(int userId, int taskId, TaskItemDDL taskItemDDL);

}