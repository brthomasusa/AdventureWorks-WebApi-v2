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
        public IActionResult GetDepartments([FromQuery] DepartmentParameters deptParameters)
        {
            var departments = _repository.Department.GetDepartments(deptParameters);

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
        public IActionResult GetDepartmentByID(int departmentID)
        {
            var dept = _repository.Department.GetDepartmentByID(departmentID);
            if (dept == null)
            {
                return NotFound();
            }

            return Ok(dept);
        }

        [HttpPost]
        public IActionResult CreateDepartment([FromBody] Department dept)
        {
            _repository.Department.CreateDepartment(dept);

            return CreatedAtRoute(nameof(GetDepartmentByID), new { departmentID = dept.DepartmentID }, dept);
        }


        [HttpPut]
        public IActionResult UpdateDepartment([FromBody] Department dept)
        {
            _repository.Department.UpdateDepartment(dept);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteDepartment([FromBody] Department dept)
        {
            _repository.Department.DeleteDepartment(dept);
            return NoContent();
        }





    }
}