namespace TpFinal_Lab2.Models;
using System.ComponentModel.DataAnnotations;

public class Juego
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Titulo { get; set; }

    [Required]
    public string? Genero { get; set; }

    public string? Descripcion { get; set; }

    [Required]
    public decimal Precio { get; set; }

    [Required]
    public int Stock { get; set; }

    [Required]
    public bool Estado { get; set; } = true;

    [Required]
    public Desarrolladora? Desarrolladora { get; set; }

    public ImagenJuego? Portada { get; set; }
    public IList<ImagenJuego> Imagenes { get; set; } = new List<ImagenJuego>();

    public override string ToString()
    {
        return $"Juego {{Id={Id}, Titulo={Titulo}, Genero={Genero}, Precio={Precio}, Stock={Stock}, Estado={Estado}}}";
    }
}