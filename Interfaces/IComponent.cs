using DTO;

namespace Interfaces;

public interface IComponent
{
	List<ComponentDTO> GetAllComponents();
	void InsertComponentInDatabase(List<string> contactPoints, string feature);
	bool IdExistsInDatabase(string id);
	ComponentDTO GetComponentBasedOnId(string id);
}