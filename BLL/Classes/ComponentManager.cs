using Interfaces;
using BLL.Exceptions;
using BLL.Models;
using Codect.Classes;
using DTO;

namespace BLL.Classes
{
	public class ComponentManager(IComponentRepository ComponentRepository) : IComponent
	{
		private readonly IComponentRepository _componentRepository = ComponentRepository;
		public List<ComponentDTO> GetAllComponents()
		{
			return ComponentRepository.GetAllComponents();
		}

		public ComponentDTO GetComponentByName(string name)
		{
			return ComponentRepository.GetComponentByName(name);
		}

		public void InsertComponentInDatabase(string name, List<int> contactPoints, int feature)
		{
			try
			{
				List<ContactPoint> contactPointsList = new();
				foreach (ContactPoint contactPoint in contactPoints)
				{
					contactPointsList.Add((ContactPoint)contactPoint);
				}

				Component component = new(name, contactPointsList, (FeatureType)feature);
			}
			catch(ComponentExceptions ex)
			{

			}
			ComponentRepository.InsertComponentInDatabase(name, contactPoints, feature);
		}

		public bool NameExistsInDatabase(string name)
		{
			return ComponentRepository.NameExistsInDatabase(name);
		}
	}
}
