using Microsoft.EntityFrameworkCore;

namespace TpFinal_Lab2.Models;

public class DataContext : DbContext
{
    
    public DbSet<Noticia> Noticias { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options) 
    { 
    }
}