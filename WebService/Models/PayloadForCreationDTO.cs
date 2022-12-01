namespace WebService.Models
{
    public class PayloadForCreationDTO
    {
        public long TS { get; set; }

        public string Sender { get; set; }

        public MessageDTO Message { get; set; }

        public string? SentFromIp { get; set; }

        public int? Priority { get; set; }
    }
}
