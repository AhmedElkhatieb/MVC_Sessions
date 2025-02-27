using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Models.Departments
{
    public class UpdateDepartmentDto
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
        [Display(Name = "Creation Date")]
        public DateOnly CreationDate { get; set; }
        public int Id { get; set; }
        
    }
}
