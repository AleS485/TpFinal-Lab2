using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TpFinal_Lab2.Models;

[Table("noticias")] 
public class Noticia
{
    [Key] 
    public int IdNoticia { get; set; }

    [Required]
    public string Titulo { get; set; } = string.Empty;

    [Required]
    public string Contenido { get; set; } = string.Empty;

    public DateTime Fecha { get; set; } = DateTime.Now;

    public int UsuarioId { get; set; } 

}