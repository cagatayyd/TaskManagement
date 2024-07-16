using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Areas.Admin.Models
{
    public class RoleModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Rol isim alanı boş bırakılamaz.")]
        [Display(Name="Rol Adı")]
        public string Name { get; set; }
        public bool Exist { get; set; }

    }
}
