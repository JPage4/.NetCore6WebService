using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebService.Entities
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Foo { get; set; }

        public string? Baz { get; set; }
    }
}
