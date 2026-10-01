using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using TpFinal_Lab2.Models;

namespace TpFinal_Lab2.Repositories;

public class ImagenJuegoRepository(IConfiguration config, [FromServices] IWebHostEnvironment environment, JuegoRepository repoJuego) : RepositorioBase(config)
{
    
    public int Upload(ImagenJuego img, Juego juego, IFormFile file)
    {
        if (img.IsPortada)
        {
            FetchPortada(juego);
            if (juego.Portada != null)
            {
                Delete(juego.Portada);
            }
        }

        string extension = Path.GetExtension(file.FileName);
        string uploadPath = Path.Combine(environment.WebRootPath, "Uploads", "Juegos", juego.Id.ToString());
        
        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }

        string fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            file.CopyTo(stream);
        }

        img.JuegoId = juego.Id;
        img.OriginalName = file.FileName;
        img.Url = $"/Uploads/Juegos/{juego.Id}/{fileName}";

        return Create(img, juego);
    }

    private int Create(ImagenJuego img, Juego juego)
    {
        var query = @"INSERT INTO imagenjuego (juego_id, original_name, url, is_portada) 
                    VALUES (@juego_id, @original_name, @url, @is_portada);
                    SELECT LAST_INSERT_ID();";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@juego_id", juego.Id);
        command.Parameters.AddWithValue("@original_name", img.OriginalName);
        command.Parameters.AddWithValue("@url", img.Url);
        command.Parameters.AddWithValue("@is_portada", img.IsPortada);

        connection.Open();
        int nuevoId = Convert.ToInt32(command.ExecuteScalar());
        img.Id = nuevoId;

        if (img.IsPortada)
        {
            juego.Portada = img;
            repoJuego.Update(juego);
        }

        return nuevoId > 0 ? 1 : 0;
    }

    public void Load(Juego juego)
    {
        FetchGaleria(juego);
        FetchPortada(juego);
    }

    public void Delete(ImagenJuego img)
    {
        var query = @"DELETE FROM imagenjuego WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", img.Id);

        connection.Open();
        command.ExecuteNonQuery();
    }

    private void FetchGaleria(Juego juego)
    {
        List<ImagenJuego> images = [];

        var query = @"SELECT * FROM imagenjuego WHERE juego_id = @juego_id AND is_portada = 0";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@juego_id", juego.Id);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            images.Add(new ImagenJuego
            {
                Id = reader.GetInt32("id"),
                JuegoId = reader.GetInt32("juego_id"),
                OriginalName = reader.GetString("original_name"),
                Url = reader.GetString("url"),
                IsPortada = false
            });
        }
        juego.Imagenes = images;
    }

    private void FetchPortada(Juego juego)
    {
        var query = @"SELECT * FROM imagenjuego WHERE juego_id = @juego_id AND is_portada = 1 LIMIT 1";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@juego_id", juego.Id);

        connection.Open();
        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            var portada = new ImagenJuego
            {
                Id = reader.GetInt32("id"),
                JuegoId = reader.GetInt32("juego_id"),
                OriginalName = reader.GetString("original_name"),
                Url = reader.GetString("url"),
                IsPortada = true
            };
            juego.Portada = portada;
        }
    }



}









