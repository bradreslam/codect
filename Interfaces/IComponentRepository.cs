using DTO;

namespace Interfaces;

public interface IComponentRepository
{
	List<string> GetAllComponentIds();
	string InsertComponentInDatabase(List<string> contactPoints, string feature);
	bool IdExistsInDatabase(string id);
	ComponentDTO GetComponentBasedOnId(string id);
}