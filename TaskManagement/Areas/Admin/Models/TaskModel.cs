using System.ComponentModel;
using TaskManagement.Core.Entities;

namespace TaskManagement.Areas.Admin.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        [DisplayName("Görev Tanımı : ")]
        public string Name { get; set; }
        [DisplayName("Başlangıç Tarihi : ")]
        public DateTime StartDate { get; set; }
        [DisplayName("Bitiş Tarihi : ")]
        public DateTime EndDate { get; set; }
        [DisplayName("Görev Detayı :")]
        public string Detail { get; set; }
        [DisplayName("Yönetici Onayı : ")]
        public bool IsApproved { get; set; }
        [DisplayName("Görev İlerleyişi :")]
        public bool IsInProgress { get; set; }
        [DisplayName("İlerleme ( % ) : ")]
        public int CompletionProgress { get; set; }
        public List<string> EmployeeNames { get; set; }
        public List<string> EmployeeDepartments { get; set; }

        public List<string> EmployeeSurnames { get; set; }
        [DisplayName(" Görevliler : ")]
        public ICollection<int> EmployeeTasks { get; set; } = new List<int>();
        public List<CorporateTask> Tasks { get; set; }
        public Department Department { get; set; }
		public int DepartmentId { get; set; }


	}
}
