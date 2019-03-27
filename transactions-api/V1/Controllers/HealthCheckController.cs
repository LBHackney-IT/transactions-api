using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using transactions_api.UseCase.V1;

namespace transactions_api.Controllers.V1
{
    [ApiVersion("1")]
    [Route("api/v1/healthcheck")]
    [ApiController]
    [Produces("application/json")]
    public class HealthCheckController : BaseController
    {
        /// <summary>Returns a static success = true message used for monitoring</summary>
        [HttpGet]
        [Route("ping")]
        [ProducesResponseType(typeof(Dictionary<string, bool>), 200)]
        public IActionResult HealthCheck()
        {
            var result = new Dictionary<string, bool> {{"success", true}};

            return Ok(result);
        }

        /// <summary>Throws a TestOpsErrorException for testing purposes</summary>
        [HttpGet]
        [Route("error")]
        [ProducesResponseType(typeof(TestOpsErrorException), 500)]
        public void ThrowError()
        {
            ThrowOpsErrorUsecase.Execute();
        }

    }
}
