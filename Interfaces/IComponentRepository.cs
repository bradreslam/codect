using System.ComponentModel;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace Interfaces;

public interface IComponentRepository
{
	DataTable GetAllComponents();
	DataRow GetComponentByName(string name);
	void InsertComponentInDatabase(string name, List<int> contactPoints, int feature);
}