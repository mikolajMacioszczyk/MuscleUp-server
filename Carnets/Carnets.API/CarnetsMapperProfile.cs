using AutoMapper;
using Carnets.Domain.Models;
using Carnets.Domain.Models.Dtos;

namespace Carnets.API
{
    public class CarnetsMapperProfile : Profile
    {
        public CarnetsMapperProfile()
        {
            CreateMap<GympassType, GympassTypeDto>();
            CreateMap<CreateGympassTypeDto, GympassType>();
        }
    }
}
