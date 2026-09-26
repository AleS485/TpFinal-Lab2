namespace TpFinal_Lab2.Models;
using System.ComponentModel.DataAnnotations;

public class Desarrolladora
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Nombre { get; set; }

    public string? Web { get; set; }

    public override string ToString()
    {
        return $"Desarrolladora {{Id={Id}, Nombre={Nombre}, Web={Web}}}";
    }
}