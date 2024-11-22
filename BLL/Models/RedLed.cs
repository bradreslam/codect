using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
	internal class RedLed : FeatureModel
	{
		public RedLed()
		{
			description = "An simple red led light will light up when provided with power";
			offSprite = Resource1.Led_Off;
			onSprite = Resource1.Red_Led_On;
		}
	}
}
