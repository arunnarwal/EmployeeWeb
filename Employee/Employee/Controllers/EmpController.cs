using Employee.Contracts;
using Employee.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        IEmployee _iEmployee;
        public EmpController(IEmployee iEmployee)
        {
            _iEmployee = iEmployee;
        }
        [HttpPost]
        [Route("Add")]
        public async Task<Response<string>> Add(EmployeeModel empModel) {
            if (ModelState.IsValid)
            {
                return await _iEmployee.AddEmployee(empModel);
            }
            else
              return  new Response<string>(); 
        }

        [HttpGet]
        [Route("Get")]
        public async Task<Response<List<EmployeeModel>>> Get()
        {
            
                return await _iEmployee.GetEmployee();
            
        }
    }
}
