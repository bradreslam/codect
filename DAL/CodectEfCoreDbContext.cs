using Microsoft.EntityFrameworkCore;
using Codect.Classes;
using BLL.Models;

namespace BLL.Database
{
	public class CodectEfCoreDbContext : DbContext
	{
		public CodectEfCoreDbContext(DbContextOptions<CodectEfCoreDbContext> options) : base(options)
		{
		}

		public DbSet<Component> Components { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Configure ContactPoints as before
			modelBuilder.Entity<Component>()
				.Property(c => c.ContactPoints)
				.HasConversion(
					v => string.Join(',', v),  // Convert list to a comma-separated string
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
						.Select(s => Enum.Parse<ContactPoint>(s))
						.ToList()
				);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)  // Ensure options are not already set
			{
				// Add a default connection string for local development, this can be changed
				optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CodectDb;Trusted_Connection=True;");
			}
		}
	}
}
