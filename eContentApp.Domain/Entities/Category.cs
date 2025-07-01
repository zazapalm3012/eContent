using System.ComponentModel.DataAnnotations;

namespace eContentApp.Domain.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; } = [];
        public Category()
        {
            Id = Guid.NewGuid();
        }
    }
}
