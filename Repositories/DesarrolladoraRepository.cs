using Microsoft.Extensions.Configuration;
using MySqlConnector;
using TpFinal_Lab2.Models;

namespace TpFinal_Lab2.Repositories;

public class DesarrolladoraRepository(IConfiguration configuration) : RepositorioBase(configuration)
{
    
    public int Create(Desarrolladora desarrolladora)
    {
        var query = @"INSERT INTO desarrolladora (nombre, web)
                    VALUES (@nombre, @web);
                    SELECT LAST_INSERT_ID();";
        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@nombre", desarrolladora.Nombre);
        command.Parameters.AddWithValue("@web", desarrolladora.Web == null ? DBNull.Value : desarrolladora.Web);
        connection.Open();
        int res = Convert.ToInt32(command.ExecuteScalar());
        desarrolladora.Id = res;
        return res;
    }

    public int Update(Desarrolladora desarrolladora)
    {
        var query = @"UPDATE desarrolladora 
                    SET nombre = @nombre, web = @web 
                    WHERE id = @id";
        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", desarrolladora.Id);
        command.Parameters.AddWithValue("@nombre", desarrolladora.Nombre);
        command.Parameters.AddWithValue("@web", desarrolladora.Web == null ? DBNull.Value : desarrolladora.Web);
        connection.Open();
        return command.ExecuteNonQuery();
    }

    public Desarrolladora? FindById(int id)
    {
        var query = @"SELECT id, nombre, web 
                    FROM desarrolladora 
                    WHERE id = @id";    
        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);
        connection.Open();
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Desarrolladora
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre"),
                Web = reader["web"] as string
            };
        }

        return null;
    }

    public List<Desarrolladora> ListAll(int page = 1, int limit = 10)
    {
        List<Desarrolladora> lista = [];
        var query = $@"SELECT id, nombre, web 
                    FROM desarrolladora 
                    ORDER BY nombre 
                    LIMIT {(page - 1) * limit}, {limit}";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Desarrolladora
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre"),
                Web = reader["web"] as string
            });
        }
        return lista;
    }
    
    public int Delete(int id)
    {
        var query = @"DELETE FROM desarrolladora WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        return command.ExecuteNonQuery();
    }


}
