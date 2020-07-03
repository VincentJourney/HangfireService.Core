using System;
using System.Collections.Generic;
using System.Text;
using HangfireService.Model;
using HangfireService.Business.CrmPlatForm.Infrastructure;

namespace HangfireService.Business.CrmPlatForm.Job
{
    public class OrderJob
    {
        /// <summary>
        /// 订单取消
        /// </summary>
        public static ResponseModel<bool?> OrderCancelTask() => CallCRMApi("OrderCancelTask");

        /// <summary>
        /// 订单退款
        /// </summary>
        public static ResponseModel<bool?> OrderReturnTask() => CallCRMApi("OrderReturnTask");

        private static ResponseModel<bool?> CallCRMApi(string ApiName)
        {
            try
            {
                var ClientUrl = $"api/task/{ApiName}";
                var param = new RequestModel<int>
                {
                    Shared = new Shared(),
                    Data = 1
                };
                var result = PlatformCommunication.PostCRM<bool?>(ClientUrl, param);
                if (result == null || result.Result == null)
                    throw new Exception("服务请求失败，请查看中台");
                if (result.Result.HasError)
                    throw new Exception($"Code:{result.Result.ErrorCode},Message:{result.Result.ErrorMessage}");
                if (!result.Data.HasValue || !result.Data.Value)
                    throw new Exception("接口执行失败");

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
