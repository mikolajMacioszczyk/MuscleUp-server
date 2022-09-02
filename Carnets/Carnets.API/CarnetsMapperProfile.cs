using AutoMapper;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;

namespace Carnets.API
{
    public class CarnetsMapperProfile : Profile
    {
        public CarnetsMapperProfile()
        {
            // GympassType
            CreateMap<GympassType, GympassTypeDto>();
            CreateMap<CreateGympassTypeDto, GympassType>();
            CreateMap<UpdateGympassTypeDto, GympassType>();

            // AllowedEntriesPermission
            CreateMap<AllowedEntriesPermission, AllowedEntriesPermissionDto>()
                .ForMember(a => a.CooldownType, 
                opt => opt.MapFrom(src => src.CooldownType.ToString()));
        }
    }
}
