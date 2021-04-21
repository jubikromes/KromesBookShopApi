using Mailing.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BookApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {


        public BaseApiController()
        {
        }

      


        protected async Task<ApiResponse<T>> HandleApiOperationAsync
            <T>
            (
           Func<Task<ApiResponse<T>>> action,
           [CallerLineNumber] int lineNo = 0,
           [CallerMemberName] string method = "")
        {
            var apiResponse = new ApiResponse<T>
            {
                Code = StatusCodes.Status200OK
            };

            try
            {
                var methodResponse = await action.Invoke();

                apiResponse.ResponseCode = methodResponse.ResponseCode;
                apiResponse.Payload = methodResponse.Payload;
                apiResponse.Code = methodResponse.Code;
                apiResponse.Errors = methodResponse.Errors;
                apiResponse.Description = string.IsNullOrEmpty(apiResponse.Description) ? methodResponse.Description : apiResponse.Description;
                return apiResponse;
            }
            catch (Exception ex)
            {
                apiResponse.Code = StatusCodes.Status500InternalServerError;

#if DEBUG
                apiResponse.Description = $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}";
#else
                apiResponse.Description = $"Error: {(ex?.InnerException?.Message ?? ex.Message)} --> {ex?.StackTrace}";

                //apiResponse.Description = "An error occurred while processing your request!";
#endif
                apiResponse.Errors.Add(apiResponse.Description);
                return apiResponse;
            }
        }
    }
}
