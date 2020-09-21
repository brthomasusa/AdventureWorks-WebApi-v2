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
    public class PayHistoryController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryCollection _repository;

        public PayHistoryController(ILoggerManager logger, IRepositoryCollection repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{employeeID}/payhistory")]
        public async Task<IActionResult> GetPayHistories(int employeeID, [FromQuery] PayHistoryParameters payHistoryParameters)
        {
            var payHistories = await _repository.PayHistory.GetPayHistories(employeeID, payHistoryParameters);

            var metadata = new
            {
                payHistories.TotalCount,
                payHistories.PageSize,
                payHistories.CurrentPage,
                payHistories.TotalPages,
                payHistories.HasNext,
                payHistories.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(payHistories);
        }


        [HttpGet("payhistory/{employeeID}/{rateChangeDate}", Name = "GetPayHistoryByID")]
        public IActionResult GetPayHistoryByID(int employeeID, DateTime rateChangeDate)
        {
            var payhistory = _repository.PayHistory.GetPayHistoryByID(employeeID, rateChangeDate);

            if (payhistory == null)
            {
                return NotFound();
            }

            return Ok(payhistory);
        }


        [HttpPost("payhistory")]
        public IActionResult CreatePayHistory([FromBody] EmployeePayHistory payHistory)
        {
            _repository.PayHistory.CreatePayHistory(payHistory);
            return CreatedAtRoute(nameof(GetPayHistoryByID), new { employeeID = payHistory.BusinessEntityID, rateChangeDate = payHistory.RateChangeDate }, payHistory);
        }


        [HttpPut("payhistory")]
        public IActionResult UpdatePayHistory([FromBody] EmployeePayHistory payHistory)
        {
            _repository.PayHistory.UpdatePayHistory(payHistory);
            return NoContent();
        }


        [HttpDelete("payhistory")]
        public IActionResult DeletePayHistory([FromBody] EmployeePayHistory payHistory)
        {
            _repository.PayHistory.DeletePayHistory(payHistory);
            return NoContent();
        }
    }
}