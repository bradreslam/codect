using BLL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DAL
{
	public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CodectEfCoreDbContext>
	{
		public CodectEfCoreDbContext CreateDbContext(string[] args)
		{
			// Assuming your API project is in a folder named "Api" at the same level as DAL
			var basePath = Path.Combine(Directory.GetCurrentDirectory(), "./Codect");

			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(basePath)
				.AddJsonFile("appsettings.Development.json")
				.Build();

			var optionsBuilder = new DbContextOptionsBuilder<CodectEfCoreDbContext>();
			var connectionString = configuration.GetConnectionString("CodectEfCoreDbContext");
			optionsBuilder.UseSqlServer(connectionString);

			return new CodectEfCoreDbContext(optionsBuilder.Options);
		}
	}
}
