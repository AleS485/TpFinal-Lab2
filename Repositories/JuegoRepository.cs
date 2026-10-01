using Microsoft.Extensions.Configuration;
using MySqlConnector;
using TpFinal_Lab2.Models;

namespace TpFinal_Lab2.Repositories;

public class JuegoRepository(IConfiguration configuration) : RepositorioBase(configuration)
{
    public int Create(Juego juego)
    {
        var query = @"INSERT INTO juego (titulo, genero, descripcion, precio, stock, estado, desarrolladora_id)
                    VALUES (@titulo, @genero, @descripcion, @precio, @stock, @estado, @desarrolladora_id);
                    SELECT LAST_INSERT_ID();";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@titulo", juego.Titulo);
        command.Parameters.AddWithValue("@genero", juego.Genero);
        command.Parameters.AddWithValue("@descripcion", juego.Descripcion == null ? DBNull.Value : juego.Descripcion);
        command.Parameters.AddWithValue("@precio", juego.Precio);
        command.Parameters.AddWithValue("@stock", juego.Stock);
        command.Parameters.AddWithValue("@estado", juego.Estado);
        command.Parameters.AddWithValue("@desarrolladora_id", juego.Desarrolladora != null ? juego.Desarrolladora.Id : DBNull.Value);

        connection.Open();
        int res = Convert.ToInt32(command.ExecuteScalar());
        juego.Id = res;
        return res;
    }

    public Juego? FindById(int id)
    {
        var query = @"SELECT j.id, j.titulo, j.genero, j.descripcion, j.precio, j.stock, j.estado, j.desarrolladora_id,
                    d.nombre AS d_nombre, d.web AS d_web
                    FROM juego j
                    JOIN desarrolladora d ON j.desarrolladora_id = d.id
                    WHERE j.id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Juego
            {
                Id = reader.GetInt32("id"),
                Titulo = reader.GetString("titulo"),
                Genero = reader.GetString("genero"),
                Descripcion = reader["descripcion"] as string,
                Precio = reader.GetDecimal("precio"),
                Stock = reader.GetInt32("stock"),
                Estado = reader.GetBoolean("estado"),
                Desarrolladora = new Desarrolladora
                {
                    Id = reader.GetInt32("desarrolladora_id"),
                    Nombre = reader.GetString("d_nombre"),
                    Web = reader["d_web"] as string
                }
            };
        }

        return null;
    }

    public List<Juego> ListAll(int page = 1, int limit = 10)
    {
        List<Juego> lista = [];
        var query = $@"SELECT j.id, j.titulo, j.genero, j.descripcion, j.precio, j.stock, j.estado, j.desarrolladora_id,
                    d.nombre AS d_nombre, d.web AS d_web
                    FROM juego j
                    JOIN desarrolladora d ON j.desarrolladora_id = d.id
                    ORDER BY j.titulo
                    LIMIT {(page - 1) * limit}, {limit}";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Juego
            {
                Id = reader.GetInt32("id"),
                Titulo = reader.GetString("titulo"),
                Genero = reader.GetString("genero"),
                Descripcion = reader["descripcion"] as string,
                Precio = reader.GetDecimal("precio"),
                Stock = reader.GetInt32("stock"),
                Estado = reader.GetBoolean("estado"),
                Desarrolladora = new Desarrolladora
                {
                    Id = reader.GetInt32("desarrolladora_id"),
                    Nombre = reader.GetString("d_nombre"),
                    Web = reader["d_web"] as string
                }
            });
        }
        return lista;
    }

    public int Update(Juego juego)
    {
        var query = @"UPDATE juego 
                    SET titulo = @titulo, genero = @genero, descripcion = @descripcion,
                    precio = @precio, stock = @stock, estado = @estado, desarrolladora_id = @desarrolladora_id
                    WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@id", juego.Id);
        command.Parameters.AddWithValue("@titulo", juego.Titulo);
        command.Parameters.AddWithValue("@genero", juego.Genero);
        command.Parameters.AddWithValue("@descripcion", juego.Descripcion == null ? DBNull.Value : juego.Descripcion);
        command.Parameters.AddWithValue("@precio", juego.Precio);
        command.Parameters.AddWithValue("@stock", juego.Stock);
        command.Parameters.AddWithValue("@estado", juego.Estado);
        command.Parameters.AddWithValue("@desarrolladora_id", juego.Desarrolladora != null ? juego.Desarrolladora.Id : DBNull.Value);

        connection.Open();
        return command.ExecuteNonQuery();
    }

    public int Delete(int id)
    {
        var query = @"DELETE FROM juego WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        return command.ExecuteNonQuery();
    }
}