using Cwiczenia7.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cwiczenia7.Middleware
{
    public class IndexMiddleware
    {
        private readonly RequestDelegate _next;

        public IndexMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IStudentDbService studentDbService)
        {
            if (!context.Request.Headers.ContainsKey("Index"))
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Brak indeksu");
                return;
            }
            var index = context.Request.Headers["Index"].ToString();
            if (studentDbService.GetStudent(index) == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                await context.Response.WriteAsync("Brak studenta w bazie z danym numerem indeksu");
                return;
            }

            await _next(context);
        }
    }
}