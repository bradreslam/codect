using BLL.Models;
using Codect.Classes;
using Microsoft.EntityFrameworkCore;
using DAL;
using Moq;
using Xunit;

namespace CodectUnitTests
{
	public class ComponentRepositoryTests
	{
		private readonly Mock<CodectEfCoreDbContext> _mockContext;
		private readonly Mock<DbSet<Component>> _mockDbSet;
		private readonly ComponentRepository _repository;
		private List<ContactPoint> contactPointlist = new()
			{ ContactPoint.E, ContactPoint.N };

		public ComponentRepositoryTests()
		{
			var DbcontextOptions = new DbContextOptionsBuilder<CodectEfCoreDbContext>()
				.UseSqlServer("Server=(localdb)\\MSSQLLocalDB; Database=CodectTestDb; Trusted_Connection=true; Trust Server Certificate=true; MultipleActiveResultSets=true; Integrated Security=true;")
				.Options;

			_mockContext = new Mock<CodectEfCoreDbContext>(DbcontextOptions);
			_mockDbSet = new Mock<DbSet<Component>>();

			_mockContext.Setup(m => m.Components).Returns(_mockDbSet.Object);
			_repository = new ComponentRepository(_mockContext.Object);
		}

		[Fact]
		public void InsertComponentInDatabase_ShouldAddComponent()
		{
			// Arrange
			var contactPoints = new List<string> { "E", "N" };
			string feature = "RedLed";

			_mockDbSet.Setup(m => m.Add(It.IsAny<Component>()));
			_mockContext.Setup(m => m.SaveChanges()).Returns(1);

			// Act
			_repository.InsertComponentInDatabase(contactPoints, feature);

			// Assert
			_mockDbSet.Verify(m => m.Add(It.IsAny<Component>()), Times.Once);
			_mockContext.Verify(m => m.SaveChanges(), Times.Once);
		}

		[Fact]
		public void IdExistsInDatabase_WhenIdExists_ShouldReturnTrue()
		{
			// Arrange
			var component = new Component(contactPointlist, "RedLed") { Id = "1" };
			_mockDbSet.Setup(m => m.Find(It.IsAny<string>())).Returns(component);

			// Act
			var result = _repository.IdExistsInDatabase("1");

			// Assert
			Xunit.Assert.True(result);
		}

		[Fact]
		public void IdExistsInDatabase_WhenIdDoesNotExist_ShouldReturnFalse()
		{
			// Arrange
			_mockDbSet.Setup(m => m.Find(It.IsAny<string>())).Returns((Component)null);

			// Act
			var result = _repository.IdExistsInDatabase("1");

			// Assert
			Xunit.Assert.False(result);
		}

		[Fact]
		public void GetComponentBasedOnId_WhenIdExists_ShouldReturnComponentDTO()
		{
			// Arrange
			var component = new Component(contactPointlist, "RedLed") { Id = "1" };

			_mockDbSet.Setup(m => m.Find(It.IsAny<string>())).Returns(component);

			// Act
			var result = _repository.GetComponentBasedOnId("1");

			// Assert
			Xunit.Assert.NotNull(result);
			Xunit.Assert.Equal("RedLed", result.feature);
			Xunit.Assert.Equal(2, result.contactPoints.Count);
			Xunit.Assert.Contains("E", result.contactPoints);
			Xunit.Assert.Contains("N", result.contactPoints);
		}

		[Fact]
		public void GetComponentBasedOnId_WhenIdDoesNotExist_ShouldThrowException()
		{
			// Arrange
			_mockDbSet.Setup(m => m.Find(It.IsAny<string>())).Returns((Component)null);

			// Act & Assert
			Xunit.Assert.Throws<FileNotFoundException>(() => _repository.GetComponentBasedOnId("1"));
		}
	}
}
