using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManagement.Areas.Admin.Models;

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Ad alanı boş bırakılamaz.")]
    [Display(Name = "Adı :")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Soyad alanı boş bırakılamaz.")]
    [Display(Name = "Soyadı :")]
    public string Surname { get; set; }

    [EmailAddress(ErrorMessage ="Email formatı yanlış.")]
    [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
    [Display(Name = "Email :")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Departman alanı boş bırakılamaz.")]
    [Display(Name = "Departman :")]
    public int DepartmentId { get; set; }
    
    public List<SelectListItem> Departments { get; set; } 

    [Required(ErrorMessage = "Telefon alanı boş bırakılamaz.")]
    [Display(Name = "Telefon :")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
    [Display(Name = "Şifre :")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakterli olabilir.")]
    public string Password { get; set; }
    [Compare(nameof(Password),ErrorMessage ="Girmiş olduğunuz şifreler eşleşmiyor.")]
    [Required(ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz.")]
    [Display(Name = "Şifre Tekrar :")]
    [MinLength(6, ErrorMessage = "Şifreniz en az 6 karakterli olabilir.")]
    [DataType(DataType.Password)]
    public string PasswordConfirm { get; set; }
    public List<string> Errors { get; set; }

}