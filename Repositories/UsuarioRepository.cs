using Microsoft.Extensions.Configuration;
using MySqlConnector;
using TpFinal_Lab2.Models;

namespace TpFinal_Lab2.Repositories;

public class UsuarioRepository(IConfiguration config, IWebHostEnvironment environment) : RepositorioBase(config)
{
    
    public int Create(Usuario usuario)
    {
        var query = @"INSERT INTO usuario (nombre, apellido, email, password, role, avatar_url, estado)
                    VALUES (@nombre, @apellido, @email, @password, @role, @avatar_url, @estado);
                    SELECT LAST_INSERT_ID();";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@nombre", usuario.Nombre);
        command.Parameters.AddWithValue("@apellido", usuario.Apellido);
        command.Parameters.AddWithValue("@email", usuario.Email);
        command.Parameters.AddWithValue("@password", usuario.Password);
        command.Parameters.AddWithValue("@role", usuario.Role);
        command.Parameters.AddWithValue("@avatar_url", usuario.AvatarUrl == null ? DBNull.Value : usuario.AvatarUrl);
        command.Parameters.AddWithValue("@estado", usuario.Estado);

        connection.Open();
        int res = Convert.ToInt32(command.ExecuteScalar());
        usuario.Id = res;
        return res;
    }

    public Usuario? FindById(int id)
    {
        var query = @"SELECT id, nombre, apellido, email, password, role, avatar_url, estado 
                    FROM usuario 
                    WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Usuario
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre"),
                Apellido = reader.GetString("apellido"),
                Email = reader.GetString("email"),
                Password = reader.GetString("password"),
                Role = reader.GetString("role"),
                AvatarUrl = reader["avatar_url"] as string,
                Estado = reader.GetBoolean("estado")
            };
        }

        return null;
    }

    public List<Usuario> ListAll(int page = 1, int limit = 10)
    {
        List<Usuario> lista = [];
        var query = $@"SELECT id, nombre, apellido, email, password, role, avatar_url, estado 
                    FROM usuario 
                    ORDER BY apellido, nombre 
                    LIMIT {(page - 1) * limit}, {limit}";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Usuario
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre"),
                Apellido = reader.GetString("apellido"),
                Email = reader.GetString("email"),
                Password = reader.GetString("password"),
                Role = reader.GetString("role"),
                AvatarUrl = reader["avatar_url"] as string,
                Estado = reader.GetBoolean("estado")
            });
        }
        return lista;
    }

    public int Update(Usuario usuario)
    {
        var query = @"UPDATE usuario 
                    SET nombre = @nombre, apellido = @apellido, email = @email, 
                    password = @password, role = @role, avatar_url = @avatar_url, estado = @estado 
                    WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@id", usuario.Id);
        command.Parameters.AddWithValue("@nombre", usuario.Nombre);
        command.Parameters.AddWithValue("@apellido", usuario.Apellido);
        command.Parameters.AddWithValue("@email", usuario.Email);
        command.Parameters.AddWithValue("@password", usuario.Password);
        command.Parameters.AddWithValue("@role", usuario.Role);
        command.Parameters.AddWithValue("@avatar_url", usuario.AvatarUrl == null ? DBNull.Value : usuario.AvatarUrl);
        command.Parameters.AddWithValue("@estado", usuario.Estado);

        connection.Open();
        return command.ExecuteNonQuery();
    }

    public Usuario? FindByEmail(string email)
    {
        var query = @"SELECT id, nombre, apellido, email, password, role, avatar_url, estado 
                    FROM usuario 
                    WHERE email = @email";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@email", email);

        connection.Open();
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return new Usuario
            {
                Id = reader.GetInt32("id"),
                Nombre = reader.GetString("nombre"),
                Apellido = reader.GetString("apellido"),
                Email = reader.GetString("email"),
                Password = reader.GetString("password"),
                Role = reader.GetString("role"),
                AvatarUrl = reader["avatar_url"] as string,
                Estado = reader.GetBoolean("estado")
            };
        }

        return null;
    }


    public int Delete(int id)
    {
        var query = @"DELETE FROM usuario WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);
        command.Parameters.AddWithValue("@id", id);

        connection.Open();
        return command.ExecuteNonQuery();
    }

    public void UploadAvatar(IFormFile avatarFile, Usuario usuario)
    {
        if (avatarFile == null) return;

        if (!string.IsNullOrEmpty(usuario.AvatarUrl))
        {
            var rutaAnterior = Path.Combine(environment.WebRootPath, usuario.AvatarUrl.TrimStart('/'));
            if (File.Exists(rutaAnterior))
            {
                File.Delete(rutaAnterior);
            }
        }

        string guidNombre = Guid.NewGuid().ToString() + Path.GetExtension(avatarFile.FileName);
        string carpeta = Path.Combine(environment.WebRootPath, "Uploads", "Usuarios", usuario.Id.ToString());

        if (!Directory.Exists(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }

        string filePath = Path.Combine(carpeta, guidNombre);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            avatarFile.CopyTo(stream);
        }

        usuario.AvatarUrl = $"/Uploads/Usuarios/{usuario.Id}/{guidNombre}";
        var query = @"UPDATE usuario SET avatar_url = @avatar_url WHERE id = @id";

        using MySqlConnection connection = new(connectionString);
        using MySqlCommand command = new(query, connection);

        command.Parameters.AddWithValue("@avatar_url", usuario.AvatarUrl);
        command.Parameters.AddWithValue("@id", usuario.Id);

        connection.Open();
        command.ExecuteNonQuery();
    }




}
