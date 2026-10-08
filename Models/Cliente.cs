namespace TpFinal_Lab2.Models;
using System.ComponentModel.DataAnnotations;

public class Cliente
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "El DNI es obligatorio")]
    [StringLength(20, ErrorMessage = "El DNI no puede superar los 20 caracteres")]
    public string? Dni { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "El apellido es obligatorio")]
    [StringLength(100, ErrorMessage = "El apellido no puede superar los 100 caracteres")]
    public string? Apellido { get; set; }

    [StringLength(50, ErrorMessage = "El telefono no puede superar los 50 caracteres")]
    public string? Telefono { get; set; }

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato de mail no es valido")]
    [StringLength(150, ErrorMessage = "El email no puede superar los 150 caracteres")]
    public string? Email { get; set; }

    
}



