using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using RentalManagement.Data;

public class FlicksDbContextFactory : IDesignTimeDbContextFactory<FlicksDbContext>
{
    public FlicksDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FlicksDbContext>();

        // Direct connection string (only used at design-time for migrations)
        optionsBuilder.UseSqlServer(
            "Server=10.0.0.106,1433;Database=MovieRental;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
        );

        return new FlicksDbContext(optionsBuilder.Options);
    }
}

