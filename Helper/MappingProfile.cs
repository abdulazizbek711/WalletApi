using AutoMapper;
using WalletApi.Dto;
using WalletApi.Models;

namespace WalletApi.Helper;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}