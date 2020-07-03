using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangfireService.Core
{
    public class CustomAuthorizeFilter : IDashboardAuthorizationFilter
    {
        private List<AuthorizeUser> _authorizeUser;
        public CustomAuthorizeFilter(List<AuthorizeUser> authorizeUser)
        {
            _authorizeUser = authorizeUser;
        }
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpcontext = context.GetHttpContext();
           // return httpcontext.User.Identity.IsAuthenticated;
            return true;
        }

    }

    public class AuthorizeUser
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
