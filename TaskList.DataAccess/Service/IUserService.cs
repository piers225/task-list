
namespace TaskList.DataAccess.Service;

public interface IUserService 
{
    Task<bool> CheckHasUser(string email, string password);

}