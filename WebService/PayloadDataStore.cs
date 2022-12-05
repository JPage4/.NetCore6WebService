using WebService.Models;
using WebService.Entities;

namespace WebService
{
    public class PayloadDataStore
    {
        public List<PayloadDTO> PayloadDTOs { get; set; }

        public PayloadDataStore()
        {
            PayloadDTOs = new List<PayloadDTO>()
            {
                new PayloadDTO()
                {
                    Id = new Guid("f0411596-95f7-4553-b5bc-f90b91534506"),
                    TS = 1530228282,
                    Sender = "testy-test-service",
                    Message = new MessageDTO()
                    {
                        Id = new Guid("1ce1fb75-d186-4700-a44a-b15382ca22ea"),
                        Foo = "bar",
                        Baz = "bang"
                    },
                    SentFromIp =  "1.2.3.4",
                    Priority = 2
                },

                new PayloadDTO()
                {
                    Id = new Guid("77194d53-0f5d-4868-bcf4-5adc693b6e62"),
                    TS = 1684234873,
                    Sender = "whoa very test",
                    Message = new MessageDTO()
                    { 
                        Id = new Guid("a5f0285e-8739-4b60-887d-d5b829164dd0"),
                        Foo = "big",
                        Baz = "whoop" 
                    },
                    SentFromIp =  "4.3.2.1",
                    Priority = 0
                }
            };
        }
    }
}
