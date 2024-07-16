using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Web.ViewModels
{
    public class UserEditViewModel
    {
        [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
        [Display(Name = "Telefon :")]
        public string Phone { get; set; } = null!;


        [Display(Name = "Fotoğraf :")]
        public IFormFile? Picture { get; set; }

    }
}
