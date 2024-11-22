using Codect.Classes;
using Svg;

namespace CodectUnitTests
{
	[TestClass]
	public class SpriteFactoryTests
	{
		private SpriteFactory _spriteFactory;
		private List<ContactPoint> endpoints;

		[TestInitialize]
		public void TestInitialize()
		{
			_spriteFactory = new SpriteFactory();
			endpoints = new();
			endpoints.Add(ContactPoint.N);
			endpoints.Add( ContactPoint.E);
		}

		[TestMethod]
		public void Constructor_InitializesBaseSprite()
		{
			Assert.IsNotNull(_spriteFactory);
		}


		[TestMethod]
		public void CreateSprite_WithNullFeatureSprite_ReturnsValidSprite()
		{
			// Arrange
			bool spriteType = false;

			// Act
			SvgDocument result = _spriteFactory.CreateSprite(endpoints, null, spriteType);

			// Assert
			Assert.IsNotNull(result);
		}
	}
}
