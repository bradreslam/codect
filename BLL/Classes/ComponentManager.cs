using Interfaces;
using DTO;

namespace BLL.Classes
{
	public class ComponentManager(IComponentRepository ComponentRepository) : Interfaces.IComponent
	{
		private readonly IComponentRepository _componentRepository = ComponentRepository;
		public List<string> GetAllComponentIds()
		{
			return ComponentRepository.GetAllComponentIds();
		}

		public string InsertComponentInDatabase( List<string> contactPoints, string feature)
		{
			return ComponentRepository.InsertComponentInDatabase(contactPoints, feature);
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
