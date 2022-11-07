using AutoMapper;
using Carnets.Application.AssignedPermissions.Dtos;
using Carnets.Application.Dtos.Permission;
using Carnets.Application.Entries.Dtos;
using Carnets.Application.Gympasses.Dtos;
using Carnets.Application.GympassTypes.Dtos;
using Carnets.Application.SpecificPermissions.Dtos;
using Carnets.Application.Subscriptions.Dtos;
using Carnets.Domain.Models;

namespace Carnets.Application.Mapping
{
    public class CarnetsMapperProfile : Profile
    {
        public CarnetsMapperProfile()
        {
            // GympassType
            CreateMap<GympassType, GympassTypeDto>();
            CreateMap<GympassType, GympassTypeWithPermissions>();
            CreateMap<GympassTypeWithPermissions, GympassTypeDto>();
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

            CreateMap<Gympass, GympassWithSessionDto>()
              .ForMember(a => a.GympassTypeName,
               opt => opt.MapFrom(src => src.GympassType.GympassTypeName))
              .ForMember(a => a.GympassTypeId,
               opt => opt.MapFrom(src => src.GympassType.GympassTypeId));

            // Subscription
            CreateMap<CreateGympassSubscriptionDto, Subscription>();
            CreateMap<Subscription, SubscriptionDto>()
                .ForMember(s => s.GympassId,
                opt => opt.MapFrom(src => src.Gympass.GympassId))
                .ForMember(s => s.GympassStatus,
                opt => opt.MapFrom(src => src.Gympass.Status));

            // Entries
            CreateMap<Entry, CreatedEntryDto>()
                .ForMember(e => e.Gympass,
                opt => opt.MapFrom(src => src.Gympass));
            CreateMap<Entry, EntryDto>()
                .ForMember(e => e.GympassId,
                opt => opt.MapFrom(src => src.Gympass.GympassId));
        }
    }
}
