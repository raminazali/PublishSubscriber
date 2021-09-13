using AutoMapper;
using HasebCoreApi.DTO;
using HasebCoreApi.DTO.DB;
using HasebCoreApi.Models;
public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<UserInfo, UserAuthDTO>();
        CreateMap<UserInfo, UserOTPDTO>();
        CreateMap<UserInfo, UserInfoDTO>();
    }
}