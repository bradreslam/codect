using Svg;
using BLL.Models;
using BLL.Classes;
using Codect.Classes;

public class SpriteFactory
{
	private readonly FeatureDictionary featureDictionary = new();

	public string CreateSprite(List<ContactPoint> endpoints, string feature, bool spriteType)
	{
		var resultSprite = new SvgDocument();
		resultSprite.Children.Add(SvgResourceManager.GetPreloadedSvgDocument("Component_Background"));

		foreach (ContactPoint endPoint in endpoints)
		{
			var state = spriteType ? "On" : "Off";
			var key = $"{endPoint}_Wire_{state}";

			resultSprite.Children.Add(SvgResourceManager.GetPreloadedSvgDocument(key));
		}

		if (!string.IsNullOrEmpty(feature))
		{
			var featureModel = featureDictionary.GetFeatureModel(feature);
			SvgDocument featureSprite;
			if (spriteType)
			{
				featureSprite = featureModel.onSprite;
			}
			else
			{
				featureSprite = featureModel.offSprite;
			}
			
			resultSprite.Children.Add(featureSprite);
		}

		return resultSprite.GetXML();
	}
}