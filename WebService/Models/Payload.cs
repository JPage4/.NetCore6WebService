using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebService.Models
{
    public class Payload
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public long TS {get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public Message Message { get; set; }

        [JsonPropertyName("sent-from-ip")]
        public string? SentFromIp { get; set; }

        public int? Priority { get; set; }
    }
}
