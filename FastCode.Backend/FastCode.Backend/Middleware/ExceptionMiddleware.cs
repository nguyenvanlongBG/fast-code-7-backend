using FastCode.Backend.Core.MException;
using System.Net;
using System.Runtime.InteropServices;

namespace FastCode.Backend.Middleware
{
    /// <summary>
    /// Middleware bắt lỗi
    /// Created By: nguyenvanlongBG (3/7/2023)
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            // Kiểm tra có phải lỗi không tìm thấy
            if (exception is NotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                NotFoundException notFoundException = (NotFoundException)exception;
                await context.Response.WriteAsync(
                    text: new BaseException()
                    {
                        ErrorCode = notFoundException.ErrorCode,
                        UserMessage = notFoundException.UserMessage,
                        DevMessage = notFoundException.DevMessage,
                        TraceId = context.TraceIdentifier,
                        MoreInfo = exception.HelpLink
                    }.ToString() ?? ""
                    );
                // Kiểm tra lỗi có phải lỗi Validate
            }
            else if (exception is ValidateException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ValidateException validateException = (ValidateException)exception;
                await context.Response.WriteAsync(text: new BaseException()
                {
                    ErrorCode = validateException.ErrorCode,
                    UserMessage = validateException.UserMessage,
                    DevMessage = validateException.DevMessage,
                    TraceId = context.TraceIdentifier,
                    MoreInfo = exception.HelpLink
                }.ToString() ?? "");
                // Kiểm tra phải lỗi nội bộ
            }
            else if (exception is InternalException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                List<string> messages = new List<string>();
                messages.Add(exception.Message);
                await context.Response.WriteAsync(text: new BaseException()
                {
                    ErrorCode = context.Response.StatusCode,
                    UserMessage = messages,
                    DevMessage = messages,
                    TraceId = context.TraceIdentifier,
                    MoreInfo = exception.HelpLink
                }.ToString() ?? "");
            }
            else
            // Kiểm tra lỗi chung
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                List<string> userMessages = new List<string>();
                List<string> devMessages = new List<string>();
                userMessages.Add("Có lỗi xảy ra vui lòng liên hệ MISA để được trợ giúp");
                devMessages.Add(exception.Message);
                await context.Response.WriteAsync(text: new BaseException()
                {
                    ErrorCode = context.Response.StatusCode,
                    UserMessage = userMessages,
                    DevMessage = devMessages,
                    TraceId = context.TraceIdentifier,
                    MoreInfo = exception.HelpLink
                }.ToString() ?? "");
            }
        }
    }
}
