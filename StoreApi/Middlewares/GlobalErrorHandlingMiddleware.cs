using Domain.Exceptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.ErrorModels;
using System.Net;

namespace StoreApi.Middlewares
{
    public class GlobalErrorHandlingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger< GlobalErrorHandlingMiddleware> logger ) 
        {
           _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound)

                 await  HandleNotFoundEndPointAsync(httpContext);
            }

            catch (Exception ex)
            {
                _logger.LogError($"somting went wrong {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {

            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            httpContext.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
               
                
                
                ErrorMessage = $"The End Point {httpContext.Request.Path} NotFound ",
                     StatuCode = (int)HttpStatusCode.NotFound,
           
               


            }.ToString();
              
            await httpContext.Response.WriteAsync(response);

        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex) 
            {
           
            httpContext.Response.StatusCode=(int)HttpStatusCode.InternalServerError;
           
            httpContext.Response.ContentType = "application/json";

            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message,

              


            };

            

            httpContext.Response.StatusCode = ex switch
            {
                UnAuthorizedException=>(int)HttpStatusCode.Unauthorized,
                NotFoundException => (int)HttpStatusCode.NotFound,
                ValidationException  validationException=> HandelValidationException(validationException,response),
                _ => (int)HttpStatusCode.InternalServerError

            }; 
            response.StatuCode = httpContext.Response.StatusCode;
            await httpContext.Response.WriteAsync(response.ToString());



            }
        private int HandelValidationException (ValidationException validationException, ErrorDetails errorDetails) 
        {
            errorDetails.Errors =validationException.Errors;

            return (int)HttpStatusCode.BadRequest;
        }
        
    }
}
