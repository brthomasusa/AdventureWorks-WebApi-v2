using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using LoggerService;
using Newtonsoft.Json;

namespace AdventureWorks.Service.Controllers.HumanResources
{
    [Route("api/department")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryCollection _repository;

        public DepartmentController(ILoggerManager logger, IRepositoryCollection repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartments([FromQuery] DepartmentParameters deptParameters)
        {
            var departments = await _repository.Department.GetDepartments(deptParameters);

            var metadata = new
            {
                departments.TotalCount,
                departments.PageSize,
                departments.CurrentPage,
                departments.TotalPages,
                departments.HasNext,
                departments.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(departments);
        }



        [HttpGet("{departmentID}", Name = "GetDepartmentByID")]
        public async Task<IActionResult> GetDepartmentByID(int departmentID)
        {
            var dept = await _repository.Department.GetDepartmentByID(departmentID);
            if (dept == null)
            {
                return NotFound();
            }

            return Ok(dept);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] Department dept)
        {
            await _repository.Department.CreateDepartment(dept);

            return CreatedAtRoute(nameof(GetDepartmentByID), new { departmentID = dept.DepartmentID }, dept);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateDepartment([FromBody] Department dept)
        {
            await _repository.Department.UpdateDepartment(dept);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteDepartment([FromBody] Department dept)
        {
            await _repository.Department.DeleteDepartment(dept);
            return NoContent();
        }
    }
}