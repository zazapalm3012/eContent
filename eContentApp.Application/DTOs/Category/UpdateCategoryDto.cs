using System.ComponentModel.DataAnnotations;

namespace eContentApp.Application.DTOs.Category
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Category ID is required for update.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Category name is required.")]
        [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }
    }
}

