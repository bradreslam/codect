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
			_mockContext = new Mock<CodectEfCoreDbContext>();
			_mockDbSet = new Mock<DbSet<Component>>();

			_mockContext.Setup(m => m.Components).Returns(_mockDbSet.Object);
			_repository = new ComponentRepository(_mockContext.Object);
		}

		private static Mock<DbSet<T>> CreateMockDbSet<T>(IQueryable<T> data) where T : class
		{
			var mockSet = new Mock<DbSet<T>>();
			mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
			mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
			mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
			mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
			return mockSet;
		}

		[Fact]
		public void GetAllComponentIds_ShouldReturnListOfComponentIds()
		{
			// Arrange
			var components = new List<Component>
		{
			new(contactPointlist, "RedLed") { Id = "1" },
			new(contactPointlist, "RedLed") { Id = "2" }
		}.AsQueryable();
			var mockSet = CreateMockDbSet(components);

			var mockContext = new Mock<CodectEfCoreDbContext>();
			mockContext.Setup(c => c.Components).Returns(mockSet.Object);

			var repository = new ComponentRepository(mockContext.Object);

			// Act
			var result = repository.GetAllComponentIds();

			// Assert
			Xunit.Assert.Equal(2, result.Count);
			Xunit.Assert.Contains("1", result);
			Xunit.Assert.Contains("2", result);
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
			Xunit.Assert.Equal("RedLed", result.Feature);
			Xunit.Assert.Equal(2, result.ContactPoints.Count);
			Xunit.Assert.Contains("E", result.ContactPoints);
			Xunit.Assert.Contains("N", result.ContactPoints);
		}

		[Fact]
		public void GetComponentBasedOnId_WhenIdDoesNotExist_ShouldThrowException()
		{
			// Arrange
			_mockDbSet.Setup(m => m.Find(It.IsAny<string>())).Returns((Component)null);

			// Act & Assert
			Xunit.Assert.Throws<NullReferenceException>(() => _repository.GetComponentBasedOnId("1"));
		}
	}
}
