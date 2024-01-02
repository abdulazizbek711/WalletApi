using WalletApi.Models;

namespace WalletApi.Interface;

public interface IUserRepository
{
    ICollection<User> GetUsers();
    User GetUser(int UserId);
    bool UserExists(int UserId);
    bool CreateUser(User user);
    bool UpdateUser(User user);
    bool DeleteUser(User user);
}