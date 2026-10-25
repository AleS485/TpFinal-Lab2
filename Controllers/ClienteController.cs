using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace TpFinal_Lab2.Controllers;

public class ClienteController(ClienteRepository clienteRepo) : Controller
{

    [Authorize]
    [HttpPost]
    public IActionResult Registrar([FromBody] Cliente cliente)
    {
        
        if (!ModelState.IsValid)
        {
            string mensajeError = "El formulario tiene datos no validos";

            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        mensajeError = error.ErrorMessage;
                        break;
                    }
                }
                if (mensajeError != "El formulario tiene datos no validos") break;
            }

            return Json(new { success = false, message = mensajeError });
        }

        try
        {
            clienteRepo.Create(cliente);
            return Json(new { success = true });
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return Json(new { success = false, message = "Error registrando el cliente"});
        }



    }
    
    [Authorize]
    [HttpGet]
    public IActionResult Listar(int pagina = 1)
    {
        return View();
    }


    [Authorize]
    [HttpGet]
    public IActionResult TraerClientes(int pagina = 1)
    {
        int tamano = 5;
        var lista = clienteRepo.ListAll(pagina, tamano);

        bool haySiguiente = lista.Count == tamano;

        return Json(new
        {
            clientes = lista,
            paginaActual = pagina,
            haySiguiente = haySiguiente
        });
    }

    [Authorize]
    [HttpPost]
    public IActionResult Editar([FromBody] Cliente cliente)
    {
        if (!ModelState.IsValid)
        {
            string mensajeError = "El formulario tiene datos no validos";

            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    if (!string.IsNullOrEmpty(error.ErrorMessage))
                    {
                        mensajeError = error.ErrorMessage;
                        break;
                    }
                }
                if (mensajeError != "El formulario tiene datos no validos") break;
            }
            return Json(new { success = false, message = mensajeError });
        }

        try
        {
            clienteRepo.Update(cliente); 
            return Json(new { success = true });
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return Json(new { success = false, message = "Error editando el cliente" });
        }
    }




    [HttpPost]
    public IActionResult Eliminar(int id)
    {
        try
        {
            var cliente = clienteRepo.FindById(id);
            if(cliente == null)
            {
                return Json(new { success = false, message = "NO EXISTE EL CLIENTE" });
            }
            
            clienteRepo.Delete(id);
            return Json(new { success = true });
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return Json(new {success = false, message = e.Message});
        }
    }

    [Authorize]
    [HttpGet]
    public JsonResult BuscarFiltrados(string filtro)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(filtro))
            {
                return Json(new List<Cliente>());
            }

            var clientes = clienteRepo.BuscarFiltrados(filtro);
            return Json(clientes);
        }
        catch (Exception)
        {
            return Json(new List<Cliente>());
        }
    }



}
