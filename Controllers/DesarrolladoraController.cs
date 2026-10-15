using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;

namespace TpFinal_Lab2.Controllers;

public class DesarrolladoraController(DesarrolladoraRepository desarrolladoraRepo) : Controller
{

    [HttpPost]
    public IActionResult Registrar([FromBody] Desarrolladora desarrolladora)
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
            desarrolladoraRepo.Create(desarrolladora);
            return Json(new { success = true });
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return Json(new { success = false, message = "Error registrando la desarrolladora" });
        }
    }


    [HttpGet]
    public IActionResult Listar(int pagina = 1)
    {
        return View();
    }

    [HttpGet]
    public IActionResult TraerDesarrolladoras(int pagina = 1)
    {
        int tamano = 5;
        var lista = desarrolladoraRepo.ListAll(pagina, tamano);

        bool haySiguiente = lista.Count == tamano;

        return Json(new
        {
            desarrolladoras = lista,
            paginaActual = pagina,
            haySiguiente = haySiguiente
        });
    }

    [HttpPost]
    public IActionResult Editar([FromBody] Desarrolladora desarrolladora)
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
            desarrolladoraRepo.Update(desarrolladora); 
            return Json(new { success = true });
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return Json(new { success = false, message = "Error editando la desarrolladora" });
        }
    }

    [HttpPost]
    public IActionResult Eliminar(int id)
    {
        try
        {
            var desarrolladora = desarrolladoraRepo.FindById(id);
            if(desarrolladora == null)
            {
                return Json(new { success = false, message = "NO EXISTE LA DESARROLLADORA" });
            }
            
            desarrolladoraRepo.Delete(id);
            return Json(new { success = true });
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e.Message);
            return Json(new { success = false, message = e.Message });
        }
    }



    
}
