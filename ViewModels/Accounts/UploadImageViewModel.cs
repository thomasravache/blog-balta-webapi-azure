using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Accounts;

public class UploadImageViewModel
{
    [Required(ErrorMessage = "Imagem Inválida")]
    public string Base64Image { get; set; } // na maioria das vezes quando o front trabalha com imagem eles mandam uma imagem em base64
}