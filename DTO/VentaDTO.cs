using System.ComponentModel.DataAnnotations;

namespace TpFinal_Lab2.DTO;

public class VentaDTO
{
    [Required(ErrorMessage = "Debe indicar el juego")]
    [Range(1, int.MaxValue, ErrorMessage = "Juego no valido")]
    public required int JuegoId { get; set; }

    [Required(ErrorMessage = "Debe indicar el cliente")]
    [Range(1, int.MaxValue, ErrorMessage = "Cliente no valido")]
    public required int ClienteId { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria")]
    [Range(1, 100, ErrorMessage = "La cantidad debe ser de al menos 1 unidad")]
    public required int Cantidad { get; set; } = 1;
}