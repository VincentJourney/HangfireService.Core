using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using HangfireService.Model;
using HangfireService.Business.CrmPlatForm.Infrastructure;
using Hangfire.Logging;
using Microsoft.Extensions.Logging;
using HangfireService.HttpJob;

namespace HangfireService.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public JobController(ILogger<JobController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        [Route("AddRecurringJob")]
        public IActionResult AddRecurringJob([FromBody] AddJobModel jobModel)
        {
            try
            {
                RecurringJob.AddOrUpdate(jobModel.Name,
                                         () => HangfireService.HttpJob.HttpJobExcuter.PlatformExcute(jobModel.Url, jobModel.Data),
                                         jobModel.Corn,
                                         TimeZoneInfo.Local);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


    }
}
