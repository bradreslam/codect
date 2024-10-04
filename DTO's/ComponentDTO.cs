using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
	public class ComponentDTO
	{
		public string Name { get; set; }
		public List<ContactPointDTO> ContactPoints { get; set; }
		public FeatureTypeDTO Feature { get; set; }
	}
}
