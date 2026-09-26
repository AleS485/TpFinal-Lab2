namespace TpFinal_Lab2.Models;
using System.ComponentModel.DataAnnotations;

public class ImagenJuego
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? OriginalName { get; set; }

    [Required]
    public string? Url { get; set; }

    public bool IsPortada { get; set; }

    public override string ToString()
    {
        return $"ImagenJuego {{Id={Id}, JuegoId={JuegoId}, Url={Url}, IsPortada={IsPortada}}}";
    }
}