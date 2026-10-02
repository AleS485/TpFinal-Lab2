using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TpFinal_Lab2.Models;

namespace TpFinal_Lab2.Controllers;

public class DesarrolladoraController : Controller
{
    [HttpGet]
    public IActionResult Listar(int pagina = 1)
    {
        var desarrolladoras = new List<Desarrolladora>
        {
            new Desarrolladora { Id = 1, Nombre = "Valve Corporation", Web = "https://www.valvesoftware.com" },
            new Desarrolladora { Id = 2, Nombre = "Nintendo", Web = "https://www.nintendo.com" },
            new Desarrolladora { Id = 3, Nombre = "Rockstar Games", Web = "https://www.rockstargames.com" },
            new Desarrolladora { Id = 4, Nombre = "CD Projekt Red", Web = "https://en.cdprojektred.com" },
            new Desarrolladora { Id = 5, Nombre = "FromSoftware", Web = "https://www.fromsoftware.jp" }
        };

        ViewBag.Pagina = pagina;
        ViewBag.HaySiguiente = true;

        return View(desarrolladoras);
    }

    

    
}
