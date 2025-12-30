using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCore.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Ocurrió una excepción no controlada: {Message}", exception.Message);

            var problemDetails = new ProblemDetails
            {
                Instance = httpContext.Request.Path
            };

            if (exception is ValidationException validationException)
            {
                // Si es error de validación (FluentValidation), devolvemos 400 Bad Request
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Title = "Error de Validación";
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Detail = "Uno o más campos no cumplieron las reglas de validación.";

                // Agregamos los errores específicos a la respuesta
                problemDetails.Extensions["errors"] = validationException.Errors
                    .Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage });
            }
            else
            {
                // Si es cualquier otro error, devolvemos 500 (pero ocultamos los detalles técnicos en Producción)
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                problemDetails.Title = "Error Interno del Servidor";
                problemDetails.Status = StatusCodes.Status500InternalServerError;
                problemDetails.Detail = "Ha ocurrido un error inesperado. Por favor contacte al soporte.";
            }

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Le decimos a .NET que ya manejamos el error
        }
    }
}
