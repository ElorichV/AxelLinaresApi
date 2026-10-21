using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AxelLinaresApi.Models
{
    /// <summary>
    /// DTO para la recepción de solicitudes de contacto.
    /// Implementa validación estricta con Data Annotations e IValidatableObject para AppSec (prevención de CRLF Injection).
    /// </summary>
    public class ContactRequest : IValidatableObject
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜ\s\.\-]+$", ErrorMessage = "El nombre contiene caracteres inválidos.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        [StringLength(254, ErrorMessage = "El correo electrónico excede la longitud máxima permitida (RFC 5321).")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje es obligatorio.")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "El mensaje debe tener entre 10 y 2000 caracteres.")]
        public string Message { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name.Contains('\r') || Name.Contains('\n'))
            {
                yield return new ValidationResult(
                    "El nombre no puede contener saltos de línea (anti CRLF Injection).",
                    new[] { nameof(Name) });
            }

            if (Email.Contains('\r') || Email.Contains('\n'))
            {
                yield return new ValidationResult(
                    "El correo electrónico no puede contener saltos de línea.",
                    new[] { nameof(Email) });
            }

            if (string.IsNullOrWhiteSpace(Message) || Message.Trim().Length < 10)
            {
                yield return new ValidationResult(
                    "El mensaje debe contener al menos 10 caracteres significativos (no solo espacios en blanco).",
                    new[] { nameof(Message) });
            }
        }
    }
}
