using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EasyAbp.BookingService.EntityFrameworkCore;

public class BookingServiceHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<BookingServiceHttpApiHostMigrationsDbContext>
{
    public BookingServiceHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<BookingServiceHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("BookingService"));

        return new BookingServiceHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
