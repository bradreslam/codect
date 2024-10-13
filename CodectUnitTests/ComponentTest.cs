using BLL.Exceptions;
using BLL.Models;
using Codect.Classes;

namespace CodectUnitTests
{
	[TestClass]
	public class ComponentTest
	{
		[TestMethod]
		public void Constructor_trows_exception_when_name_is_longer_than_30_characters()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);
			contactPoints.Add(ContactPoint.E);

			// Act & Assert
			Xunit.Assert.Throws<ComponentExceptions>(() =>
				new Component("testNameThatIsWayToLongForTheConstructor",contactPoints, FeatureType.Led));
		}

		[TestMethod]
		public void Constructor_trows_exception_when_name_is_shorter_than_3_characters()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);
			contactPoints.Add(ContactPoint.E);

			// Act & Assert
			Xunit.Assert.Throws<ComponentExceptions>(() =>
				new Component("tn", contactPoints, FeatureType.Led));
		}

		[TestMethod]
		public void Constructor_trows_exception_when_there_is_more_than_one_of_the_same_contactPoint()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);
			contactPoints.Add(ContactPoint.N);

			// Act & Assert
			Xunit.Assert.Throws<ComponentExceptions>(() =>
				new Component("testName", contactPoints, FeatureType.Led));
		}

		[TestMethod]
		public void Constructor_trows_exception_when_there_is_less_than_two_contactPoints()
		{
			// Arrange
			List<ContactPoint> contactPoints = new List<ContactPoint>();
			contactPoints.Add(ContactPoint.N);

			// Act & Assert
			Xunit.Assert.Throws<ComponentExceptions>(() =>
				new Component("testName", contactPoints, FeatureType.Led));
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

			// Act
			Component component = new Component(name, contactPoints, feature);

			//Assert
			Assert.AreEqual(name, component.Name);
			Assert.AreEqual(contactPoints, component.ContactPoints);
			Assert.AreEqual(feature, component.Feature);
		}
	}
}