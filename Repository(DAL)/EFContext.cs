using Microsoft.EntityFrameworkCore;
using Repository_DAL_.Model;

namespace Repository_DAL_;

public partial class EFContext : DbContext
{
    public EFContext( DbContextOptions<EFContext> options ) : base(options)
    {
    }
    public DbSet<User> users { get; set; }
    public DbSet<Category> categories { get; set; }
    public DbSet<Product> products { get; set; }
    public DbSet<Image> images { get; set; }
    public DbSet<ControlMaster> controlmaster { get; set; }
    public DbSet<Label> labels { get; set; }
    public DbSet<Reservation> reservations { get; set; }
    public DbSet<Orders> orders { get; set; }
    public DbSet<Address> address { get; set; }


    public DbSet<ApiLog> apilog { get; set; }
}