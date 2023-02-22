using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using WebService.Entities;
using WebService.Models;
using NuGet.Protocol;
using Microsoft.AspNetCore.JsonPatch;
using WebService.Services;
using AutoMapper;

namespace WebService.Controllers
{
    [Route("api/payloads")]
    [ApiController]
    public class PayloadsController : ControllerBase
    {
        private readonly ILogger<PayloadsController> _logger;
        private readonly IMailService _mailService;
        private readonly IPayloadInfoRepository _payloadInfoRepository;
        private readonly IMapper _mapper;

        public PayloadsController(ILogger<PayloadsController> logger, 
            IMailService mailService, 
            IPayloadInfoRepository payloadInfoRepository,
            IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _mailService = mailService ?? throw new ArgumentException(nameof(mailService));
            _payloadInfoRepository = payloadInfoRepository ?? throw new ArgumentException(nameof(payloadInfoRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PayloadDTO>>> GetPayloadsAsync()
        {
            var payloadEntities = await _payloadInfoRepository.GetPayloadsAsync();
            
            return Ok(_mapper.Map<IEnumerable<PayloadDTO>>(payloadEntities));
        }

        [HttpGet("{payloadId}", Name = "GetPayload")]
        public async Task<IActionResult> GetPayloadAsync(Guid payloadId)
        {
            try
            {
                if (!PayloadExists(payloadId))
                {
                    _logger.LogInformation($"Payload with Id {payloadId} wasn't found");
                    return NotFound();
                }

                var payloadToReturn = await _payloadInfoRepository.GetPayloadAsync(payloadId);

                return Ok(_mapper.Map<PayloadDTO>(payloadToReturn));
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting payload with id {payloadId}", ex);
                return StatusCode(500, "A problem happened while handling request");
            }
        }

        [HttpPost]
        public async Task<ActionResult<PayloadDTO>> CreatePayload(PayloadForCreationDTO payload)
        {
            var newPayload = _mapper.Map<Payload>(payload);

            var result = ValidatePayload(newPayload);

            if (result.Result != null)
            {
                if (result.Result.GetType() != typeof(OkObjectResult))
                {
                    return BadRequest();
                }
            }
            _payloadInfoRepository.AddPayloadAsync(newPayload);

            await _payloadInfoRepository.SaveChangesAsync();

            var createdPayloadToReturn = _mapper.Map<PayloadDTO>(newPayload);

            string GetPayloadName = "GetPayload";
            return CreatedAtRoute(GetPayloadName,
                new
                {
                    payloadId = createdPayloadToReturn.Id,
                }, createdPayloadToReturn);
        }

        [HttpPut("{payloadId}")]
        public async Task<ActionResult<PayloadDTO>> UpdatePayload(Guid payloadId, PayloadForUpdateDTO updatedPayload)
        {
            if (!PayloadExists(payloadId))
            {
                return NotFound();
            }
            var payloadToUpdateEntity = _payloadInfoRepository.GetPayloadAsync(payloadId);

            await _payloadInfoRepository.SaveChangesAsync();

            await _mapper.Map(updatedPayload, payloadToUpdateEntity);

            await _payloadInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{payloadId}")]
        public async Task<ActionResult<PayloadDTO>> PartiallyUpdatePayload(Guid payloadId, JsonPatchDocument<PayloadForUpdateDTO> patchPayload)
        {
            if (!PayloadExists(payloadId))
            {
                return NotFound();
            }
            var payloadToPartiallyUpdateEntity = _payloadInfoRepository.GetPayloadAsync(payloadId);

            var payloadToPatch = _mapper.Map<PayloadForUpdateDTO>(payloadToPartiallyUpdateEntity);

            patchPayload.ApplyTo(payloadToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(!TryValidateModel(payloadToPatch))
            {
                return BadRequest(ModelState);
            }

            await _mapper.Map(payloadToPatch, payloadToPartiallyUpdateEntity);

            await _payloadInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{payloadId}")]
        public async Task<ActionResult> DeletePayload(Guid payloadId)
        {
            if (!PayloadExists(payloadId))
            {
                return NotFound();
            }
            var payloadToDelete = _payloadInfoRepository.GetPayloadAsync(payloadId);

            var payloadEntity = _mapper.Map<Payload>(payloadToDelete);

            if (payloadToDelete == null)
            {
                return NotFound();
            }

            _payloadInfoRepository.DeletePayloadAsync(payloadEntity);

            await _payloadInfoRepository.SaveChangesAsync();

            _mailService.Send("Payload deleted", $"Payload {payloadEntity.Id} was deleted");

            return NoContent();
        }


    #region VALIDATION
        private bool PayloadExists(Guid payloadId)
        {
            var payloadToReturn = _payloadInfoRepository.GetPayloadAsync(payloadId);

            return payloadToReturn == null ? false : true;
        }

        private ActionResult<Payload> ValidatePayload(Payload payload)
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
                if (IsValidJson(jsonPayload))
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
#endregion
    }
}