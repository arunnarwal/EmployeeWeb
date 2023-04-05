using Employee.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Contracts
{
    public interface IEmployee
    {
        Task<Response<string>>AddEmployee(EmployeeModel empModel);
        Task<Response<List<EmployeeModel>>>GetEmployee();
    }
}
