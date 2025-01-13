using Svg;
using BLL.Models;
using BLL;
using BLL.Classes;
using Codect.Classes;
using System.Drawing;


public class SpriteFactory
{
	private SvgResourceManager srm = new();
	private readonly FeatureDictionary featureDictionary = new();

	public SvgDocument CreateSprite(List<ContactPoint> endpoints, string feature, bool spriteType)
	{
		SvgDocument resultSprite = new SvgDocument();
		resultSprite.Children.Add(SvgResourceManager.GetPreloadedSvgDocument("Component_Background"));
		List<SvgDocument> endpointSprites = new();

		foreach (ContactPoint endPoint in endpoints)
		{
			string state = spriteType ? "On" : "Off";
			string key = $"{endPoint}_Wire_{state}";

			resultSprite.Children.Add(SvgResourceManager.GetPreloadedSvgDocument(key));
		}

		if (!string.IsNullOrEmpty(feature))
		{
			FeatureModel featureModel = featureDictionary.GetFeatureModel(feature);
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

		return resultSprite;
	}
}