using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.HumanResources;
using LoggerService;
using Newtonsoft.Json;

namespace AdventureWorks.Service.Controllers.HumanResources
{
    [Route("api/employees")]
    [ApiController]
    public class DeptHistoryController : ControllerBase
    {

        private ILoggerManager _logger;
        private IRepositoryCollection _repository;

        public DeptHistoryController(ILoggerManager logger, IRepositoryCollection repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{employeeID}/depthistory")]
        public async Task<IActionResult> GetDeptHistories(int employeeID, [FromQuery] DepartmentHistoryParameters deptHistoryParameters)
        {
            var deptHistories = await _repository.DepartmentHistory.GetDepartmentHistories(employeeID, deptHistoryParameters);

            var metadata = new
            {
                deptHistories.TotalCount,
                deptHistories.PageSize,
                deptHistories.CurrentPage,
                deptHistories.TotalPages,
                deptHistories.HasNext,
                deptHistories.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(deptHistories);
        }

        [HttpGet("depthistory/{employeeID}/{deptID}/{shiftID}/{startDate}", Name = "GetDeptHistoryByID")]
        public async Task<IActionResult> GetDeptHistoryByID(int employeeID, short deptID, byte shiftID, DateTime startDate)
        {
            var depthistory = await _repository.DepartmentHistory.GetDepartmentHistoryByID(employeeID, deptID, shiftID, startDate);
            if (depthistory == null)
            {
                return NotFound();
            }

            return Ok(depthistory);
        }

        [HttpPost("depthistory")]
        public IActionResult CreateDeptHistory([FromBody] EmployeeDepartmentHistory deptHistory)
        {
            _repository.DepartmentHistory.CreateDepartmentHistory(deptHistory);
            return CreatedAtRoute(nameof(GetDeptHistoryByID),
                                  new
                                  {
                                      employeeID = deptHistory.BusinessEntityID,
                                      deptID = deptHistory.DepartmentID,
                                      shiftID = deptHistory.ShiftID,
                                      startDate = deptHistory.StartDate
                                  },
                                  deptHistory);
        }

        [HttpPut("depthistory")]
        public IActionResult UpdateDeptHistory([FromBody] EmployeeDepartmentHistory deptHistory)
        {
            _repository.DepartmentHistory.UpdateDepartmentHistory(deptHistory);
            return NoContent();
        }

        [HttpDelete("depthistory")]
        public IActionResult DeleteDeptHistory([FromBody] EmployeeDepartmentHistory deptHistory)
        {
            _repository.DepartmentHistory.DeleteDepartmentHistory(deptHistory);
            return NoContent();
        }
    }
}