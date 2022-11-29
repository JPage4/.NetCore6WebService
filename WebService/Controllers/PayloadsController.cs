using Microsoft.AspNetCore.Mvc;
using WebService.Models;

namespace WebService.Controllers
{
    [Route("api/payloads")]
    [ApiController]
    public class PayloadsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PayloadDTO>> GetPayloads()
        {
            return Ok(PayloadDataStore.Current.PayloadDTOs);
        }

        [HttpGet("{id}")]
        public ActionResult<PayloadDTO> GetPayload(Guid id)
        {
            var payloadToReturn = PayloadDataStore.Current.PayloadDTOs.FirstOrDefault(p => p.Id == id);

            if (payloadToReturn == null) 
            {
                return NotFound();
            }
            return Ok(payloadToReturn);
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

        //private bool PayloadExists(Guid id)
        //{
        //    return (_context.Payloads?.Any(e => e.Id == id)).GetValueOrDefault();
        //}

        //private bool ValidateTimeStamp(Payload payload)
        //{
        //    long minRange = 0;
        //    DateTime now = DateTime.Now;
        //    long maxRange = ((DateTimeOffset)now).ToUnixTimeSeconds();
        //    bool isValid = false;

        //    if(payload.TS > minRange && payload.TS <= maxRange)
        //    {
        //        isValid = true;
        //    }
        //    return isValid;
        //}

        //private bool ValidateSender(Payload payload)
        //{
        //    bool isValid = false;
        //    if (payload.Sender != null)
        //    {
        //        if(payload.Sender.GetType() == typeof(string))
        //        {
        //            isValid= true;
        //        }
        //        return isValid;
        //    }

        //    return false;
        //}

        //private bool ValidateMessage(Payload payload)
        //{
        //    bool isValid = false;
        //    if (payload.Message!= null) 
        //    { //IF MESSAGE IS A VALID JSON OBJECT??? IDK json schema isn't working... come back to this
        //        if (payload.Message.Foo!= null || payload.Message.Baz != null) 
        //        { 
        //            isValid = true;
        //        }
        //    }
        //    return isValid;
        //}

        //private bool ValidateIPAddress(Payload payload) 
        //{
        //    bool isValid = false;
        //    if (payload.SentFromIp != null)
        //    {
        //        if (IPAddress.TryParse(payload.SentFromIp, out System.Net.IPAddress? address))
        //        {
        //            isValid= true;
        //        }
        //    }
        //    return isValid;
        //}

        //private bool ValidatePayload(Payload payload)
        //{
        //    bool isValidTimeStamp = ValidateTimeStamp(payload);
        //    bool isValidSender = ValidateSender(payload);
        //    bool isValidMessage = ValidateMessage(payload);
        //    bool isValidIPAddress = ValidateIPAddress(payload);
        //    bool isValidPayload = false;

        //    if (isValidTimeStamp && isValidSender && isValidMessage && isValidIPAddress)
        //    {
        //        isValidPayload = true;
        //    }

        //    return isValidPayload;
        //}
