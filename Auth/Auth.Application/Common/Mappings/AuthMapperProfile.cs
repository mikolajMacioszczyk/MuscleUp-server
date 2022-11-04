using Auth.Application.Common.Dtos;
using Auth.Application.Members.Dtos;
using Auth.Application.Owners.Dtos;
using Auth.Application.Trainer.Dtos;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using AutoMapper;
using Common.Models.Dtos;

namespace Auth.API
{
    public class AuthMapperProfile : Profile
    {
        public AuthMapperProfile()
        {
            CreateMap<UpdateUserDto, ApplicationUser>();

            CreateMap<Member, MemberDto>().AddApplicationUserMapping();
            CreateMap<UpdateMemberDto, Member>();

            CreateMap<Trainer, TrainerDto>().AddApplicationUserMapping();
            CreateMap<UpdateTrainerDto, Trainer>();

            CreateMap<Worker, WorkerDto>().AddApplicationUserMapping();
            CreateMap<UpdateWorkerDto, Worker>();

            CreateMap<Owner, OwnerDto>().AddApplicationUserMapping();
            CreateMap<UpdateOwnerDto, Owner>();
        }
    }

    public static class AuthMappingExtensions
    {
        public static void AddApplicationUserMapping<TUser, TUserDto>(this IMappingExpression<TUser, TUserDto> map)
            where TUser : SpecificUserBase
            where TUserDto : UserDto
        {
            map.ForMember(m => m.FirstName, c => c.MapFrom(src => src.User.FirstName))
            .ForMember(m => m.LastName, c => c.MapFrom(src => src.User.LastName))
            .ForMember(m => m.Email, c => c.MapFrom(src => src.User.Email))
            .ForMember(m => m.BirthDate, c => c.MapFrom(src => src.User.BirthDate))
            .ForMember(m => m.RegisteredDate, c => c.MapFrom(src => src.User.RegisterDate))
            .ForMember(m => m.Gender, c => c.MapFrom(src => src.User.Gender))
            .ForMember(m => m.AvatarUrl, c => c.MapFrom(src => src.User.AvatarUrl));
        }
    }
}
