using BLL.Classes;

namespace BLL.Models
{
	internal class RedLed : FeatureModel
	{
		public RedLed()
		{
			description = "An simple red led light will light up when provided with power";
			offSprite = SvgResourceManager.GetSvgDocument("Led_Off");
			onSprite = SvgResourceManager.GetSvgDocument("Red_Led_On");
		}
	}
}
