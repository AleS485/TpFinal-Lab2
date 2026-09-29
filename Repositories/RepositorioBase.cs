using Microsoft.Extensions.Configuration;

namespace TpFinal_Lab2.Repositories;

public abstract class RepositorioBase(IConfiguration configuration)
{
    protected readonly IConfiguration configuration = configuration;
    protected readonly string? connectionString = configuration["ConnectionStrings:DefaultConnection"];
}
