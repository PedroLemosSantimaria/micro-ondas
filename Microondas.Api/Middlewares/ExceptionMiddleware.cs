using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microondas.Core.Exceptions;

namespace Microondas.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string errorLogFilePath;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;

            var dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            errorLogFilePath = Path.Combine(dataFolder, "errors.txt");
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (BusinessException ex)
            {
                await WriteLog(ex);

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = ex.Message,
                    type = "business_error"
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                await WriteLog(ex);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    success = false,
                    message = "Ocorreu um erro interno no servidor.",
                    type = "server_error"
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }

        private async Task WriteLog(Exception ex)
        {
            var sb = new StringBuilder();

            sb.AppendLine("=======================================");
            sb.AppendLine("Data: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            sb.AppendLine("Exception: " + ex.Message);

            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception: " + ex.InnerException.Message);
            }

            sb.AppendLine("StackTrace:");
            sb.AppendLine(ex.StackTrace);
            sb.AppendLine();

            await File.AppendAllTextAsync(errorLogFilePath, sb.ToString());
        }
    }
}