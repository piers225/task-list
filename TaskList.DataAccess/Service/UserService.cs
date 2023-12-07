
using TaskList.DataAccess.Repository;
using TaskList.DataAccess.DataContext;

namespace TaskList.DataAccess.Service;

internal class UserService : IUserService {

    private readonly IRepository<User> userRepository;

    public UserService(IRepository<User> userRepository) 
    {
        this.userRepository = userRepository;
    }

    public Task<bool> CheckHasUser(string email, string password)
    {
        return this.userRepository.Any(user => user.Email == email && user.Password == password);   
    }
}