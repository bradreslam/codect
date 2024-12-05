using BLL.Models;
using Codect.Classes;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace CodectUnitTests
{
	[TestClass]
	public class ComponentTest
	{
		[Fact]
		public void Component_gives_error_if_feature_is_not_in_dictionary()
		{
			// Arrange
			List<ContactPoint> contactPoints = new() { ContactPoint.E, ContactPoint.N };
			string invalidFeature = "InvalidFeature";

			// Act & Assert
			var exception = Xunit.Assert.Throws<ValidationException>(() =>
			{
				new Component(contactPoints, invalidFeature);
			});

			Xunit.Assert.Equal("Feature has to exist in dictionary", exception.Message);
		}

		[Fact]
		public void Component_gives_error_if_contact_point_list_is_shorter_than_2()
		{
			// Arrange
			List<ContactPoint> contactPoints = new() { ContactPoint.N };
			string validFeature = "RedLed";

			// Act & Assert
			var exception = Xunit.Assert.Throws<ValidationException>(() =>
			{
				new Component(contactPoints, validFeature);
			});

			Xunit.Assert.Equal("There can be no less than 2 contact points", exception.Message);
		}

		[Fact]
		public void Component_gives_error_if_contact_point_list_contains_double_values()
		{
			// Arrange
			List<ContactPoint> contactPoints = new()
		{
			ContactPoint.N, ContactPoint.N
		};
			string validFeature = "RedLed";

			// Act & Assert
			var exception = Xunit.Assert.Throws<ValidationException>(() =>
			{
				new Component(contactPoints, validFeature);
			});

			Xunit.Assert.Equal("The list contains duplicate values.", exception.Message);
		}

		[Fact]
		public void Component_id_gets_created_correctly()
		{
			// Arrange
			List<ContactPoint> contactPoints = new() { ContactPoint.N, ContactPoint.E };
			string validFeature = "RedLed";

			// Act
			Component component = new(contactPoints, validFeature);

			// Assert
			Xunit.Assert.Equal("1100RedLed", component.Id);
		}

		[Fact]
		public void Component_gets_initialized_correctly_when_nothing_is_wrong()
		{
			// Arrange
			List<ContactPoint> contactPoints = new() { ContactPoint.N, ContactPoint.S };
			string validFeature = "RedLed";

			// Act
			Component component = new(contactPoints, validFeature);

			// Assert
			Xunit.Assert.NotNull(component);
			Xunit.Assert.Equal(contactPoints, component.ContactPoints);
			Xunit.Assert.Equal(validFeature, component.Feature);
		}
	}
}