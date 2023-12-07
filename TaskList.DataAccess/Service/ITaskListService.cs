using TaskList.DataAccess.Models;

namespace TaskList.DataAccess.Service;

public interface ITaskListService 
{
    Task<TaskItemDDL[]> GetTaskItems(int userId);

    Task AddPendingTaskItem(int userId, string taskDescription);

    Task SetItemStatusToPending(int userId, int taskItemId) ;

    Task SetItemStatusToComplete(int userId, int taskItemId);
}