using TaskManagement.Core.Entities;

namespace TaskManagement.Areas.Admin.Models
{
    public class EmployeeTaskModel
    {
        public int EmployeeId;
        public Employee Employee { get; set; }

        public int TaskId { get; set; }
        public CorporateTask CorporateTasks { get; set; }
    }
}
