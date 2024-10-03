using System.ComponentModel;
using System.Data;

namespace Interfaces;

public interface IComponent
{
	DataTable GetAllComponents();
	DataRow GetComponentByName(string name);
	void InsertComponentInDatabase(string name, List<int> contactPoints, int feature);
}