using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebService.Entities;

namespace WebService.Models
{
    public class PayloadDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [DataType(DataType.DateTime)]
        public long TS { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public MessageDTO Message { get; set; }
        public Guid MessageId { get => Message.Id; set => Message.Id = value; }

        [JsonPropertyName("sent-from-ip")]
        public string? SentFromIp { get; set; }

        public int? Priority { get; set; }
    }
}
