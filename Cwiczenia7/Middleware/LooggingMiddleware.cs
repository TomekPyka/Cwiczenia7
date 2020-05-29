using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cwiczenia7.Middleware
{
    public class LooggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LooggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            PathString path = context.Request.Path;
            string method = context.Request.Method;
            QueryString queryString = context.Request.QueryString;
            string body = "";
            using StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true);
            body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;
            var lines = new List<string>
                {
                    "Data: " + DateTime.Now.ToString(),
                    "Metoda: " + method,
                    "Ścieżka: " + path,
                    "QueryString: " + queryString,
                    "Body:\n" + body
                };
            await File.AppendAllLinesAsync("requestsLog.txt ", lines, Encoding.UTF8);

            await _next(context);
        }
    }
}