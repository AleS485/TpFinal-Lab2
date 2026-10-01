using Microsoft.Extensions.Configuration;
using MySqlConnector;
using TpFinal_Lab2.Models;

namespace TpFinal_Lab2.Repositories;

public class ClienteRepository(IConfiguration configuration) : RepositorioBase(configuration)
{
    public int Create(Cliente cliente)
    {
        var query = @"INSERT INTO cliente (dni, nombre, apellido, telefono, email)
                    VALUES (@dni, @nombre, @apellido, @telefono, @email);
                    SELECT LAST_INSERT_ID();";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@dni", cliente.Dni);
        command.Parameters.AddWithValue("@nombre", cliente.Nombre);
        command.Parameters.AddWithValue("@apellido", cliente.Apellido);
        command.Parameters.AddWithValue("@telefono", cliente.Telefono == null ? DBNull.Value : cliente.Telefono);
        command.Parameters.AddWithValue("@email", cliente.Email);

        connection.Open();
        int res = Convert.ToInt32(command.ExecuteScalar());
        cliente.Id = res;
        return res;
    }

    public Cliente? FindById(int id)
    {
        var query = @"SELECT id, dni, nombre, apellido, telefono, email 
                    FROM cliente 
                    WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Cliente
            {
                Id = reader.GetInt32("id"),
                Dni = reader.GetString("dni"),
                Nombre = reader.GetString("nombre"),
                Apellido = reader.GetString("apellido"),
                Telefono = reader["telefono"] as string,
                Email = reader.GetString("email")
            };
        }

        return null;
    }

    public Cliente? FindByDni(string dni)
    {
        var query = @"SELECT id, dni, nombre, apellido, telefono, email 
                    FROM cliente 
                    WHERE dni = @dni";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@dni", dni);

        connection.Open();
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Cliente
            {
                Id = reader.GetInt32("id"),
                Dni = reader.GetString("dni"),
                Nombre = reader.GetString("nombre"),
                Apellido = reader.GetString("apellido"),
                Telefono = reader["telefono"] as string,
                Email = reader.GetString("email")
            };
        }

        return null;
    }

    public List<Cliente> ListAll(int page = 1, int limit = 10)
    {
        List<Cliente> lista = [];
        var query = $@"SELECT id, dni, nombre, apellido, telefono, email 
                    FROM cliente 
                    ORDER BY apellido, nombre 
                      LIMIT {(page - 1) * limit}, {limit}";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Cliente
            {
                Id = reader.GetInt32("id"),
                Dni = reader.GetString("dni"),
                Nombre = reader.GetString("nombre"),
                Apellido = reader.GetString("apellido"),
                Telefono = reader["telefono"] as string,
                Email = reader.GetString("email")
            });
        }
        return lista;
    }

    public int Update(Cliente cliente)
    {
        var query = @"UPDATE cliente 
                    SET dni = @dni, nombre = @nombre, apellido = @apellido, 
                    telefono = @telefono, email = @email 
                    WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@id", cliente.Id);
        command.Parameters.AddWithValue("@dni", cliente.Dni);
        command.Parameters.AddWithValue("@nombre", cliente.Nombre);
        command.Parameters.AddWithValue("@apellido", cliente.Apellido);
        command.Parameters.AddWithValue("@telefono", cliente.Telefono == null ? DBNull.Value : cliente.Telefono);
        command.Parameters.AddWithValue("@email", cliente.Email);

        connection.Open();
        return command.ExecuteNonQuery();
    }

    public int Delete(int id)
    {
        var query = @"DELETE FROM cliente WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        return command.ExecuteNonQuery();
    }
}