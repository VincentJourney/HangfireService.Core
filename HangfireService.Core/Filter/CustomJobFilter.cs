using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireService.Core.Filter
{
    public class CustomJobFilter : JobFilterAttribute, IClientFilter, IServerFilter, IElectStateFilter
    {
       // private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        /// <summary>
        /// 创建任务前
        /// </summary>
        /// <param name="context"></param>
        public void OnCreating(CreatingContext context)
        {

        }
        /// <summary>
        /// 创建完成
        /// </summary>
        /// <param name="context"></param>
        public void OnCreated(CreatedContext context)
        {

        }
        /// <summary>
        /// 开始执行任务
        /// </summary>
        /// <param name="context"></param>
        public void OnPerforming(PerformingContext context)
        {

        }
        /// <summary>
        /// 执行完毕
        /// </summary>
        /// <param name="context"></param>
        public void OnPerformed(PerformedContext context)
        {
           
        }
        /// <summary>
        /// 异常
        /// </summary>
        /// <param name="context"></param>
        public void OnStateElection(ElectStateContext context)
        {
            //
            //var failedState = context.CandidateState as FailedState;
            //if (failedState != null)
            //{
            //    Logger.WarnFormat(
            //        "Job `{0}` has been failed due to an exception `{1}`",
            //        context.BackgroundJob.Id,
            //        failedState.Exception);
            //}
        }

    }
}
