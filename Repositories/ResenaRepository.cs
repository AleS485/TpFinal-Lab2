using Microsoft.Extensions.Configuration;
using MySqlConnector;
using TpFinal_Lab2.Models;

namespace TpFinal_Lab2.Repositories;

public class ResenaRepository(IConfiguration config) : RepositorioBase(config)
{
    
    public int Create(Resena resena)
    {
        var query = @"INSERT INTO resena (calificacion, comentario, fecha, estado, juego_id, cliente_id) 
                    VALUES (@calificacion, @comentario, @fecha, @estado, @juego_id, @cliente_id);
                    SELECT LAST_INSERT_ID();";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@calificacion", resena.Calificacion);
        command.Parameters.AddWithValue("@comentario", resena.Comentario);
        command.Parameters.AddWithValue("@fecha", resena.Fecha);
        command.Parameters.AddWithValue("@estado", resena.Estado);
        command.Parameters.AddWithValue("@juego_id", resena.Juego!.Id);
        command.Parameters.AddWithValue("@cliente_id", resena.Cliente!.Id);

        connection.Open();
        int res = Convert.ToInt32(command.ExecuteScalar());
        resena.Id = res;
        return res;
    }

    public List<Resena> ListAll()
    {
        List<Resena> resenas = [];
        var query = @"
            SELECT 
                r.id AS resena_id, r.calificacion, r.comentario, r.fecha, r.estado,
                j.id AS juego_id, j.titulo AS juego_titulo,
                c.id AS cliente_id, c.nombre AS cliente_nombre, c.apellido AS cliente_apellido, c.dni AS cliente_dni
            FROM resena r
            INNER JOIN juego j ON r.juego_id = j.id
            INNER JOIN cliente c ON r.cliente_id = c.id
            ORDER BY r.fecha DESC";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            resenas.Add(new Resena
            {
                Id = reader.GetInt32("resena_id"),
                Calificacion = reader.GetInt32("calificacion"),
                Comentario = reader["comentario"]?.ToString(),
                Fecha = reader.GetDateTime("fecha"),
                Estado = reader.GetBoolean("estado"),
                Juego = new Juego
                {
                    Id = reader.GetInt32("juego_id"),
                    Titulo = reader.GetString("juego_titulo")
                },
                Cliente = new Cliente
                {
                    Id = reader.GetInt32("cliente_id"),
                    Nombre = reader.GetString("cliente_nombre"),
                    Apellido = reader.GetString("cliente_apellido"),
                    Dni = reader.GetString("cliente_dni")
                }
            });
        }
        return resenas;
    }

    public Resena? GetById(int id)
    {
        var query = @"
            SELECT 
                r.id AS resena_id, r.calificacion, r.comentario, r.fecha, r.estado,
                j.id AS juego_id, j.titulo AS juego_titulo,
                c.id AS cliente_id, c.nombre AS cliente_nombre, c.apellido AS cliente_apellido, c.dni AS cliente_dni
            FROM resena r
            INNER JOIN juego j ON r.juego_id = j.id
            INNER JOIN cliente c ON r.cliente_id = c.id
            WHERE r.id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);
        connection.Open();

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Resena
            {
                Id = reader.GetInt32("resena_id"),
                Calificacion = reader.GetInt32("calificacion"),
                Comentario = reader["comentario"]?.ToString(),
                Fecha = reader.GetDateTime("fecha"),
                Estado = reader.GetBoolean("estado"),
                Juego = new Juego
                {
                    Id = reader.GetInt32("juego_id"),
                    Titulo = reader.GetString("juego_titulo")
                },
                Cliente = new Cliente
                {
                    Id = reader.GetInt32("cliente_id"),
                    Nombre = reader.GetString("cliente_nombre"),
                    Apellido = reader.GetString("cliente_apellido"),
                    Dni = reader.GetString("cliente_dni")
                }
            };
        }
        return null;
    }

    public List<Resena> ListByJuego(int juegoId)
    {
        List<Resena> resenas = [];
        var query = @"
            SELECT 
                r.id AS resena_id, r.calificacion, r.comentario, r.fecha, r.estado,
                c.id AS cliente_id, c.nombre AS cliente_nombre, c.apellido AS cliente_apellido, c.dni AS cliente_dni
            FROM resena r
            INNER JOIN cliente c ON r.cliente_id = c.id
            WHERE r.juego_id = @juego_id AND r.estado = 1
            ORDER BY r.fecha DESC";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@juego_id", juegoId);
        connection.Open();

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            resenas.Add(new Resena
            {
                Id = reader.GetInt32("resena_id"),
                Calificacion = reader.GetInt32("calificacion"),
                Comentario = reader["comentario"]?.ToString(),
                Fecha = reader.GetDateTime("fecha"),
                Estado = reader.GetBoolean("estado"),
                Cliente = new Cliente
                {
                    Id = reader.GetInt32("cliente_id"),
                    Nombre = reader.GetString("cliente_nombre"),
                    Apellido = reader.GetString("cliente_apellido"),
                    Dni = reader.GetString("cliente_dni")
                }
            });
        }
        return resenas;
    }

    public int Update(Resena resena)
    {
        var query = @"UPDATE resena 
                    SET calificacion = @calificacion, 
                        comentario = @comentario, 
                        fecha = @fecha, 
                        estado = @estado, 
                        juego_id = @juego_id, 
                        cliente_id = @cliente_id 
                    WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@id", resena.Id);
        command.Parameters.AddWithValue("@calificacion", resena.Calificacion);
        command.Parameters.AddWithValue("@comentario", resena.Comentario);
        command.Parameters.AddWithValue("@fecha", resena.Fecha);
        command.Parameters.AddWithValue("@estado", resena.Estado);
        command.Parameters.AddWithValue("@juego_id", resena.Juego!.Id);
        command.Parameters.AddWithValue("@cliente_id", resena.Cliente!.Id);

        connection.Open();
        return command.ExecuteNonQuery();
    }

    public int Delete(int id)
    {
        var query = @"DELETE FROM resena WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        return command.ExecuteNonQuery();
    }



}






