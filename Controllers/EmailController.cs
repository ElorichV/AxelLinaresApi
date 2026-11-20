using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AxelLinaresApi.Models;

namespace AxelLinaresApi.Controllers
{
    /// <summary>
    /// Controlador responsable del envío de mensajes desde el formulario de contacto.
    /// Soporta despacho agéntico hacia n8n webhook con fallback a servidor SMTP.
    /// Protegido con Rate Limiting nativo de .NET 8 y validación estricta (Data Annotations + IValidatableObject).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [EnableRateLimiting("EmailRateLimitPolicy")]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailController> _logger;
        private readonly IHttpClientFactory? _httpClientFactory;

        public EmailController(
            IConfiguration config, 
            ILogger<EmailController> logger,
            IHttpClientFactory? httpClientFactory = null)
        {
            _config = config;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("send")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
        public async Task<IActionResult> SendEmailAsync([FromBody] ContactRequest request, CancellationToken cancellationToken)
        {
            // 1. Validación estricta con Data Annotations e IValidatableObject (RFC 7807)
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            // 2. Mitigación AppSec: Prevención de SMTP Header Injection (CRLF) y sanitización
            var sanitizedName = request.Name.Replace("\r", "").Replace("\n", "").Trim();
            var sanitizedEmail = request.Email.Replace("\r", "").Replace("\n", "").Trim();

            // 3. Intento primario: Despacho a n8n Webhook si está configurado
            var n8nWebhookUrl = _config["N8nSettings:ContactWebhookUrl"] 
                                ?? Environment.GetEnvironmentVariable("N8N_CONTACT_WEBHOOK_URL");

            if (!string.IsNullOrWhiteSpace(n8nWebhookUrl) && _httpClientFactory != null)
            {
                try
                {
                    var httpClient = _httpClientFactory.CreateClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(10);

                    var payload = new
                    {
                        name = sanitizedName,
                        email = sanitizedEmail,
                        message = request.Message,
                        submittedAt = DateTime.UtcNow,
                        source = "Axel Linares Portfolio Web"
                    };

                    var n8nResponse = await httpClient.PostAsJsonAsync(n8nWebhookUrl, payload, cancellationToken);
                    if (n8nResponse.IsSuccessStatusCode)
                    {
                        _logger.LogInformation("Mensaje de contacto despachado exitosamente a n8n para {Email}", sanitizedEmail);
                        return Ok(new { message = "Mensaje procesado y despachado exitosamente." });
                    }

                    _logger.LogWarning("Webhook de n8n retornó HTTP {StatusCode}. Intentando envío alternativo...", n8nResponse.StatusCode);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Falla en la comunicación con el webhook de n8n. Procediendo con fallback SMTP si está disponible.");
                }
            }

            // 4. Fallback: Servidor SMTP tradicional (Gmail / AWS SES)
            var senderEmail = _config["EmailSettings:SenderEmail"];
            var senderPassword = _config["EmailSettings:SenderPassword"];

            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(senderPassword))
            {
                _logger.LogWarning("Configuración de servidor SMTP y n8n ausente o incompleta.");
                return Problem(
                    statusCode: StatusCodes.Status503ServiceUnavailable,
                    type: "https://tools.ietf.org/html/rfc7231#section-6.6.4",
                    title: "Servicio de correo no disponible",
                    detail: "El servicio de mensajería no se encuentra disponible temporalmente por configuración.");
            }

            try
            {
                using var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                };

                using var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "Portfolio Contact Notification"),
                    Subject = $"Nuevo mensaje de portafolio de: {sanitizedName}",
                    Body = $"Has recibido un nuevo mensaje de contacto desde tu Portfolio.\n\n" +
                           $"Nombre: {sanitizedName}\n" +
                           $"Correo: {sanitizedEmail}\n" +
                           $"Mensaje:\n{request.Message}",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(senderEmail);
                mailMessage.ReplyToList.Add(new MailAddress(sanitizedEmail, sanitizedName));

                // E/S asíncrona no bloqueante
                await smtpClient.SendMailAsync(mailMessage, cancellationToken);

                _logger.LogInformation("Mensaje de contacto enviado exitosamente vía SMTP de {Email}", sanitizedEmail);

                return Ok(new { message = "Correo enviado exitosamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar el correo de contacto desde {Email}", sanitizedEmail);

                // Cumplimiento estricto RFC 7807: No exponer detalles internos de infraestructura
                return Problem(
                    statusCode: StatusCodes.Status500InternalServerError,
                    type: "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    title: "Error interno al enviar el correo",
                    detail: "Ocurrió un problema de comunicación al intentar enviar tu mensaje. Por favor, intenta de nuevo más tarde.");
            }
        }
    }
}
