using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using transactions_api.V1.Boundary;
using transactions_api.V1.Exceptions;
using transactions_api.V1.Validation;

namespace transactions_api.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v1/transactions")]
    [ApiController]
    [Produces("application/json")]
    public class TransactionsController : BaseController
    {
        private readonly IListTransactions _listTransactions;
        private readonly ILogger<TransactionsController> _logger;
        private readonly IGetTenancyTransactionsValidator _getTenancyTransactionsValidator;

        public TransactionsController(IListTransactions listTransactions, ILogger<TransactionsController> logger, IGetTenancyTransactionsValidator getTenancyTransactionsValidator)
        {
            _listTransactions = listTransactions;
            _logger = logger;
            _getTenancyTransactionsValidator = getTenancyTransactionsValidator;
        }

        [ProducesResponseType(typeof(ListTransactionsResponse), 200)]
        [HttpGet]
        public JsonResult GetTransactions([FromQuery]ListTransactionsRequest request)
        {
            _logger.LogInformation("Transactions requested for TagRef: " + request.TagRef);
            var usecaseResponce = _listTransactions.Execute(request);
            return new JsonResult(usecaseResponce) {StatusCode = 200};
        }

        [HttpGet]
        [Route("payment-ref/{payment_ref}/post-code/{post_code}")] //should we add "GetAllTenancyTransactions/" to the start of the url?
        [Produces("application/json")]
        public IActionResult GetAllTenancyTransactions([FromRoute] GetAllTenancyTransactionsRequest request)
        {
            var validationResult = _getTenancyTransactionsValidator.Validate(request);
            
            if (validationResult.IsValid)
            {
                return Ok(new { }); // had to use anonymous object, because if no object is put inside Ok(), it will produce OkResult, not OkObjectResult, which we want down the line...
            }

            return BadRequest(
                    new ErrorResponse(validationResult.Errors)
                );
        }
    }
}
