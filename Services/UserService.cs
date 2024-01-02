using System.Security.Cryptography;
using System.Text;
using WalletApi.Interface;
using WalletApi.Models;

namespace WalletApi.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }
    public string GenerateXAuthToken(int userId, string apiKey)
    {
        using (SHA256 sha256Hash = SHA256.Create())
        {
            var storedApiKey = _configuration.GetValue<string>("Authentication:ApiKey");
            string combinedKey = $"{userId}-{storedApiKey}";
            byte[] hashBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedKey));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(hashBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}