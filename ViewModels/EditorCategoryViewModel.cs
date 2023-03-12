using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "Category name is required")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "This field must have between 3 and 40 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        public string Slug { get; set; }
    }
}