using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
	public class FeatureDictionary
	{
		private readonly Dictionary<string, Func<FeatureModel>> featureName = new()
		{
			{ "RedLed", () => new RedLed() }
		};

		public FeatureModel GetFeatureModel(string name)
		{
			if (featureName.TryGetValue(name, out var feature))
			{
				return feature();
			}
			throw new Exception($"Feature {name} was not found.");
		}

		public List<string> GetKeyList()
		{
			return featureName.Keys.ToList();
		}
	}
}
