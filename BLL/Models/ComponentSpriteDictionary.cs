using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class ComponentSpriteDictionary
    {
	    private readonly Dictionary<string, string> spritePaths = new()
	    {
		    { "LedOff", Resource1.Led_Off },
		    { "RedLedOn", Resource1.Red_Led_On },
		    { "YellowLedOn", Resource1.Yellow_Led_On },
		    { "WhiteLedOn", Resource1.White_Led_On },
		    { "GreenLedOn", Resource1.Green_Led_On },
		    { "BlueLedOn", Resource1.Blue_Led_On }
	    };
	    public string FindComponentSprite(string name)
	    {
		    if (spritePaths.TryGetValue(name, out string sprite))
		    {
			    return sprite;
		    }
		    throw new KeyNotFoundException($"Sprite for name '{name}' not found.");
	    }
	}
}
