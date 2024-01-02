namespace WalletApi.Dto;

public class UserDto
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public decimal WalletBalance { get; set; } 
}