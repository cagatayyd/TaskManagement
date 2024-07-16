namespace TaskManagement.Web.Areas.Admin.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PictureUrl { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public List<string> Roles { get; set; }
    }

}
