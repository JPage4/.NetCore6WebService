using AutoMapper;

namespace WebService.Profiles
{
    public class MessageProfile :Profile
    {
        public MessageProfile()     
        { 
            CreateMap<Entities.Message, Models.MessageDTO>();
            CreateMap<Models.MessageDTO, Entities.Message>();
        }

    }
}
