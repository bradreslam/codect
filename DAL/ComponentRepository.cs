using BLL.Models;
using Interfaces;
using System.ComponentModel;
using System.Data;
using Codect.Classes;
using Component = BLL.Models.Component;

namespace DAL
{
	public class ComponentRepository : IComponentRepository
	{
		private readonly CodectEfCoreDbContext _context;
		public List<Component> GetAllComponents()
		{
			return _context.Components.ToList();
		}

		public Component GetComponentByName(string name)
		{
			return _context.Components.Find(name);
		}

		public void InsertComponentInDatabase(string name, List<int> contactPoints, int feature)
		{
			List<ContactPoint> listContactPoints = new();
			foreach(var contactPoint in contactPoints)
			{
				listContactPoints.Add((ContactPoint)contactPoint);
			}
			FeatureType featureType = (FeatureType)feature;
			Component component = new(name, listContactPoints, featureType);
			// Add the new student to the DbSet
			_context.Components.Add(component);

			// Save changes to the database
			_context.SaveChanges();
		}

		DataTable IComponentRepository.GetAllComponents()
		{
			throw new NotImplementedException();
		}

		DataRow IComponentRepository.GetComponentByName(string name)
		{
			throw new NotImplementedException();
		}
	}
}
