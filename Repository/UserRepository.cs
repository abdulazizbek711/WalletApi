using WalletApi.Data;
using WalletApi.Interface;
using WalletApi.Models;

namespace WalletApi.Repository;

public class UserRepository: IUserRepository
{
    private readonly DataContext _context;
    public UserRepository(DataContext context)
    {
        _context = context;
    }
    public ICollection<User> GetUsers()
    {
        return _context.Users.OrderBy(u => u.UserId).ToList();
    }

    public User GetUser(int UserId)
    {
        return _context.Users.Where(u => u.UserId==UserId).FirstOrDefault();
    }
    

    public bool UserExists(int UserId)
    {
        return _context.Users.Any(u => u.UserId == UserId);
    }

    public bool CreateUser(User user)
    {
        _context.Add(user);
        return Save();
    }

    public bool UpdateUser(User user)
    {
        _context.Update(user);
        return Save();
    }

    public bool DeleteUser(User user)
    {
        _context.Remove(user);
        return Save();
    }
    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}