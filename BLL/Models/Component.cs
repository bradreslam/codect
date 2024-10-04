using System.ComponentModel.DataAnnotations;
using BLL.Exceptions;
using Codect.Classes;
using Interfaces;

namespace BLL.Models
{
	public class Component
	{
		[Key]
		public string Name { get; set; }
		public List<ContactPoint> ContactPoints { get; set; }
		public FeatureType Feature { get; set; }

		public Component(string name, List<ContactPoint> contactPoints, FeatureType feature, IComponentRepository ComponentRepository)
		{
			IComponentRepository _componentRepository = ComponentRepository;

			_ = name.Length switch
			{
				> 30 => throw new ComponentExceptions($"The name {name} is longer than 30 characters."),
				< 3 => throw new ComponentExceptions($"The name {name} is shorter than 3 characters."),
				_ => Name = name,
			};

			if (_componentRepository.NameExistsInDatabase(name))
			{
				throw new ComponentExceptions($"The name {name} already exists in the database");
			}
			if (contactPoints.Count != contactPoints.Distinct().Count())
			{
				throw new ComponentExceptions("The contact points can not contain the same value more than once.");
			}
			if (contactPoints.Count < 2)
			{
				throw new ComponentExceptions("There has to be at least 2 contact points on a valid component");
			}
			{
				ContactPoints = contactPoints;
			}

			Feature = feature;
		}
	}
}
