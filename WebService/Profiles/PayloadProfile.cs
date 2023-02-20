using AutoMapper;

namespace WebService.Profiles
{
    public class PayloadProfile : Profile
    {
        public PayloadProfile() 
        { 
            CreateMap<Entities.Payload, Models.PayloadDTO>();
            CreateMap<Models.PayloadForCreationDTO, Entities.Payload>();
            CreateMap<Entities.Payload, Models.PayloadForUpdateDTO>();
            CreateMap<Models.PayloadForUpdateDTO, Entities.Payload>();
        }
    }
}
