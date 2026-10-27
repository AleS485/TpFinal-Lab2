using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TpFinal_Lab2.Models;

namespace TpFinal_Lab2.Controllers.Api;

[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
[ApiController]
public class NoticiasController(DataContext contexto) : ControllerBase
{
    // GET: api/noticias
    [HttpGet]
    public async Task<ActionResult<List<Noticia>>> Get()
    {
        try
        {
            var noticias = await contexto.Noticias.ToListAsync();
            return Ok(noticias);
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // GET: api/noticias/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Noticia>> Get(int id)
    {
        try
        {
            var noticia = await contexto.Noticias.FindAsync(id);
            return noticia != null ? Ok(noticia) : NotFound();
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // POST: api/noticias
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Noticia noticia)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "los datos de la noticia no son validos" });

            contexto.Noticias.Add(noticia);
            await contexto.SaveChangesAsync();
            
            return CreatedAtAction(nameof(Get), new { id = noticia.IdNoticia }, noticia);
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // PUT: api/noticias/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] Noticia noticia)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "los datos de la noticia no son validos" });

            var original = await contexto.Noticias.FindAsync(id);
            if (original == null)
                return NotFound(new { success = false, message = "no se encontro esa noticia" });

            original.Titulo = noticia.Titulo;
            original.Contenido = noticia.Contenido;

            contexto.Noticias.Update(original);
            await contexto.SaveChangesAsync();

            return Ok(new { success = true, message = "se actualizo la noticia correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }

    // DELETE: api/noticias/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var noticia = await contexto.Noticias.FindAsync(id);
            if (noticia == null)
                return NotFound(new { success = false, message = "no se encontro esa noticia" });

            contexto.Noticias.Remove(noticia);
            await contexto.SaveChangesAsync();

            return Ok(new { success = true, message = "se elimino la noticia correctamente" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { success = false, message = ex.Message });
        }
    }
}