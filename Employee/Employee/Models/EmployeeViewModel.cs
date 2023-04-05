using System.Collections.Generic;

namespace Employee.Models
{
    public class EmployeeViewModel:EmployeeModel
    {
        public List<EmployeeModel> EmployeeList { get; set; }
    }
}
