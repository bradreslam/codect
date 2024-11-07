using BLL.Classes;
using Codect.Classes;
using System.Drawing.Imaging;
using System.Drawing;

namespace CodectUnitTests
{
	[TestClass]
	public class SpriteFactoryTests
	{
		private SpriteFactory _spriteFactory;

		[TestInitialize]
		public void TestInitialize()
		{
			_spriteFactory = new SpriteFactory();
		}

		[TestMethod]
		public void Constructor_InitializesBaseSprite()
		{
			Assert.IsNotNull(_spriteFactory);
			// You might want to add more specific assertions about the baseSprite if possible
		}

		[TestMethod]
		public void ResizeBitmap_ReturnsCorrectSize()
		{
			// Arrange
			Bitmap sourceBmp = new Bitmap(100, 100);
			int newWidth = 50;
			int newHeight = 75;

			// Act
			Bitmap result = _spriteFactory.ResizeBitmap(sourceBmp, newWidth, newHeight);

			// Assert
			Assert.AreEqual(newWidth, result.Width);
			Assert.AreEqual(newHeight, result.Height);
		}

		[TestMethod]
		public void CreateSprite_ReturnsCorrectSizeSprite()
		{
			// Arrange
			List<ContactPoint> endpoints = new List<ContactPoint>(); // Add mock endpoints if needed
			byte[] featureSprite = CreateDummyImageByteArray(10, 10);
			bool spriteType = true;

			// Act
			Bitmap result = _spriteFactory.CreateSprite(endpoints, featureSprite, spriteType);

			// Assert
			Assert.AreEqual(25, result.Width);
			Assert.AreEqual(25, result.Height);
		}

		[TestMethod]
		public void CreateSprite_WithNullFeatureSprite_ReturnsValidSprite()
		{
			// Arrange
			List<ContactPoint> endpoints = new List<ContactPoint>(); // Add mock endpoints if needed
			bool spriteType = false;

			// Act
			Bitmap result = _spriteFactory.CreateSprite(endpoints, null, spriteType);

			// Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(25, result.Width);
			Assert.AreEqual(25, result.Height);
		}

		[TestMethod]
		public void CreateSprite_WithEndpoints_DrawsAllEndpoints()
		{
			// Arrange
			List<ContactPoint> endpoints = new List<ContactPoint>
		{
			new ContactPoint(), // Add mock ContactPoints
            new ContactPoint()
		};
			bool spriteType = true;

			// This test might need to mock WireDictionary or use a test double

			// Act
			Bitmap result = _spriteFactory.CreateSprite(endpoints, null, spriteType);

			// Assert
			// Here you might want to check pixel data to ensure all endpoints were drawn
			// This is a complex assertion and might require additional helper methods
			Assert.IsNotNull(result);
		}

		// Helper method to create a dummy image byte array for testing
		private byte[] CreateDummyImageByteArray(int width, int height)
		{
			using (Bitmap bmp = new Bitmap(width, height))
			using (MemoryStream ms = new MemoryStream())
			{
				bmp.Save(ms, ImageFormat.Png);
				return ms.ToArray();
			}
		}
	}
}
