﻿using MvcMCBA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

    namespace MvcMCBA.Authorise
    {
    public class SecureContentAttribute : Attribute, IAuthorizationFilter
    { 
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var customerID = context.HttpContext.Session.GetInt32("CustomerID");
            if (!customerID.HasValue)
                context.Result = new RedirectToActionResult("Index", "Login",null);
        }
    }
}
