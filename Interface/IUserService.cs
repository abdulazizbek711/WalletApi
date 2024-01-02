namespace WalletApi.Interface;

public interface IUserService
{
    public string GenerateXAuthToken(int userId, string apiKey);
}