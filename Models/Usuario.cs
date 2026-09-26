namespace TpFinal_Lab2.Models;
using System.ComponentModel.DataAnnotations;

public class Usuario
{
    [Key]
    public int Id {get; set;}

    [Required]
    public string? Nombre {get; set;}

    [Required]
    public string? Apellido {get; set;}

    [Required]
    [EmailAddress]
    public string? Email {get; set;}

    [Required]
    public string? Password {get; set;}

    [Required]
    public string? Role {get; set;}

    public string? AvatarUrl {get; set;}

    public bool Estado{get; set;} = true;

    public override string ToString()
    {
        return $"Usuario {{Id={Id}, Nombre={Nombre}, Apellido={Apellido}, Email={Email}, Role={Role}, Estado={Estado}}}";
    }




}


