using System.ComponentModel.DataAnnotations;

namespace TpFinal_Lab2.Models;

public class LoginView
{
    [DataType(DataType.EmailAddress)]
    public string Usuario { get; set; } = "";

    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
}