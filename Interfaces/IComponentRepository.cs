using System.ComponentModel;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using DTO;

namespace Interfaces;

public interface IComponentRepository
{
	List<ComponentDTO> GetAllComponents();
	ComponentDTO? GetComponentByName(string name);
	void InsertComponentInDatabase(string name, List<int> contactPoints, int feature);
	bool NameExistsInDatabase(string name);
}