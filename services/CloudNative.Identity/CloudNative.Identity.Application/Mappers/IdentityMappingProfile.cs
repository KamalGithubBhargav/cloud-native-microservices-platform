using AutoMapper;
using CloudNative.Identity.Application.DTOs.Auth;
using CloudNative.Identity.Core.Entities;

namespace CloudNative.Identity.Application.Mappers
{
    public class IdentityMappingProfile : Profile
    {
        public IdentityMappingProfile() {
            CreateMap<UserDto, UserEntity>().ReverseMap();
        }
    }
}
