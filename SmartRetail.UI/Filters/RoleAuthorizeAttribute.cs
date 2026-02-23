using System.Web;
using System.Web.Mvc;

namespace SmartRetail.UI.Filters
{
    public class RoleAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string _role;

        public RoleAuthorizeAttribute(string role)
        {
            _role = role;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var userRole = httpContext.Session["Role"]?.ToString();

            if (string.IsNullOrEmpty(userRole))
                return false;

            return userRole == _role;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/Account/Login");
        }
    }
}