using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Services.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class HasAccess : AuthorizeAttribute, IAuthorizationFilter
    {
        private string[] permissions;

        public HasAccess(params string[] permissions)
        {
            this.permissions = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            ClaimsIdentity identity = user.Identity as ClaimsIdentity;

            if (!user.Identity.IsAuthenticated) return;
            if (!user.HasClaim(c => c.Type == ClaimTypes.Role)) return;
            IRoleService roleService = (IRoleService)context.HttpContext.RequestServices.GetService(typeof(IRoleService));

            //roleService.checkPermission(user.)


        }
    }

}
