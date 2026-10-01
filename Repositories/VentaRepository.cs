using Microsoft.Extensions.Configuration;
using MySqlConnector;
using TpFinal_Lab2.Models;

namespace TpFinal_Lab2.Repositories;

public class VentaRepository(IConfiguration config) : RepositorioBase(config)
{
    public int Create(Venta venta)
    {
        var query = @"INSERT INTO venta (fecha_hora, cantidad, precio_total, juego_id, cliente_id, usuario_id) 
                    VALUES (@fecha_hora, @cantidad, @precio_total, @juego_id, @cliente_id, @usuario_id);
                    SELECT LAST_INSERT_ID();";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@fecha_hora", venta.FechaHora);
        command.Parameters.AddWithValue("@cantidad", venta.Cantidad);
        command.Parameters.AddWithValue("@precio_total", venta.PrecioTotal);
        command.Parameters.AddWithValue("@juego_id", venta.Juego!.Id);
        command.Parameters.AddWithValue("@cliente_id", venta.Cliente!.Id);
        command.Parameters.AddWithValue("@usuario_id", venta.Usuario!.Id);

        connection.Open();
        int res = Convert.ToInt32(command.ExecuteScalar());
        venta.Id = res;
        return res;
    }

    public List<Venta> ListAll()
    {
        List<Venta> ventas = [];
        var query = @"
            SELECT 
            v.id AS venta_id, v.fecha_hora, v.cantidad, v.precio_total,
            j.id AS juego_id, j.titulo AS juego_titulo, j.precio AS juego_precio,
            c.id AS cliente_id, c.nombre AS cliente_nombre, c.apellido AS cliente_apellido, c.dni AS cliente_dni,
            u.id AS usuario_id, u.nombre AS usuario_nombre, u.apellido AS usuario_apellido, u.email AS usuario_email
            FROM venta v
            INNER JOIN juego j ON v.juego_id = j.id
            INNER JOIN cliente c ON v.cliente_id = c.id
            INNER JOIN usuario u ON v.usuario_id = u.id
            ORDER BY v.fecha_hora DESC";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            ventas.Add(new Venta
            {
                Id = reader.GetInt32("venta_id"),
                FechaHora = reader.GetDateTime("fecha_hora"),
                Cantidad = reader.GetInt32("cantidad"),
                PrecioTotal = reader.GetDecimal("precio_total"),
                Juego = new Juego
                {
                    Id = reader.GetInt32("juego_id"),
                    Titulo = reader.GetString("juego_titulo"),
                    Precio = reader.GetDecimal("juego_precio")
                },
                Cliente = new Cliente
                {
                    Id = reader.GetInt32("cliente_id"),
                    Nombre = reader.GetString("cliente_nombre"),
                    Apellido = reader.GetString("cliente_apellido"),
                    Dni = reader.GetString("cliente_dni")
                },
                Usuario = new Usuario
                {
                    Id = reader.GetInt32("usuario_id"),
                    Nombre = reader.GetString("usuario_nombre"),
                    Apellido = reader.GetString("usuario_apellido"),
                    Email = reader.GetString("usuario_email")
                }
            });
        }
        return ventas;
    }

    public Venta? GetById(int id)
    {
        var query = @"
            SELECT 
                v.id AS venta_id, v.fecha_hora, v.cantidad, v.precio_total,
                j.id AS juego_id, j.titulo AS juego_titulo, j.precio AS juego_precio,
                c.id AS cliente_id, c.nombre AS cliente_nombre, c.apellido AS cliente_apellido, c.dni AS cliente_dni,
                u.id AS usuario_id, u.nombre AS usuario_nombre, u.apellido AS usuario_apellido, u.email AS usuario_email
            FROM venta v
            INNER JOIN juego j ON v.juego_id = j.id
            INNER JOIN cliente c ON v.cliente_id = c.id
            INNER JOIN usuario u ON v.usuario_id = u.id
            WHERE v.id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);
        connection.Open();

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new Venta
            {
                Id = reader.GetInt32("venta_id"),
                FechaHora = reader.GetDateTime("fecha_hora"),
                Cantidad = reader.GetInt32("cantidad"),
                PrecioTotal = reader.GetDecimal("precio_total"),
                Juego = new Juego
                {
                    Id = reader.GetInt32("juego_id"),
                    Titulo = reader.GetString("juego_titulo"),
                    Precio = reader.GetDecimal("juego_precio")
                },
                Cliente = new Cliente
                {
                    Id = reader.GetInt32("cliente_id"),
                    Nombre = reader.GetString("cliente_nombre"),
                    Apellido = reader.GetString("cliente_apellido"),
                    Dni = reader.GetString("cliente_dni")
                },
                Usuario = new Usuario
                {
                    Id = reader.GetInt32("usuario_id"),
                    Nombre = reader.GetString("usuario_nombre"),
                    Apellido = reader.GetString("usuario_apellido"),
                    Email = reader.GetString("usuario_email")
                }
            };
        }
        return null;
    }

    public int Update(Venta venta)
    {
        var query = @"UPDATE venta 
                    SET fecha_hora = @fecha_hora, 
                        cantidad = @cantidad, 
                        precio_total = @precio_total, 
                    juego_id = @juego_id, 
                        cliente_id = @cliente_id, 
                        usuario_id = @usuario_id 
                    WHERE id = @id";
        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", venta.Id);
        command.Parameters.AddWithValue("@fecha_hora", venta.FechaHora);
        command.Parameters.AddWithValue("@cantidad", venta.Cantidad);
        command.Parameters.AddWithValue("@precio_total", venta.PrecioTotal);
        command.Parameters.AddWithValue("@juego_id", venta.Juego!.Id);
        command.Parameters.AddWithValue("@cliente_id", venta.Cliente!.Id);
        command.Parameters.AddWithValue("@usuario_id", venta.Usuario!.Id);
        connection.Open();
        return command.ExecuteNonQuery();
    }

    public int Delete(int id)
    {
        var query = @"DELETE FROM venta WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        return command.ExecuteNonQuery();
    }

}

