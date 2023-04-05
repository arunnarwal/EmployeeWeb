using Employee.Constraints;
using Employee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Employee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _iConfiguration;

        public HomeController(ILogger<HomeController> logger, IConfiguration iConfiguration)
        {
            _logger = logger;
            _iConfiguration = iConfiguration;
        }

        public async Task<IActionResult> Index()
        {
            EmployeeViewModel employee = new EmployeeViewModel();
            employee.EmployeeList = new List<EmployeeModel>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync($"{_iConfiguration["ApiUrl"]}{ApiUrl.Get}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<Response<List<EmployeeModel>>>(json);
                    employee.EmployeeList = data.Data;
                }
            }
            return View(employee);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Index(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(new EmployeeModel { FirstName=employeeViewModel.FirstName,LastName=employeeViewModel.LastName}), Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync($"{_iConfiguration["ApiUrl"]}{ApiUrl.Add}",content);

                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<Response<string>>(json);
                        TempData["EmpStatus"] = data.Data;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
