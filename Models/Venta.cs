namespace TpFinal_Lab2.Models;
using System.ComponentModel.DataAnnotations;

public class Venta
{
    [Key]
    public int Id { get; set; }

    public DateTime FechaHora { get; set; } = DateTime.Now;

    [Required]
    public int Cantidad { get; set; } = 1;

    [Required]
    public decimal PrecioTotal { get; set; }

    [Required]
    public Juego? Juego { get; set; }

    [Required]
    public Cliente? Cliente { get; set; }

    [Required]
    public Usuario? Usuario { get; set; }

    public override string ToString()
    {
        return $"Venta {{Id={Id}, Fecha={FechaHora}, Cantidad={Cantidad}, Total={PrecioTotal}, Juego={Juego?.Titulo}, Cliente={Cliente?.Dni}}}";
    }
}