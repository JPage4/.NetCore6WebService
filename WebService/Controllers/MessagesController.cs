using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebService.DbContexts;
using WebService.Entities;
using WebService.Models;

namespace WebService.Controllers
{
    [Route("api/payloads/{payloadId}/messages")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly PayloadDataStore _payloadDataStore;

        public MessagesController(PayloadDataStore payloadDataStore) 
        {
            _payloadDataStore = payloadDataStore ?? throw new ArgumentException(nameof(payloadDataStore));
        }

        [HttpGet]
        public ActionResult<MessageDTO> GetMessage(Guid payloadId)
        {
            var payload = _payloadDataStore.PayloadDTOs.FirstOrDefault(p => p.Id == payloadId);

            if (payload == null)
            {
                return NotFound();
            }

            var message = payload.Message;
            if(message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }
    }
}
