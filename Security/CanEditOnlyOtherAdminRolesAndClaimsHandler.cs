using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StudentManagement.Security
{
    public class CanEditOnlyOtherAdminRolesAndClaimsHandler: AuthorizationHandler<ManageAdminRolesAndClaimsRequirement>
    {

        private readonly IHttpContextAccessor contextAccessor;

        public CanEditOnlyOtherAdminRolesAndClaimsHandler(IHttpContextAccessor contextAccessor)
        {
            this.contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            ManageAdminRolesAndClaimsRequirement requirement)
        {
            var authFilterContext = context.Resource as AuthorizationFilterContext;

            string loggedInAdminId =
                context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            //string adminIdBeingEdited = authFilterContext.HttpContext.Request.Query["userId"];

            string adminIdBeingEdited = contextAccessor.HttpContext.Request.Query["userId"];
            //Our requirement is met and the authorization succeeds If the user is in the Admin role 
            //AND has Edit Role claim type with a claim value of true AND the logged-in user Id is NOT
            //EQUAL TO the Id of the Admin user being edited
            if (context.User.IsInRole("Admin") &&
                context.User.HasClaim(claim => claim.Type == "Edit Role" && claim.Value == "true") &&
                adminIdBeingEdited.ToLower() != loggedInAdminId.ToLower())
            {
                context.Succeed(requirement);
            }
            //else
            //{
            //    context.Fail(); //Failure takes precedence over success. When one of the handlers return failure,
            //                    //the policy fails even if the other handlers return successBy default, all handlers are called,
            //                    //irrespective of what a handler returns (success, failure or nothing)
            //}

            return Task.CompletedTask;
        }
    }
}
