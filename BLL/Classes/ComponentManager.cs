using Interfaces;
using System.Data;

namespace BLL.Classes
{
	public class ComponentManager(IComponentRepository ComponentRepository) : IComponent
	{
		private readonly IComponentRepository _componentRepository = ComponentRepository;
		public DataTable GetAllComponents()
		{
			return ComponentRepository.GetAllComponents();
		}

		public DataRow GetComponentByName(string name)
		{
			return ComponentRepository.GetComponentByName(name);
		}

		public void InsertComponentInDatabase(string name, List<int> contactPoints, int feature)
		{
			ComponentRepository.InsertComponentInDatabase(name, contactPoints, feature);
		}
	}
}
