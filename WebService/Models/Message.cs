using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebService.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Foo { get; set; }

        public string? Baz { get; set; }
    }
}
