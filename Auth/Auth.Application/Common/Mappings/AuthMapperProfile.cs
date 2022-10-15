using Auth.Application.Common.Dtos;
using Auth.Application.Members.Dtos;
using Auth.Application.Trainer.Dtos;
using Auth.Application.Workers.Dtos;
using Auth.Domain.Models;
using AutoMapper;

namespace Auth.API
{
    public class AuthMapperProfile : Profile
    {
        public AuthMapperProfile()
        {
            CreateMap<UpdateUserDto, ApplicationUser>();

            CreateMap<Member, MemberDto>()
                .ForMember(m => m.FirstName, c => c.MapFrom(src => src.User.FirstName))
                .ForMember(m => m.LastName, c => c.MapFrom(src => src.User.LastName))
                .ForMember(m => m.Email, c => c.MapFrom(src => src.User.Email))
                .ForMember(m => m.BirthDate, c => c.MapFrom(src => src.User.BirthDate))
                .ForMember(m => m.RegisteredDate, c => c.MapFrom(src => src.User.RegisterDate))
                .ForMember(m => m.Gender, c => c.MapFrom(src => src.User.Gender));
            CreateMap<UpdateMemberDto, Member>();

            CreateMap<Trainer, TrainerDto>()
                .ForMember(m => m.FirstName, c => c.MapFrom(src => src.User.FirstName))
                .ForMember(m => m.LastName, c => c.MapFrom(src => src.User.LastName))
                .ForMember(m => m.Email, c => c.MapFrom(src => src.User.Email))
                .ForMember(m => m.BirthDate, c => c.MapFrom(src => src.User.BirthDate))
                .ForMember(m => m.RegisteredDate, c => c.MapFrom(src => src.User.RegisterDate))
                .ForMember(m => m.Gender, c => c.MapFrom(src => src.User.Gender));
            CreateMap<UpdateTrainerDto, Trainer>();

            CreateMap<Worker, WorkerDto>()
                .ForMember(m => m.FirstName, c => c.MapFrom(src => src.User.FirstName))
                .ForMember(m => m.LastName, c => c.MapFrom(src => src.User.LastName))
                .ForMember(m => m.Email, c => c.MapFrom(src => src.User.Email))
                .ForMember(m => m.BirthDate, c => c.MapFrom(src => src.User.BirthDate))
                .ForMember(m => m.RegisteredDate, c => c.MapFrom(src => src.User.RegisterDate))
                .ForMember(m => m.Gender, c => c.MapFrom(src => src.User.Gender));
            CreateMap<UpdateWorkerDto, Worker>();
        }
    }
}
