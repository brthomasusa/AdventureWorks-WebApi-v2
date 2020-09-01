using System;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.HumanResources;
using LoggerService;

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




        // [HttpGet("depthistory/{employeeID}/{rateChangeDate}", Name = "GetDeptHistoryByID")]
        // public IActionResult GetDeptHistoryByID(int employeeID, DateTime rateChangeDate)
        // {
        //     // var depthistory = _repository.PayHistory
        // }







    }
}