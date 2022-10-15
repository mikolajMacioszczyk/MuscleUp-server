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
            CreateMap<UpdateGympassTypeWithPermissionsDto, GympassType>();

            // Permission
            CreateMap<PermissionBase, PermissionBaseDto>();

            CreateMap<ClassPermission, ClassPermissionDto>();
            CreateMap<CreateClassPermissionDto, ClassPermission>();

            CreateMap<PerkPermission, PerkPermissionDto>();
            CreateMap<CreatePerkPermissionDto, PerkPermission>();

            // AssignedPermission
            CreateMap<GrantRevokePermissionDto, AssignedPermission>();
            CreateMap<AssignedPermission, AssignedPermissionDto>();

            // Gympass
            CreateMap<Gympass, GympassDto>()
               .ForMember(a => a.GympassTypeName,
                opt => opt.MapFrom(src => src.GympassType.GympassTypeName))
               .ForMember(a => a.GympassTypeId,
                opt => opt.MapFrom(src => src.GympassType.GympassTypeId))
               .ForMember(a => a.ValidationType,
                opt => opt.MapFrom(src => src.GympassType.ValidationType));

            // Subscription
            CreateMap<CreateGympassSubscriptionDto, Subscription>();
            CreateMap<Subscription, SubscriptionDto>()
                .ForMember(s => s.GympassId,
                opt => opt.MapFrom(src => src.Gympass.GympassId))
                .ForMember(s => s.GympassStatus,
                opt => opt.MapFrom(src => src.Gympass.Status));
        }
    }
}
