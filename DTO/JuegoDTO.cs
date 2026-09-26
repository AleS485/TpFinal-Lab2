using System.ComponentModel.DataAnnotations;

namespace TpFinal_Lab2.DTO;

public class JuegoDTO
{
    [Required(ErrorMessage = "El titulo es obligatorio")]
    [StringLength(150, ErrorMessage = "El titulo no puede superar los 150 caracteres")]
    public required string? Titulo { get; set; }

    [Required(ErrorMessage = "El genero es obligatorio")]
    [StringLength(50, ErrorMessage = "El genero no puede superar los 50 caracteres")]
    public required string? Genero { get; set; }

    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0, 9999999.99, ErrorMessage = "El precio debe ser un valor positivo")]
    public decimal Precio { get; set; }

    [Required(ErrorMessage = "El stock es obligatorio")]
    [Range(0, 100000, ErrorMessage = "El stock debe estar entre 0 y 100.000")]
    public required int Stock { get; set; }

    public bool Estado { get; set; } = true;

    [Required(ErrorMessage = "La desarrolladora es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una desarrolladora que exista")]
    public required int DesarrolladoraId { get; set; }
}