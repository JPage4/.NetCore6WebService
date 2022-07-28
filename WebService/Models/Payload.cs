using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebService.Models
{
    public class Payload
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();

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

    public class Message
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required]
        public string Foo { get; set; }
        public string Baz { get; set; }
    }
}
