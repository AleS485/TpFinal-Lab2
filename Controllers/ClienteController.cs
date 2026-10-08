using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;

namespace TpFinal_Lab2.Controllers;

public class ClienteController(ClienteRepository clienteRepo) : Controller
{

    [HttpGet]
    public IActionResult Registrar()
    {
        
        return View();

    }

    [HttpPost]
    public IActionResult Registrar(Cliente cliente)
    {
        if (!ModelState.IsValid)
        {
            return View(cliente);
        }
        try
        {
            clienteRepo.Create(cliente);
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
        var lista = clienteRepo.ListAll(pagina, tamano);

        ViewBag.Pagina = pagina;
        ViewBag.HaySiguiente = lista.Count == tamano;

        return View(lista);
    }

    [HttpGet]
    public IActionResult Editar(int id)
    {
        var cliente = clienteRepo.FindById(id);
        if(cliente == null)
        {
            return RedirectToAction(nameof(Listar));
        }
        return View(cliente);
    }

    [HttpPost]
    public IActionResult Editar(Cliente cliente)
    {
        if (!ModelState.IsValid)
        {
            return View(cliente);
        }
        try
        {
            clienteRepo.Update(cliente);
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
        
        var cliente = clienteRepo.FindById(id);
        if (cliente == null)
        {
            return RedirectToAction(nameof(Listar));
        }
        return View(cliente);

    }

    [HttpPost]
    public IActionResult Eliminar(Cliente cliente)
    {
        try
        {
            clienteRepo.Delete(cliente.Id);
            return RedirectToAction(nameof(Listar));
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return RedirectToAction(nameof(Listar));
        }
    }



}
