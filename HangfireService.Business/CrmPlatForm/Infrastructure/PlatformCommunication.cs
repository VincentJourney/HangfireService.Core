using System;
using System.Collections.Generic;
using System.Text;
using HangfireService.Commom;
using HangfireService.Model;
using Newtonsoft.Json;

namespace HangfireService.Business.CrmPlatForm.Infrastructure
{
    public class PlatformCommunication
    {
        private static string Token;

        private static string GetToken()
        {
            var url = $"{ConfigUtil.CRMUrl}/api/platform/token?appid={ConfigUtil.CRMTokenAppid}&secret={ConfigUtil.CRMTokenSecret}";
            var res = HttpHelper.HttpGet(url);
            ResponseModel<string> r = null;
            try
            {
                r = JsonConvert.DeserializeObject<ResponseModel<string>>(res);
            }
            catch { r = null; }
            if (r != null) if (r.Result != null) if (r.Result.HasError) throw new Exception(r.Result.ErrorMessage);
            return res;
        }

        public static ResponseModel<Response> PostCRM<Response>(string url, object model)
        {
            if (string.IsNullOrWhiteSpace(Token))
                Token = GetToken();
            string res = Post(url, JsonConvert.SerializeObject(model));
            var r = JsonConvert.DeserializeObject<ResponseModel<Response>>(res);
            if (r.Result != null) if (r.Result.HasError) if (r.Result.ErrorCode == "401") //Invalid Token or expired.
                    {
                        Token = GetToken();
                        res = Post(url, JsonConvert.SerializeObject(model));
                        r = JsonConvert.DeserializeObject<ResponseModel<Response>>(res);
                    }
            return r;
        }

        /// <summary>
        /// 调用中台接口
        /// </summary>
        /// <param name="route">接口路径</param>
        /// <param name="postData">传入参数</param>
        /// <returns></returns>
        private static string Post(string route, string postData)
        {
            string posturl = ConfigUtil.CRMUrl + "/" + route + "?token=" + Token;
            var result = HttpHelper.HttpPost(posturl, postData, "application/json");
            return result;
        }


    }
}
