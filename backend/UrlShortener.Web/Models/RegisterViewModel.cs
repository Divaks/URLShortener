using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Web.Models;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Вкажіть пошту")]
    [EmailAddress(ErrorMessage = "Некоректний формат пошти")]
    [Display(Name = "Email")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Введіть пароль")]
    [DataType(DataType.Password)]
    [StringLength(100, ErrorMessage = "{0} має бути мінімум {2} символів.", MinimumLength = 6)]
    [Display(Name = "Пароль")]
    public required string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Підтвердження пароля")]
    [Compare("Password", ErrorMessage = "Паролі не співпадають.")]
    public required string ConfirmPassword { get; set; }
}