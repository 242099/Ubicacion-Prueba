using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Conectar a SQL Server usando la cadena que acabamos de configurar
var conexion = builder.Configuration.GetConnectionString("ConexionSQL");
builder.Services.AddDbContext<AppDbContext>(opciones =>
    opciones.UseSqlServer(conexion));

var app = builder.Build();

// 2. Crear el endpoint que recibirá las coordenadas desde .NET MAUI
app.MapPost("/api/ubicaciones", async (Ubicacion ubi, AppDbContext db) =>
{
    db.Ubicaciones.Add(ubi);
    await db.SaveChangesAsync();
    return Results.Ok(ubi);
});

app.Run();

// 3. Modelos de datos para Entity Framework
public class Ubicacion
{
    public int Id { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    // Nota: No incluimos FechaRegistro porque SQL Server la genera sola (GETDATE())
}

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opciones) : base(opciones) { }
    public DbSet<Ubicacion> Ubicaciones { get; set; }
}