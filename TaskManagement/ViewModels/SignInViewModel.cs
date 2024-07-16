using System.ComponentModel.DataAnnotations;

namespace TaskManagement.ViewModels
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {

        }
        public SignInViewModel(string email, string password)
        {
            Email = email;
           Password = password;
        }
        [Required(ErrorMessage = "E Posta alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "E Posta formatı yanlış.")]
        [Display(Name = "E Posta :")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [Display(Name = "Şifre :")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakterli olabilir.")]
        public string Password { get; set; }
    }
}
