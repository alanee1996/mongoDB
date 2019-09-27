using Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;
using Services.IServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
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
            if (!user.Identity.IsAuthenticated) return;
            if (!user.HasClaim(c => c.Type == ClaimTypes.Role)) return;
            IRoleService roleService = (IRoleService)context.HttpContext.RequestServices.GetService(typeof(IRoleService));
            var condition = roleService.checkPermission(user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value, permissions).Result;
            if (!condition)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                context.Result = new JsonResult(JsonResponse.failed("Access denied, you do not have the permission to access the link", (int)HttpStatusCode.Forbidden));
            }

        }
    }

}
