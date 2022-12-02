namespace WebService.Models
{
    public class MessageDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Foo { get; set; }

        public string? Baz { get; set; }
    }
}
