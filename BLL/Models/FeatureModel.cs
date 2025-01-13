
using Svg;

namespace BLL.Models
{
	public class FeatureModel
	{
		public string description { get; set; }
		public SvgDocument offSprite { get; set; }
		public SvgDocument onSprite { get; set; }
	}
}
