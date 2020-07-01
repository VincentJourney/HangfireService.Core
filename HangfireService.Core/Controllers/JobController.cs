using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hangfire;
using HangfireService.Model;
using HangfireService.Business.CrmPlatForm.Infrastructure;

namespace HangfireService.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost]
        [Route("AddRecurringJob")]
        public IActionResult AddRecurringJob([FromBody] AddJobModel jobModel)
        {
            try
            {
                RecurringJob.AddOrUpdate(jobModel.Name,
                                         () => PlatformExcute(jobModel.Url, jobModel.Data),
                                         jobModel.Corn,
                                         TimeZoneInfo.Local);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        public static object PlatformExcute(string Url, object data)
        {
            return PlatformCommunication.PostCRM<object>(Url, data);
        }
    }
}
