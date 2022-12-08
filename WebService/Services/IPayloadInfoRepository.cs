using WebService.Entities;

namespace WebService.Services
{
    public interface IPayloadInfoRepository
    {
        Task<IEnumerable<Payload>> GetPayloadsAsync();

        Task<Payload?> GetPayloadAsync(Guid payloadId);

        Task<Message?> GetMessageFromPayloadAsync(Guid payloadId);

        void AddPayloadAsync(Payload payload);

        Task<bool> SaveChangesAsync();
    }
}
