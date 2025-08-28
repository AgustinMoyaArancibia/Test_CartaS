using Domain.Entities;
using Domain.Entitties;
using Microsoft.EntityFrameworkCore;



namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Sucursal> Sucursales => Set<Sucursal>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Venta> Ventas => Set<Venta>();       
    public DbSet<VentaDetalle> VentaDetalles => Set<VentaDetalle>();

    protected override void OnModelCreating(ModelBuilder model)
    {
        base.OnModelCreating(model);

        model.Entity<Usuario>(e =>
        {
            e.ToTable("Usuarios");
            e.HasKey(x => x.IdUsuario);
            e.Property(x => x.Nombre).HasMaxLength(50).IsRequired();
            e.Property(x => x.Pass).HasMaxLength(500).IsRequired();
        
            e.HasIndex(x => x.Nombre).IsUnique();
        });
        // === Clientes ===
        model.Entity<Cliente>(e =>
        {
            e.ToTable("Clientes");
            e.HasKey(x => x.IdCliente);
            e.Property(x => x.Dni)
                .HasMaxLength(10)
                .IsRequired();
            e.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();
            e.Property(x => x.DireccionEnvio)
                .HasMaxLength(100);

            e.HasIndex(x => x.Dni).IsUnique();
            e.HasCheckConstraint("CK_Clientes_Dni", "LEN([Dni]) BETWEEN 7 AND 10 AND [Dni] NOT LIKE '%[^0-9]%'");
        });

        // === Empleados ===
        model.Entity<Empleado>(e =>
        {
            e.ToTable("Empleados");
            e.HasKey(x => x.IdEmpleado);
            e.Property(x => x.Nombre).HasMaxLength(100).IsRequired();
            e.Property(x => x.Activo).HasDefaultValue(true);
        });

        // === Sucursales ===
        model.Entity<Sucursal>(e =>
        {
            e.ToTable("Sucursales");
            e.HasKey(x => x.IdSucursal);
            e.Property(x => x.Direccion)
                .HasMaxLength(100)
                .IsRequired();
            e.Property(x => x.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            e.HasIndex(x => new { x.Direccion, x.Nombre }).IsUnique();
        });

        // === Productos ===
        model.Entity<Producto>(e =>
        {
            e.ToTable("Productos");
            e.HasKey(x => x.IdProducto);
            e.Property(x => x.Nombre)
                .HasMaxLength(20)
                .IsRequired();
            e.HasIndex(x => x.Nombre).IsUnique();
        });

        // === Ventas (cabecera) -> tabla Ventas_N ===
        model.Entity<Venta>(e =>
        {
            e.ToTable("Ventas_N");
            e.HasKey(x => x.IdVenta);

            e.Property(x => x.FechaVenta)
                .IsRequired();

            e.Property(x => x.ImporteTotal)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            e.HasOne(x => x.Cliente)
                .WithMany(c => c.Ventas)
                .HasForeignKey(x => x.IdCliente)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ventas_Cliente");

            e.HasOne(x => x.Empleado)
                .WithMany(emp => emp.Ventas)
                .HasForeignKey(x => x.IdEmpleado)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ventas_Empleado");

            e.HasOne(x => x.Sucursal)
                .WithMany(s => s.Ventas)
                .HasForeignKey(x => x.IdSucursal)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Ventas_Sucursal");

            e.HasIndex(x => x.FechaVenta).HasDatabaseName("IX_VentasN_FechaVenta");
            e.HasIndex(x => x.IdCliente).HasDatabaseName("IX_VentasN_IdCliente");
            e.HasIndex(x => x.IdEmpleado).HasDatabaseName("IX_VentasN_IdEmpleado");
            e.HasIndex(x => x.IdSucursal).HasDatabaseName("IX_VentasN_IdSucursal");

            e.HasCheckConstraint("CK_Ventas_ImporteTotal", "[ImporteTotal] >= 0");
        });

        // === VentaDetalle ===
        model.Entity<VentaDetalle>(e =>
        {
            e.ToTable("VentaDetalle");
            e.HasKey(x => x.IdDetalle);

            e.Property(x => x.Cantidad).IsRequired();
            e.Property(x => x.PrecioUnitario)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            // Columna calculada persistida (igual que en SQL)
            e.Property(x => x.SubTotal)
                .HasColumnType("decimal(19,2)")
                .HasComputedColumnSql("[Cantidad] * [PrecioUnitario]", stored: true);

            e.HasOne(x => x.Venta)
                .WithMany(v => v.Detalles)
                .HasForeignKey(x => x.IdVenta)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_VentaDetalle_Venta");

            e.HasOne(x => x.Producto)
                .WithMany(p => p.VentaDetalles)
                .HasForeignKey(x => x.IdProducto)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_VentaDetalle_Producto");

            e.HasIndex(x => x.IdVenta).HasDatabaseName("IX_Detalle_IdVenta");
            e.HasIndex(x => x.IdProducto).HasDatabaseName("IX_Detalle_IdProducto");

            e.HasCheckConstraint("CK_VentaDetalle_Cantidad", "[Cantidad] > 0");
            e.HasCheckConstraint("CK_VentaDetalle_PrecioUnitario", "[PrecioUnitario] >= 0");
        });
    }
}
