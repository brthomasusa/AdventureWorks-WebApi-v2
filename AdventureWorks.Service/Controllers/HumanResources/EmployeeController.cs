using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Person;
using LoggerService;
using Newtonsoft.Json;

namespace AdventureWorks.Service.Controllers
{
    [Route("api/employees")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryCollection _repository;

        public EmployeeController(ILoggerManager logger, IRepositoryCollection repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllEmployees([FromQuery] EmployeeParameters employeeParameters)
        {
            if (employeeParameters.EmployeeStatus != null && !employeeParameters.IsValidEmployeeStatus)
            {
                return BadRequest(new { message = "Valid employee status codes are 'Active', 'Inactive', and 'All'." });
            }

            var employees = _repository.Employee.GetEmployees(employeeParameters);

            var metadata = new
            {
                employees.TotalCount,
                employees.PageSize,
                employees.CurrentPage,
                employees.TotalPages,
                employees.HasNext,
                employees.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(employees);
        }


        [HttpGet("{employeeID}", Name = "GetEmployeeByID")]
        public IActionResult GetEmployeeByID(int employeeID)
        {
            var employee = _repository.Employee.GetEmployeeByID(employeeID);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("{employeeID}/details")]
        public IActionResult GetEmployeeByIDWithDetails(int employeeID)
        {
            var employee = _repository.Employee.GetEmployeeByIDWithDetails(employeeID);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] EmployeeDomainObj employee)
        {
            _repository.Employee.CreateEmployee(employee);
            return CreatedAtRoute(nameof(GetEmployeeByID), new { employeeID = employee.BusinessEntityID }, employee);
        }

        [HttpPut]
        public IActionResult UpdateEmployee([FromBody] EmployeeDomainObj employee)
        {
            _repository.Employee.UpdateEmployee(employee);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteEmployee([FromBody] EmployeeDomainObj employee)
        {
            _repository.Employee.DeleteEmployee(employee);
            return NoContent();
        }
    }
}