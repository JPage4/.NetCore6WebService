using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebService.Entities
{
    public class Payload
    {
        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        public long TS {get; set; }

        [Required]
        public string Sender { get; set; }


        [Required]
        public Message Message { get; set; } = new Message();

        [Required]
        [ForeignKey("MessageId")]
        public Guid MessageId { get => Message.Id; set => Message.Id = value; }

        [JsonPropertyName("sent-from-ip")]
        public string? SentFromIp { get; set; }

        public int? Priority { get; set; }
    }
}
