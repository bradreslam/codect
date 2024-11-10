using DTO;

namespace Interfaces;

public interface IComponentRepository
{
	List<string> GetAllComponentIds();
	void InsertComponentInDatabase(List<string> contactPoints, string feature);
	bool IdExistsInDatabase(string id);
	ComponentDTO GetComponentBasedOnId(string id);
}