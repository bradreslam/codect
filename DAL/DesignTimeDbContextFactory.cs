using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CodectEfCoreDbContext>
	{
		public CodectEfCoreDbContext CreateDbContext(string[] args)
		{
			// Set the correct base path to the project where your appsettings.json is located (Codect project in this case)
			var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "Codect");

			// Ensure that the base path exists and that the configuration file can be found
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(basePath) // Set the base path
				.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true) // Adjust based on your environment
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<CodectEfCoreDbContext>();

			// Ensure that the connection string is correctly set in the appsettings.json file
			var connectionString = configuration.GetConnectionString("CodectEfCoreDbContext");

			if (string.IsNullOrEmpty(connectionString))
			{
				throw new InvalidOperationException("Could not find a connection string named 'CodectEfCoreDbContext'.");
			}

			// Configure the DbContext to use SQL Server
			optionsBuilder.UseSqlServer(connectionString);

			return new CodectEfCoreDbContext(optionsBuilder.Options);
		}
	}
}
