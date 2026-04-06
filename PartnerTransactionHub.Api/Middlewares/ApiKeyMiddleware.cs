using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using PartnerTransactionHub.Api.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PartnerTransactionHub.Api.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiKey;
        private const string APIKEY_HEADER = "X-API-KEY";
        //private const string VALID_API_KEY = "super-secret-key";

        public ApiKeyMiddleware(RequestDelegate next, IOptions<ApiKeySettings> options)
        {
            _next = next;
            _apiKey = options.Value.ApiKey;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.StartsWithSegments("/api"))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(APIKEY_HEADER, out var extractedKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("API Key missing");
                return;
            }

            if (!_apiKey.Equals(extractedKey))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.Response.WriteAsync("Invalid API Key");
                return;
            }

            await _next(context);
        }
    }
}
