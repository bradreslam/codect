using BLL.Exceptions;
using BLL.Models;
using Codect.Classes;
using DTO;
using Interfaces;
using Moq;

namespace CodectUnitTests
{
	[TestClass]
	public class ComponentTest
	{
		private readonly Mock<IComponentRepository> _mockComponentRepository;

		public ComponentTest()
		{
			_mockComponentRepository = new Mock<IComponentRepository>();
		}

		[TestMethod]
		public void Constructor_trows_exception_when_name_is_longer_than_30_characters()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);
			contactPoints.Add(ContactPoint.E);
			_mockComponentRepository.Setup(g => g.NameExistsInDatabase(It.IsAny<string>())).Returns(false);

			// Act & Assert
			Xunit.Assert.Throws<ComponentExceptions>(() =>
				new Component("testNameThatIsWayToLongForTheConstructor",contactPoints, FeatureType.Led, _mockComponentRepository.Object));
		}

		[TestMethod]
		public void Constructor_trows_exception_when_name_is_shorter_than_3_characters()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);
			contactPoints.Add(ContactPoint.E);
			_mockComponentRepository.Setup(g => g.NameExistsInDatabase(It.IsAny<string>())).Returns(false);

			// Act & Assert
			Xunit.Assert.Throws<ComponentExceptions>(() =>
				new Component("tn", contactPoints, FeatureType.Led, _mockComponentRepository.Object));
		}

		[TestMethod]
		public void Constructor_trows_exception_when_name_already_exists_in_the_database()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);
			contactPoints.Add(ContactPoint.E);
			_mockComponentRepository.Setup(g => g.NameExistsInDatabase(It.IsAny<string>())).Returns(true);

			// Act & Assert
			Xunit.Assert.Throws<ComponentExceptions>(() =>
				new Component("testName", contactPoints, FeatureType.Led, _mockComponentRepository.Object));
		}

		[TestMethod]
		public void Constructor_trows_exception_when_there_is_more_than_one_of_the_same_contactPoint()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);
			contactPoints.Add(ContactPoint.N);
			_mockComponentRepository.Setup(g => g.NameExistsInDatabase(It.IsAny<string>())).Returns(false);

			// Act & Assert
			Xunit.Assert.Throws<ComponentExceptions>(() =>
				new Component("testName", contactPoints, FeatureType.Led, _mockComponentRepository.Object));
		}

		[TestMethod]
		public void Constructor_trows_exception_when_there_is_less_than_two_contactPoints()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);
			_mockComponentRepository.Setup(g => g.NameExistsInDatabase(It.IsAny<string>())).Returns(false);

			// Act & Assert
			Xunit.Assert.Throws<ComponentExceptions>(() =>
				new Component("testName", contactPoints, FeatureType.Led, _mockComponentRepository.Object));
		}

		[TestMethod]
		public void Constructor_compiles_correctly_when_nothing_is_wrong()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);
			contactPoints.Add(ContactPoint.E);
			contactPoints.Add(ContactPoint.W);
			string name = "testName";
			FeatureType feature = FeatureType.Null;
			_mockComponentRepository.Setup(g => g.NameExistsInDatabase(It.IsAny<string>())).Returns(false);

			// Act
			Component component = new Component(name, contactPoints, feature, _mockComponentRepository.Object);

			//Assert
			Assert.AreEqual(name, component.Name);
			Assert.AreEqual(contactPoints, component.ContactPoints);
			Assert.AreEqual(feature, component.Feature);
		}
	}
}