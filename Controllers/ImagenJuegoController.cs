using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TpFinal_Lab2.DTO;
using TpFinal_Lab2.Models;
using TpFinal_Lab2.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace TpFinal_Lab2.Controllers;

public class ImagenJuegoController([FromServices] IWebHostEnvironment environment, JuegoRepository juegoRepo, ImagenJuegoRepository imagenRepo) : Controller
{

    [Authorize]
    public IActionResult TraerFotos(int id)
    {
        var juego = juegoRepo.FindById(id); 
        
        if (juego == null)
        {
            return NotFound();
        }

        imagenRepo.Load(juego);
        
        return Ok(new { juego.Imagenes, juego.Portada });
    }

    [Authorize]
    public async Task<IActionResult> CambiarPortada(int id, IFormFile portadaFile)
    {
        var juego = juegoRepo.FindById(id);

        if (juego == null)
        {
            return NotFound();
        }

        if (portadaFile == null || portadaFile.Length == 0)
        {
            return BadRequest("tenes que seleccionar un archivo de portada");
        }

        var img = new ImagenJuego
        {
            OriginalName = portadaFile.FileName,
            File = portadaFile,
            IsPortada = true
        };

        imagenRepo.Upload(img, juego, portadaFile);

        return Ok(new { url = img.Url });
    }
    

    [Authorize]
    public async Task<IActionResult> Alta(int id, [FromForm] List<IFormFile> imagenes)
    {
        if (imagenes == null || imagenes.Count == 0)
        {
            return BadRequest("no se recibieron los archivos para la galeria");
        }
        
        var juego = juegoRepo.FindById(id);
        if (juego == null)
        {
            return NotFound();
        }

        foreach (var file in imagenes)
        {
            if (file.Length > 0)
            {
                var img = new ImagenJuego
                {
                    OriginalName = file.FileName,
                    IsPortada = false
                };
                
                imagenRepo.Upload(img, juego, file);
            }
        }

        imagenRepo.Load(juego);
        return Ok(juego.Imagenes);
    }

    [Authorize]
    public IActionResult Eliminar(int id)
    {
        if (id <= 0)
        {
            return BadRequest("el id de la imagen es obligatorio");
        }
        
        var img = new ImagenJuego { Id = id };
        imagenRepo.Delete(img);
        
        return Ok();
    }


}
