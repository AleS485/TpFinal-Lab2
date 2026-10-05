using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;

namespace TpFinal_Lab2.Controllers;

public class DesarrolladoraController(DesarrolladoraRepository desarrolladoraRepo) : Controller
{

    [HttpGet]
    public IActionResult Registrar()
    {
        
        return View();

    }

    [HttpPost]
    public IActionResult Registrar(Desarrolladora desarrolladora)
    {
        if (!ModelState.IsValid)
        {
            return View(desarrolladora);
        }
        try
        {
            desarrolladoraRepo.Create(desarrolladora);
            return RedirectToAction(nameof(Listar));
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return RedirectToAction(nameof(Listar));
        }
    }


    [HttpGet]
    public IActionResult Listar(int pagina = 1)
    {
        int tamano = 5;
        var lista = desarrolladoraRepo.ListAll(pagina, tamano);

        ViewBag.Pagina = pagina;
        ViewBag.HaySiguiente = lista.Count == tamano;

        return View(lista);
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var desarrolladora = desarrolladoraRepo.FindById(id);
        if(desarrolladora == null)
        {
            return RedirectToAction(nameof(Listar));
        }
        return View(desarrolladora);
    }

    [HttpPost]
    public IActionResult Editar(Desarrolladora desarrolladora)
    {
        if (!ModelState.IsValid)
        {
            return View(desarrolladora);
        }
        try
        {
            desarrolladoraRepo.Update(desarrolladora);
            return RedirectToAction(nameof(Listar));
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return RedirectToAction(nameof(Listar));
        }
    }


    [HttpGet]
    public IActionResult Eliminar(int id)
    {
        
        var desarrolladora = desarrolladoraRepo.FindById(id);
        if (desarrolladora == null)
        {
            return RedirectToAction(nameof(Listar));
        }
        return View(desarrolladora);

    }

    [HttpPost]
    public IActionResult Eliminar(Desarrolladora desarrolladora)
    {
        try
        {
            desarrolladoraRepo.Delete(desarrolladora.Id);
            return RedirectToAction(nameof(Listar));
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return RedirectToAction(nameof(Listar));
        }
    }

    
}
