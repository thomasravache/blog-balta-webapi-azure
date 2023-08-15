using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve ter entre 3 e 40 caractéres")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "O Slug é obrigatório")]
        public string Slug { get; set; } = null!;
    }
}