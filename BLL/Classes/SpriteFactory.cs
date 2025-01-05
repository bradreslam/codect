using Svg;
using BLL.Models;
using BLL;
using Codect.Classes;


public class SpriteFactory
{
	private SvgDocument baseSprite;

	public SpriteFactory()
	{
		using MemoryStream ms = new(System.Text.Encoding.UTF8.GetBytes(Resource1.Component_Background));
		baseSprite = SvgDocument.Open<SvgDocument>(ms);
	}

	public SvgDocument CreateSprite(List<ContactPoint> endpoints, string feature, bool spriteType)
	{
		SvgDocument resultSprite = new SvgDocument();
		resultSprite.Children.Add(baseSprite.DeepCopy());
		List<SvgDocument> endpointSprites = new();
		WireDictionary wd = new();

		foreach (ContactPoint endPoint in endpoints)
		{
			string wireSvgString = wd.GetWireSprite(endPoint, spriteType);
			using (MemoryStream ms = new(System.Text.Encoding.UTF8.GetBytes(wireSvgString)))
			{
				SvgDocument endpointSvg = SvgDocument.Open<SvgDocument>(ms);
				endpointSprites.Add(endpointSvg);
			}
		}

		foreach (SvgDocument sprite in endpointSprites)
		{
			resultSprite.Children.Add(sprite.DeepCopy());
		}

		if (!string.IsNullOrEmpty(feature))
		{
			FeatureDictionary fd = new();
			FeatureModel featureModel = fd.GetFeatureModel(feature);
			string featureSprite;
			if (spriteType)
			{
				featureSprite = featureModel.onSprite;
			}
			else
			{
				featureSprite = featureModel.offSprite;
			}
			using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(featureSprite)))
			{
				SvgDocument featureSvg = SvgDocument.Open<SvgDocument>(ms);
				resultSprite.Children.Add(featureSvg.DeepCopy());
			}
		}

		return resultSprite;
	}
}