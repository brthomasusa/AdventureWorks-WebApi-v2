using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Person;
using LoggerService;

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
        public IActionResult GetAllEmployees()
            => Ok(_repository.Employee.GetEmployees(new EmployeeParameters { PageNumber = 1, PageSize = 10 }));


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
    }
}