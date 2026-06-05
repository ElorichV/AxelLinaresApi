using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using AxeLinaresapi.Models;

namespace AxeLinaresapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IConfiguration _config;

        public EmailController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("send")]
        public IActionResult SendEmail([FromBody] ContactRequest request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Message))
            {
                return BadRequest(new { message = "El correo y el mensaje son obligatorios." });
            }

            try
            {
                // Lectura segura de credenciales desde IConfiguration
                var senderEmail = _config["EmailSettings:SenderEmail"];
                var senderPassword = _config["EmailSettings:SenderPassword"];

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = $"Nuevo mensaje de portafolio de: {request.Name}",
                    Body = $"Has recibido un nuevo mensaje de contacto desde tu Portfolio.\n\nNombre: {request.Name}\nCorreo: {request.Email}\nMensaje:\n{request.Message}",
                    IsBodyHtml = false,
                };

                // Te envía el correo a tu propia bandeja
                mailMessage.To.Add(senderEmail);

                smtpClient.Send(mailMessage);

                return Ok(new { message = "Correo enviado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno al enviar el correo.", error = ex.Message });
            }
        }
    }
}