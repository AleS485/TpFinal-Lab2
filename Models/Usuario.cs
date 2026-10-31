namespace TpFinal_Lab2.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int Id {get; set;}

    [Required(ErrorMessage = "El nombre es obligatorio")]
    public string? Nombre {get; set;}

    [Required(ErrorMessage = "El apellido es obligatorio")]
    public string? Apellido {get; set;}

    [Required(ErrorMessage = "El email es obligatorio")]
    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    public string? Email {get; set;}

    [Required(ErrorMessage = "La contrasena es obligatoria")]
    public string? Password {get; set;}

    [Required(ErrorMessage = "Tenes que seleccionar un rol")]
    public string? Role {get; set;}

    public string? AvatarUrl {get; set;}

    public bool Estado{get; set;}

    public override string ToString()
    {
        return $"Usuario {{Id={Id}, Nombre={Nombre}, Apellido={Apellido}, Email={Email}, Role={Role}, Estado={Estado}}}";
    }




}


