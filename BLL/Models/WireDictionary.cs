
using Codect.Classes;

namespace BLL.Models
{
	public class WireDictionary
	{
		private readonly Dictionary<string, string> spritePaths = new()
		{
			{ "NWireOff", "../Codect_Sprites/N_Wire_Off.png" },
			{ "EWireOff", "../Codect_Sprites/E_Wire_Off.png" },
			{ "SWireOff", "../Codect_Sprites/S_Wire_Off.png" },
			{ "WWireOff", "../Codect_Sprites/W_Wire_Off.png" },
			{ "NWireOn", "../Codect_Sprites/N_Wire_On.png" },
			{ "EWireOn", "../Codect_Sprites/E_Wire_On.png" },
			{ "SWireOn", "../Codect_Sprites/S_Wire_On.png" },
			{ "WWireOn", "../Codect_Sprites/W_Wire_On.png" },
		};
		public string SendPath(string key)
		{
			if (spritePaths.TryGetValue(key, out string path))
			{
				return path;
			}
			throw new KeyNotFoundException($"Sprite path for key '{key}' not found.");
		}

		public string GetPath(ContactPoint direction, bool isOn)
		{
			string state = isOn ? "On" : "Off";
			string key = $"{direction}Wire{state}";
			return SendPath(key);
		}
	}
}
