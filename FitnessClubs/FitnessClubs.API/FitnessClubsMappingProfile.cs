using AutoMapper;
using Common.Models.Dtos;
using FitnessClubs.Domain.Models;
using FitnessClubs.Domain.Models.Dtos;

namespace FitnessClubs.API
{
    public class FitnessClubsMappingProfile : Profile
    {
        public FitnessClubsMappingProfile()
        {
            CreateMap<FitnessClub, FitnessClubDto>();
            CreateMap<CreateFitnessClubDto, FitnessClub>();

            CreateMap<WorkerEmployment, WorkerEmploymentDto>();
            CreateMap<CreateWorkerEmploymentDto, WorkerEmployment>();
        }
    }
}
