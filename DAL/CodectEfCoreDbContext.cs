using Microsoft.EntityFrameworkCore;
using Codect.Classes;
using BLL.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DAL
{
	public class CodectEfCoreDbContext : DbContext
	{
		public CodectEfCoreDbContext(DbContextOptions<CodectEfCoreDbContext> options) : base(options)
		{
		}

		// Parameterless constructor for testing
		public CodectEfCoreDbContext()
			: base(new DbContextOptionsBuilder<CodectEfCoreDbContext>().Options) { }

		public virtual DbSet<Component> Components { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var contactPointsComparer = new ValueComparer<List<ContactPoint>>(
				(c1, c2) => c1.SequenceEqual(c2),                          // Equality check
				c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v)),     // Hash code
				c => c.ToList()                                            // Cloning logic
			);

			// Configure ContactPoints as a comma-separated string with a ValueComparer
			modelBuilder.Entity<Component>()
				.Property(c => c.ContactPoints)
				.HasConversion(
					v => string.Join(',', v),  // Convert list to a comma-separated string
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries)
						.Select(s => Enum.Parse<ContactPoint>(s))
						.ToList()
				)
				.Metadata.SetValueComparer(contactPointsComparer);

			base.OnModelCreating(modelBuilder);
		}
	}
}
