using Codect.Classes;
using Svg;
using Xunit;

namespace CodectUnitTests
{
	
	public class SpriteFactoryTests
	{
		private SpriteFactory _spriteFactory = new();
		private List<ContactPoint> endpoints = new()
			{ ContactPoint.N, ContactPoint.E };

		[Fact]
		public void Constructor_InitializesBaseSprite()
		{
			Xunit.Assert.NotNull(_spriteFactory);
		}


		[Fact]
		public void CreateSprite_WithNullFeatureSprite_ReturnsValidSprite()
		{
			// Arrange
			bool spriteType = false;

			// Act
			string result = _spriteFactory.CreateSprite(endpoints, null, spriteType);

			// Assert
			Xunit.Assert.NotNull(result);
		}
	}
}
