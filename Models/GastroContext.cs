using Microsoft.EntityFrameworkCore;

namespace GastroApi.Models;

public class GastroContext : DbContext
{
    public GastroContext(DbContextOptions<GastroContext> options)
        :base(options)
        {

        }
        public DbSet<GastroItem> GastroItems {get;set;} = null!;
}