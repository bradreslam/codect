using System.Data;
using BLL.Classes;
using DTO;
using Interfaces;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace CodectUnitTests
{
	[TestClass]
	public class ComponentManagerTest
	{
		private readonly Mock<IComponentRepository> _mockIComponent;
		private readonly ComponentManager _componentManager;

		private List<string> contactPoints = new()
		{
			"E",
			"N",
			"S"
		};

		public ComponentManagerTest()
		{
			_mockIComponent = new();

			_componentManager = new(_mockIComponent.Object);
		}

		[Fact]
		public void GetAllComponentIds_returns_a_list_of_ids_when_called()
		{
			// Arrange
			List<string> idList = new()
			{
				"0110RedLed",
				"1111RedLed",
				"0111"
			};
			_mockIComponent.Setup(m => m.GetAllComponentIds()).Returns(idList);

			// Act
			var result = _componentManager.GetAllComponentIds();

			// Assert
			Assert.IsType<List<string>>(result);
			Assert.Equal(idList, result);
		}

		[Fact]
		public void InsertComponentIntoDatabase_returns_an_id_when_succeeding()
		{
			// Arrange
			string feature = "";
			string id = "1110";
			_mockIComponent.Setup(m => m.InsertComponentInDatabase(contactPoints, feature)).Returns(id);

			// Act
			var result = _componentManager.InsertComponentInDatabase(contactPoints, feature);

			// Assert
			Assert.IsType<string>(result);
			Assert.Equal(id, result);

		}

		[Fact]
		public void InsertComponentIntoDatabase_returns_an_error_when_component_already_exists()
		{
			// Arrange
			string feature = "";
			string errorMessage = "Component already exists in the database";
			_mockIComponent.Setup(m => m.InsertComponentInDatabase(contactPoints, feature)).Throws(new DataException(errorMessage));

			// Act
			var result = Record.Exception(() => _componentManager.InsertComponentInDatabase(contactPoints, feature));

			// Assert
			Assert.IsType<DataException>(result);
			Assert.Equal(errorMessage, result.Message);
		}

		[Fact]
		public void IdExistsInDatabase_returns_true_when_it_exists()
		{
			// Arrange
			string id = "1011RedLed";
			_mockIComponent.Setup(m => m.IdExistsInDatabase(id)).Returns(true);

			// Act
			var result = _componentManager.IdExistsInDatabase(id);

			// Assert
			Assert.IsType<bool>(result);
			Assert.Equal(true, result);
		}

		[Fact]
		public void IdExistsInDatabase_returns_false_when_it_does_not_exists()
		{
			// Arrange
			string id = "fakeId";
			_mockIComponent.Setup(m => m.IdExistsInDatabase(id)).Returns(false);

			// Act
			var result = _componentManager.IdExistsInDatabase(id);

			// Assert
			Assert.IsType<bool>(result);
			Assert.Equal(false, result);

		}

		[Fact]
		public void GetComponentBasedOnId_returns_a_component_when_id_exists()
		{
			// Arrange
			string id = "0011RedLed";
			string feature = "RedLed";
			ComponentDTO componentDto = new()
			{
				contactPoints = contactPoints,
				feature = feature
			};
			_mockIComponent.Setup(m => m.GetComponentBasedOnId(id)).Returns(componentDto);
			// Act
			var result = _componentManager.GetComponentBasedOnId(id);

			// Assert
			Assert.IsType<ComponentDTO>(result);
			Assert.Equal(feature, result.feature);
			Assert.Equal(contactPoints, result.contactPoints);
		}

		[Fact]
		public void GetComponentBasedOnId_returns_a_error_when_id_does_not_exists()
		{
			// Arrange
			string id = "fakeId";
			string errorMessage = "Component does not exist in database";
			_mockIComponent.Setup(m => m.GetComponentBasedOnId(id)).Throws(new DataException(errorMessage));
			// Act
			var result = Record.Exception(() => _componentManager.GetComponentBasedOnId(id));

			// Assert
			Assert.IsType<DataException>(result);
			Assert.Equal(errorMessage, result.Message);
		}
	}
}
