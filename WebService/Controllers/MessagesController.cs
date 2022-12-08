using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.DbContexts;
using WebService.Entities;
using WebService.Models;
using WebService.Services;

namespace WebService.Controllers
{
    [Route("api/payloads/{payloadId}/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly PayloadDataStore _payloadDataStore;
        private readonly IPayloadInfoRepository _payloadInfoRepository;
        private readonly IMapper _mapper;

        public MessagesController(PayloadDataStore payloadDataStore, 
            IPayloadInfoRepository payloadInfoRepository,
            IMapper mapper) 
        {
            _payloadDataStore = payloadDataStore ?? throw new ArgumentException(nameof(payloadDataStore));
            _payloadInfoRepository = payloadInfoRepository ?? throw new ArgumentException(nameof(payloadInfoRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<MessageDTO>> GetMessageAsync(Guid payloadId)
        {
            var message = await _payloadInfoRepository.GetMessageFromPayloadAsync(payloadId);

            if (!MessageExists(payloadId))
            {
                return NotFound();
            }

            return Ok(_mapper.Map<MessageDTO>(message));
        }

        //VALIDATION
        private bool MessageExists(Guid payloadId)
        {
            var messageToReturn = _payloadInfoRepository.GetMessageFromPayloadAsync(payloadId);

            return messageToReturn == null ? false : true;
        }
    }
}
