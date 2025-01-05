
using Codect.Classes;

namespace BLL.Models
{
	public class WireDictionary
	{
		private readonly Dictionary<string, string> wireName = new()
		{
			{ "NWireOff", Resource1.N_Wire_Off },
			{ "EWireOff", Resource1.E_Wire_Off },
			{ "SWireOff", Resource1.S_Wire_Off },
			{ "WWireOff", Resource1.W_Wire_Off },
			{ "NWireOn", Resource1.N_Wire_On },
			{ "EWireOn", Resource1.E_Wire_On },
			{ "SWireOn", Resource1.S_Wire_On },
			{ "WWireOn", Resource1.W_Wire_On },
		};
		public string FindWireSprite(string key)
		{
			if (wireName.TryGetValue(key, out string path))
			{
				return path;
			}
			throw new KeyNotFoundException($"Sprite path for key '{key}' not found.");
		}

		public string GetWireSprite(ContactPoint direction, bool isOn)
		{
			string state = isOn ? "On" : "Off";
			string key = $"{direction}Wire{state}";
			return FindWireSprite(key);
		}
	}
}
