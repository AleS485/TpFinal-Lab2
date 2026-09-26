using System.ComponentModel.DataAnnotations;

namespace TpFinal_Lab2.DTO;

public class ResenaDTO
{
    [Required(ErrorMessage = "La calificacion es obligatoria")]
    [Range(1, 5, ErrorMessage = "La calificacion debe ser un valor entre 1 y 5 estrellas")]
    public required int Calificacion { get; set; }

    public string? Comentario { get; set; }

    [Required(ErrorMessage = "El juego es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El juego no es valido")]
    public required int JuegoId { get; set; }

    [Required(ErrorMessage = "El cliente es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El cliente no es valido")]
    public required int ClienteId { get; set; }
}