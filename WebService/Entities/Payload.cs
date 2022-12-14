using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebService.Entities
{
    public class Payload
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public Guid Id { get; set; } = new Guid();

        [Required]
        public long TS {get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public Guid MessageId { get; set; }

        [Required]
        [ForeignKey("MessageId")]
        public Message Message { get; set; }


        [JsonPropertyName("sent-from-ip")]
        public string? SentFromIp { get; set; }

        public int? Priority { get; set; }
    }
}
