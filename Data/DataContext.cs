using Microsoft.EntityFrameworkCore;
using WalletApi.Models;

namespace WalletApi.Data;

public class DataContext: DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
}