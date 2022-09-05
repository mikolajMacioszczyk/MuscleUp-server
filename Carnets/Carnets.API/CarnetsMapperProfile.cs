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

            // Permission
            CreateMap<AllowedEntriesPermission, AllowedEntriesPermissionDto>()
                .ForMember(a => a.CooldownType, 
                opt => opt.MapFrom(src => src.CooldownType.ToString()));
            CreateMap<CreateAllowedEntriesPermissionDto, AllowedEntriesPermission>();

            CreateMap<ClassPermission, ClassPermissionDto>();
            CreateMap<CreateClassPermissionDto, ClassPermission>();

            CreateMap<TimePermissionEntry, TimePermissionEntryDto>();
            CreateMap<CreateTimePermissionEntryDto, TimePermissionEntry>();

            // AssignedPermission
            CreateMap<GrantRevokePermissionDto, AssignedPermission>();
            CreateMap<AssignedPermission, AssignedPermissionDto>();
        }
    }
}
