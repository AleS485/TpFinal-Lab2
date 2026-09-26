namespace TpFinal_Lab2.Models;
using System.ComponentModel.DataAnnotations;

public class Cliente
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Dni { get; set; }

    [Required]
    public string? Nombre { get; set; }

    [Required]
    public string? Apellido { get; set; }

    public string? Telefono { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    public override string ToString()
    {
        return $"Cliente {{Id={Id}, Dni={Dni}, Nombre={Nombre}, Apellido={Apellido}, Email={Email}}}";
    }
}



