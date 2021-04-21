using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kromes.Core.ViewModels
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public bool HasErrors => Errors.Any();
    }
    
    public class ApiResponse<T> : ApiResponse
    {
        public T Payload { get; set; } = default;
        public string ResponseCode { get; set; }


        public ApiResponse(T data = default, string message = "",
            int codes = StatusCodes.Status200OK,  params string[] errors)
        {
            Payload = data;
            Errors = errors.ToList();
            Code = !errors.Any() ? codes : codes == StatusCodes.Status200OK ? StatusCodes.Status500InternalServerError : codes;
            Description = message;
        }
    }
}
