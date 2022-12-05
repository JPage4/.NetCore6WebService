using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using WebService.Entities;
using WebService.Models;
using NuGet.Protocol;
using Microsoft.AspNetCore.JsonPatch;
using WebService.Services;

namespace WebService.Controllers
{
    [Route("api/payloads")]
    [ApiController]
    public class PayloadsController : ControllerBase
    {
        private readonly ILogger<PayloadsController> _logger;
        private readonly IMailService _mailService;
        private readonly PayloadDataStore _payloadDataStore;
        public PayloadsController(ILogger<PayloadsController> logger, IMailService mailService, PayloadDataStore payloadDataStore)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentException(nameof(mailService));
            _payloadDataStore = payloadDataStore ?? throw new ArgumentException(nameof(payloadDataStore));
        }

        [HttpGet]
        public ActionResult<IEnumerable<PayloadDTO>> GetPayloads()
        {
            return Ok(_payloadDataStore.PayloadDTOs);
        }

        [HttpGet("{payloadId}", Name = "GetPayload")]
        public ActionResult<Payload> GetPayload(Guid payloadId)
        {
            try
            {
                if (!PayloadExists(payloadId))
                {
                    _logger.LogInformation($"Payload with Id {payloadId} wasn't found");
                    return NotFound();
                }

                var payloadToReturn = _payloadDataStore.PayloadDTOs.FirstOrDefault(p => p.Id == payloadId);

                return Ok(payloadToReturn);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting payload with id {payloadId}", ex);
                return StatusCode(500, "A problem happened while handling request");
            }
        }

        [HttpPost]
        public ActionResult<PayloadDTO> CreatePayload(PayloadForCreationDTO payload)
        {
            var newPayloadId = Guid.NewGuid();
            var newPayload = new PayloadDTO
            {
                TS = payload.TS,
                Sender = payload.Sender,
                Message = payload.Message,
                SentFromIp = payload.SentFromIp,
                Priority = payload.Priority,
            };

            var result = ValidatePayload(newPayload);

            if (result.Result != null)
            {
                if(result.Result.GetType() != typeof(OkObjectResult))
                {
                    return BadRequest();
                }
            }
            

            string GetPayloadName = "GetPayload";
            return CreatedAtRoute(GetPayloadName, 
                new 
                {
                    payloadId = newPayloadId,
                }, newPayload);
        }

        [HttpPut("{payloadId}")]
        public ActionResult<PayloadDTO> UpdatePayload(Guid payloadId, PayloadForUpdateDTO updatedPayload)
        {
            if (!PayloadExists(payloadId))
            {
                return NotFound();
            }
            var payloadToUpdate = _payloadDataStore.PayloadDTOs.FirstOrDefault(p => p.Id == payloadId);


            payloadToUpdate.TS = updatedPayload.TS;
            payloadToUpdate.Sender = updatedPayload.Sender;
            payloadToUpdate.Message = updatedPayload.Message;
            payloadToUpdate.SentFromIp = updatedPayload.SentFromIp;
            payloadToUpdate.Priority = updatedPayload.Priority;

            return NoContent();
        }

        [HttpPatch("{payloadId}")]
        public ActionResult<PayloadDTO> EditPayload(Guid payloadId, JsonPatchDocument<PayloadForUpdateDTO> patchPayload)
        {
            if (!PayloadExists(payloadId))
            {
                return NotFound();
            }
            var payloadToEdit = _payloadDataStore.PayloadDTOs.FirstOrDefault(p => p.Id == payloadId);


            var newPatchedPayload =
                new PayloadForUpdateDTO()
                {
                    TS = payloadToEdit.TS,
                    Sender = payloadToEdit.Sender,
                    Message = payloadToEdit.Message,
                    SentFromIp = payloadToEdit.SentFromIp,
                    Priority = payloadToEdit.Priority,
                };
            patchPayload.ApplyTo(newPatchedPayload, ModelState);

            if(!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }

            payloadToEdit.TS = newPatchedPayload.TS;
            payloadToEdit.Sender = payloadToEdit.Sender;
            payloadToEdit.Message = payloadToEdit.Message;
            payloadToEdit.SentFromIp = payloadToEdit.SentFromIp;
            payloadToEdit.Priority = payloadToEdit.Priority;
            
            return NoContent();
        }
        [HttpDelete("{payloadId}")]
        public ActionResult DeletePayload(Guid payloadId)
        {
            if (!PayloadExists(payloadId))
            {
                return NotFound();
            }
            var payloadToDelete = _payloadDataStore.PayloadDTOs.FirstOrDefault(p => p.Id == payloadId);

            _payloadDataStore.PayloadDTOs.Remove(payloadToDelete);
            _mailService.Send("Payload deleted", $"Payload {payloadToDelete.Id} was deleted");
            return NoContent();
        }


        //VALIDATION
        private bool PayloadExists(Guid payloadId)
        {
            var payloadToReturn = _payloadDataStore.PayloadDTOs.FirstOrDefault(p => p.Id == payloadId);

            return payloadToReturn == null ? false : true;
        }

        private ActionResult<PayloadDTO> ValidatePayload(PayloadDTO payload)
        {
        //Timestamp
            long minRange = 0;
            DateTime now = DateTime.Now;
            long maxRange = ((DateTimeOffset)now).ToUnixTimeSeconds();

            if (payload.TS != 0 && (payload.TS < minRange || payload.TS >= maxRange)) 
            {
                return BadRequest(payload.TS);
            }

        //Sender
            if (payload.Sender != null)
            {
                if (payload.Sender.GetType() != typeof(string)) 
                {
                    return BadRequest(payload.Sender);
                }
            }

        //Message
            if (payload.Message != null)
            {
                string jsonPayload = payload.Message.ToJson();
                if(IsValidJson(jsonPayload))
                {
                    if (payload.Message.Foo == null || payload.Message.Baz == null)
                    {
                        return BadRequest(payload.Message);
                    }
                }
            }

        //IP Address
            if (payload.SentFromIp != null)
            {
                if (!IPAddress.TryParse(payload.SentFromIp, out System.Net.IPAddress? address))
                {
                    return BadRequest(payload.SentFromIp);
                }
            }
            return Ok(payload);
        }

        private static bool IsValidJson(string strInput)
        {
            if (string.IsNullOrWhiteSpace(strInput)) { return false; }
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")))
            {
                try
                {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    //Exception in parsing json
                    Console.WriteLine(jex.Message);
                    return false;
                }
                catch (Exception ex) //some other exception
                {
                    Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
        //private readonly PayloadContext _context;

        //public PayloadsController(PayloadContext context)
        //{
        //    _context = context;
        //}

        //// GET: api/Payloads
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Payload>>> GetPayloads()
        //{
        //  if (_context.Payloads == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Payloads.ToListAsync();
        //}

        //// GET: api/Payloads/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Payload>> GetPayload(Guid id)
        //{
        //  if (_context.Payloads == null)
        //  {
        //      return NotFound();
        //  }
        //    var payload = await _context.Payloads.FindAsync(id);

        //    if (payload == null)
        //    {
        //        return NotFound();
        //    }

        //    return payload;
        //}

        //// PUT: api/Payloads/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPayload(Guid id, Payload payload)
        //{
        //    bool isValid = ValidatePayload(payload);
        //    if (!isValid) 
        //    {
        //        return BadRequest();
        //    }

        //    if (id != payload.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(payload).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PayloadExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Payloads
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Payload>> PostPayload(Payload payload)
        //{
        //    bool isValid = ValidatePayload(payload);
        //    if (!isValid)
        //    {
        //        return BadRequest();
        //    }

        //    if (_context.Payloads == null)
        //    {
        //        return Problem("Entity set 'PayloadContext.Payloads'  is null.");
        //    }

        //    _context.Payloads.Add(payload);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetPayload), new { id = payload.Id }, payload);
        //}

        //// DELETE: api/Payloads/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePayload(Guid id)
        //{
        //    if (_context.Payloads == null)
        //    {
        //        return NotFound();
        //    }
        //    var payload = await _context.Payloads.FindAsync(id);
        //    if (payload == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Payloads.Remove(payload);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
