using Microsoft.EntityFrameworkCore;
using WebService.DbContexts;
using WebService.Entities;

namespace WebService.Services
{
    public class PayloadInfoRepository : IPayloadInfoRepository
    {
        private readonly PayloadContext _context;

        public PayloadInfoRepository(PayloadContext context) 
        {
            _context = context ?? throw new ArgumentException(nameof(context)); 
        }
        public async Task<IEnumerable<Payload>> GetPayloadsAsync()
        {
            return await _context.Payloads.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Payload?> GetPayloadAsync(Guid payloadId)
        {
            return await _context.Payloads.Where(p => p.Id == payloadId).FirstOrDefaultAsync();
        }

        public async Task<Message?> GetMessageFromPayloadAsync(Guid payloadId)
        {
            var payload = await _context.Payloads.Where(p => p.Id == payloadId).FirstOrDefaultAsync();

            if (payload == null) 
            { 
                return null;
            }

            return await _context.Messages.Where(m => m.Id == payload.MessageId).FirstOrDefaultAsync();
        }

        public void AddPayloadAsync(Payload payload)
        {
            _context.Payloads.Add(payload);
        }

        public void DeletePayloadAsync(Payload payload)
        {
            _context.Payloads.Remove(payload);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0); //return true when 0 or more entities has been saved
                                                            //but at some point we'll want to change to 1 or more
        }
    }
}
