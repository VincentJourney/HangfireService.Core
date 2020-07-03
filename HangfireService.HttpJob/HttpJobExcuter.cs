using HangfireService.Business.CrmPlatForm.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace HangfireService.HttpJob
{
    public class HttpJobExcuter
    {
        public static object PlatformExcute(string Url, object data)
        {
            try
            {
                return PlatformCommunication.PostCRM<object>(Url, data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
