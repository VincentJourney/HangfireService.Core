using System;
using System.Collections.Generic;
using System.Text;

namespace HangfireService.Model
{
    public class AddJobModel
    {
        public string Method { get; set; } = "POST";
        public string ContentType { get; set; } = "application/json";
        public string Url { get; set; }
        public object Data { get; set; }
        public string Corn { get; set; }
        public string Name { get; set; }
    }
    public class JobRes
    {
        public string Message { get; set; }
        public int Code { get; set; }
    }

}
