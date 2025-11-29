using System.ComponentModel.DataAnnotations;
namespace Weblog.Domain.Core.CategoryAgg.Dtos
{
    public class CreateCategoryDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required")]
        [StringLength(50, ErrorMessage = "Category Name cannot exceed 50 characters")]
        public string Name { get; set; }
        public string UserId { get; set; }
    }
}
