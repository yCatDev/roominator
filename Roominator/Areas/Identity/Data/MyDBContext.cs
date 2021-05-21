using Microsoft.EntityFrameworkCore;

public class MyDbContext : DbContext
{
    public MyDbContext()
    {
        
    }

    //public DbContextOptions CreateDbContext(string[] args)
    //{
    //    IConfigurationRoot configuration = new ConfigurationBuilder()
    //        .SetBasePath(Directory.GetCurrentDirectory())
    //        .AddJsonFile("appsettings.json")
    //        .Build();
    //    var builder = new DbContextOptionsBuilder();
    //    var connectionString = configuration.GetConnectionString("DataAccessPostgreSqlProvider");
    //    builder.UseNpgsql(connectionString);
    //    return builder.Options;
    //}
}
