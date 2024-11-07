using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
	internal class RedLed : FeatureModel
	{
		public string description = "An simple red led light will light up when provided with power";
		public string offSprite = Resource1.Led_Off;
		public string onSprite = Resource1.Red_Led_On;
	}
}
