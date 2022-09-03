using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EasyAbp.BookingService.EntityFrameworkCore;

public class BookingServiceOrleansHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<BookingServiceOrleansHttpApiHostMigrationsDbContext>
{
    public BookingServiceOrleansHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<BookingServiceOrleansHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("BookingService"));

        return new BookingServiceOrleansHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
