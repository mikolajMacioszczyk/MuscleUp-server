using AutoMapper;
using Common.Models.Dtos;
using FitnessClubs.Application.FitnessClubs.Dtos;
using FitnessClubs.Application.Memberships.Dtos;
using FitnessClubs.Application.TrainerEmployments.Dtos;
using FitnessClubs.Application.WorkoutEmployments.Dtos;
using FitnessClubs.Domain.Models;

namespace FitnessClubs.Application.Mappings
{
    public class FitnessClubsMappingProfile : Profile
    {
        public FitnessClubsMappingProfile()
        {
            CreateMap<FitnessClub, FitnessClubDto>();
            CreateMap<CreateFitnessClubDto, FitnessClub>();

            CreateMap<WorkerEmployment, WorkerEmploymentDto>();
            CreateMap<CreateWorkerEmploymentDto, WorkerEmployment>();

            CreateMap<TrainerEmployment, TrainerEmploymentDto>();
            CreateMap<CreateTrainerEmploymentDto, TrainerEmployment>();

            CreateMap<Membership, MembershipDto>();
            CreateMap<CreateMembershipDto, Membership>();
        }
    }
}
