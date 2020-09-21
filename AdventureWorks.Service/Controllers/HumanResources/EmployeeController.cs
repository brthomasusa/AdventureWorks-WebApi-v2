using System.Threading.Tasks;
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
        public async Task<IActionResult> GetAllEmployees([FromQuery] EmployeeParameters employeeParameters)
        {
            if (employeeParameters.EmployeeStatus != null && !employeeParameters.IsValidEmployeeStatus)
            {
                return BadRequest(new { message = "Valid employee status codes are 'Active', 'Inactive', and 'All'." });
            }

            var employees = await _repository.Employee.GetEmployees(employeeParameters);

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
        public async Task<IActionResult> GetEmployeeByID(int employeeID)
        {
            var employee = await _repository.Employee.GetEmployeeByID(employeeID);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("{employeeID}/details")]
        public async Task<IActionResult> GetEmployeeByIDWithDetails(int employeeID)
        {
            var employee = await _repository.Employee.GetEmployeeByIDWithDetails(employeeID);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("{employeeID}/phones")]
        public async Task<IActionResult> GetEmployeePhones(int employeeID, [FromQuery] PersonPhoneParameters phoneParameters)
        {
            var phones = await _repository.Telephone.GetPhones(employeeID, phoneParameters);

            var metadata = new
            {
                phones.TotalCount,
                phones.PageSize,
                phones.CurrentPage,
                phones.TotalPages,
                phones.HasNext,
                phones.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(phones);
        }


        [HttpGet("{employeeID}/phones/{phoneNumber}/{phoneNumberTypeID}", Name = "GetEmployeePhoneByID")]
        public async Task<IActionResult> GetEmployeePhoneByID(int employeeID, string phoneNumber, int phoneNumberTypeID)
        {
            var phone = await _repository.Telephone.GetPhoneByID(employeeID, phoneNumber, phoneNumberTypeID);

            if (phone == null)
            {
                return NotFound();
            }

            return Ok(phone);
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] EmployeeDomainObj employee)
        {
            _repository.Employee.CreateEmployee(employee);
            return CreatedAtRoute(nameof(GetEmployeeByID), new { employeeID = employee.BusinessEntityID }, employee);
        }

        [HttpPost("phones")]
        public IActionResult CreateEmployeePhone([FromBody] PersonPhone phone)
        {
            _repository.Telephone.CreatePhone(phone);
            return CreatedAtRoute(nameof(GetEmployeePhoneByID),
                new
                {
                    employeeID = phone.BusinessEntityID,
                    phoneNumber = phone.PhoneNumber,
                    PhoneNumberTypeID = phone.PhoneNumberTypeID
                },
                phone);
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

        [HttpDelete("phones")]
        public IActionResult DeleteEmployeePhone([FromBody] PersonPhone phone)
        {
            _repository.Telephone.DeletePhone(phone);
            return NoContent();
        }
    }
}