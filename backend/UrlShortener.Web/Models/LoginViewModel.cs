using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Web.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Вкажіть пошту")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "Введіть пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Запам'ятати мене?")]
    public bool RememberMe { get; set; }
}