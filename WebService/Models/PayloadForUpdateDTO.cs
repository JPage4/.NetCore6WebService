using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebService.Models
{
    public class PayloadForUpdateDTO
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
