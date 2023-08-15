using System.ComponentModel.DataAnnotations;
using Blog.Models;

namespace Blog.ViewModels.Accounts;

public class RegisterViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "O E-mail é obrigatório")]
    [EmailAddress(ErrorMessage = "O e-mail é inválido")]
    public string Email { get; set; } = null!;

    // Exemplos de conversão de um RegisterViewModel pra User (OBS: com implicit/explicit é ligeiramente mais rápido (BEM POUCO))
    public static explicit operator User(RegisterViewModel registerViewModel)
        => new User
        {
            Name = registerViewModel.Name,
            Email = registerViewModel.Email,
            Slug = registerViewModel.Email.Replace("@", "-").Replace(".", "-")
        };

    public static User ToUser(RegisterViewModel registerViewModel)
        => new User
        {
            Name = registerViewModel.Name,
            Email = registerViewModel.Email,
            Slug = registerViewModel.Email.Replace("@", "-").Replace(".", "-")
        };

    // Exemplos de conversão de um User pra RegisterViewModel (OBS: com implicit/explicit é ligeiramente mais rápido (BEM POUCO))
    public static explicit operator RegisterViewModel(User user)
        => new RegisterViewModel
        {
            Name = user.Name,
            Email = user.Email
        };

    public static RegisterViewModel FromUser(User user)
        => new RegisterViewModel
        {
            Name = user.Name,
            Email = user.Email
        };

}