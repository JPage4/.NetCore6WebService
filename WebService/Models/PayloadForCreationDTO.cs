using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WebService.Entities;

namespace WebService.Models
{
    public class PayloadForCreationDTO
    {
        [Required(ErrorMessage = "TimeStamp property is required")]
        public long TS { get; set; }

        [Required(ErrorMessage = "Sender property is required")]
        public string Sender { get; set; }

        [Required(ErrorMessage = "Message property is required")]
        public MessageDTO Message { get; set; }

        [JsonPropertyName("sent-from-ip")]
        public string? SentFromIp { get; set; }

        public int? Priority { get; set; }
    }
}
