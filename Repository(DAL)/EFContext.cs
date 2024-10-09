using Microsoft.EntityFrameworkCore;
using Repository_DAL_.Model;

namespace Repository_DAL_;

public partial class EFContext : DbContext
{
    public EFContext( DbContextOptions<EFContext> options ) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<ControlMaster> ControlMaster { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Orders> Orders { get; set; }


    public DbSet<ApiLog> ApiLog { get; set; }
}