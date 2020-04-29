using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using transactions_api.V1.Boundary;
using transactions_api.V1.Exceptions;
using transactions_api.V1.Validation;

namespace transactions_api.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v1")] // the tenancy details endpoint screw this up a bit for now..
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
        [Route("transactions")]
        public JsonResult GetTransactions([FromQuery]ListTransactionsRequest request)
        {
            _logger.LogInformation("Transactions requested for TagRef: " + request.TagRef);
            var usecaseResponce = _listTransactions.Execute(request);
            return new JsonResult(usecaseResponce) {StatusCode = 200};
        }

        [HttpGet]
        [Route("transactions/payment-ref/{payment_ref}/post-code/{post_code}")]                                              //should we add "GetAllTenancyTransactions/" to the start of the url?
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetAllTenancyTransactionsResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        [ProducesResponseType(typeof(ErrorResponse), 500)]
        public IActionResult GetAllTenancyTransactions([FromRoute] GetAllTenancyTransactionsRequest request)
        {
            _logger.LogInformation(                                                                             //TODO: Add tests for logging! An the rest of the logging. Add logging message formatter.
                $"The request has hit GetAllTenancyTransactions controller with the following data. PaymentRef = {request.PaymentRef ?? "null" } and PostCode = {request.PostCode ?? "null"}"
                );

            try                                                                                                 //TODO: add tests for this. No tests due to this needing to be rushed!
            {
                var validationResult = _getTenancyTransactionsValidator.Validate(request);

                if (validationResult.IsValid)
                {
                    var usecaseResponse = _listTransactions.ExecuteGetTenancyTransactions(request);

                    return Ok(usecaseResponse);
                }

                return BadRequest(
                    new ErrorResponse(validationResult.Errors)
                    );
            }
            catch (AggregateException ex) when ( ex.InnerException != null )
            {
                return StatusCode(
                    500,
                    new ErrorResponse(ex.Message, ex.InnerException.Message)
                    );
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new ErrorResponse(ex.Message)
                    );
            }
        }

        [HttpGet]
        [Route("tenancy-details/payment-ref/{payment_ref}/post-code/{post_code}")]                                              //should we add "GetAllTenancyTransactions/" to the start of the url?
        [Produces("application/json")]
        [ProducesResponseType(typeof(GetTenancyDetailsResponse), 200)]
        public IActionResult GetTenancyDetails([FromRoute] GetTenancyDetailsRequest request)
        {
                    var usecaseResponse = _listTransactions.ExecuteGetTenancyDetails(request);
                        return Ok(usecaseResponse);
        }
    }
}
