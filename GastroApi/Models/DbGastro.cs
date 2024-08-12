using Microsoft.EntityFrameworkCore;

namespace GastroApi.Models;

public class DbGastro : DbContext
{
    public DbGastro(DbContextOptions<DbGastro> options)
        : base(options){

        }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder
        // .Entity<GastroItem>()
        // .OwnsOne(item => item.Data, builder => {builder.ToJson();});

        modelBuilder.Entity<GastroItem>()
        .Property(p=>p.data)
        .HasColumnType("jsonb")
        .IsRequired();
    }

    public DbSet<GastroItem> gastroitems { get; set; } = null!;
}