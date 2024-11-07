using Interfaces;
using BLL.Exceptions;
using BLL.Models;
using Codect.Classes;
using DTO;
using System.ComponentModel;

namespace BLL.Classes
{
	public class ComponentManager(IComponentRepository ComponentRepository) : Interfaces.IComponent
	{
		private readonly IComponentRepository _componentRepository = ComponentRepository;
		public List<ComponentDTO> GetAllComponents()
		{
			return ComponentRepository.GetAllComponents();
		}

		public void InsertComponentInDatabase( List<string> contactPoints, string feature)
		{
			ComponentRepository.InsertComponentInDatabase(contactPoints, feature);
		}

		public bool IdExistsInDatabase(string id)
		{
			return ComponentRepository.IdExistsInDatabase(id);
		}

		public ComponentDTO GetComponentBasedOnId(string id)
		{
			return ComponentRepository.GetComponentBasedOnId(id);
		}
	}
}
