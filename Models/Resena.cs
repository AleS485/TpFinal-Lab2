namespace TpFinal_Lab2.Models;
using System.ComponentModel.DataAnnotations;

public class Resena
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Calificacion { get; set; }

    public string? Comentario { get; set; }

    [Required]
    public DateTime Fecha { get; set; }

    [Required]
    public bool Estado { get; set; }

    [Required]
    public Juego? Juego { get; set; }

    [Required]
    public Cliente? Cliente { get; set; }

    public override string ToString()
    {
        return $"Resena {{Id={Id}, Calificacion={Calificacion}, Juego={Juego?.Titulo}, Cliente={Cliente?.Dni}}}";
    }
}