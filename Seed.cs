using WalletApi.Data;
using WalletApi.Models;

namespace WalletApi;

public class Seed
{
    private readonly DataContext _context;
    public Seed(DataContext context)
    {
        _context = context;
    }

    public void SeedDataContext()
    {
        if (!_context.Users.Any())
        {
            var users = new List<User>
            {
                new User { UserName = "User1", Email = "user1@example.com" },
                new User { UserName = "User2", Email = "user2@example.com" },
            };
            _context.Users.AddRange(users);
        }
        _context.SaveChanges();
    }
}