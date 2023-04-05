using Dapper;
using Employee.Constraints;
using Employee.Contracts;
using Employee.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Linq;

namespace Employee.Repository
{
    public class EmployeeRepository : IEmployee
    {
        private readonly IConfiguration _iConfiguration;

        public EmployeeRepository(IConfiguration iconfiguration)
        {
            _iConfiguration = iconfiguration;
        }
        public async Task<Response<string>> AddEmployee(EmployeeModel empModel)
        {
         
            try
            {
                List<EmployeeModel> dataList = new List<EmployeeModel>();
                if (File.Exists(_iConfiguration["JsonFile"]))
                {
                    string json = await File.ReadAllTextAsync(_iConfiguration["JsonFile"]);
                    dataList = JsonConvert.DeserializeObject<List<EmployeeModel>>(json);
                    if (dataList == null)
                        dataList = new List<EmployeeModel>();
                }
                var checkdt = dataList.Where(x => x.FirstName.ToLower() == empModel.FirstName.ToLower()).FirstOrDefault();
                if (checkdt == null)
                {
                    dataList.Add(new EmployeeModel { FirstName = empModel.FirstName, LastName = empModel.LastName });
                    string updatedJson = JsonConvert.SerializeObject(dataList);
                    await File.WriteAllTextAsync(_iConfiguration["JsonFile"], updatedJson);
                    return new Response<string>
                    {
                        IsSuccess = true,
                        Message = ResponseConstant.MessageSuccess,
                        Data = ResponseConstant.Success 
                    };
                }
                else
                    return new Response<string>
                    {
                        IsSuccess = true,
                        Message = ResponseConstant.MessageFailure,
                        Data = ResponseConstant.Failure
                    };

            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    IsSuccess = true,
                    Message = ResponseConstant.MessageFailure,
                    Data = ex.Message.ToString()
                };
            }
        }

        public async Task<Response<List<EmployeeModel>>> GetEmployee()
        {
            Response<List<EmployeeModel>> response = null;
            try
            {
                List<EmployeeModel> dataList = new List<EmployeeModel>();
                if (File.Exists(_iConfiguration["JsonFile"]))
                {
                    string json = await File.ReadAllTextAsync(_iConfiguration["JsonFile"]);
                    dataList = JsonConvert.DeserializeObject<List<EmployeeModel>>(json);
                }
                    return new Response<List<EmployeeModel>>
                    {
                        IsSuccess = true,
                        Message = ResponseConstant.MessageSuccess,
                        StatusCode=(int)HttpStatusCode.OK,
                        Data = dataList
                    };
                
            }
            catch (Exception ex)
            {
                return new Response<List<EmployeeModel>>
                {
                    IsSuccess = true,
                    Message = ResponseConstant.MessageFailure,
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Data =new List<EmployeeModel>()
                };
            }
        }
    }
}
