using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExampleApp.Api.Controllers;

public partial class StudentsController
{
    public class ApiResponseModel<T> : ActionResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public string Message { get; internal set; }
    }
}
