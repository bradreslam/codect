using BLL.Models;
using Interfaces;
using System.Data;
using Codect.Classes;
using DTO;
using Component = BLL.Models.Component;

namespace DAL
{
	public class ComponentRepository(IComponentRepository ComponentRepository) : IComponentRepository
	{
		private readonly CodectEfCoreDbContext _context;

		public List<ComponentDTO> GetAllComponents()
		{
			List<Component> components = _context.Components.ToList();

			List<ComponentDTO> ComponentDTOS = components
				.Select(component => new ComponentDTO
				{
					Name = component.Name,
					ContactPoints = component.ContactPoints
						.Select(contactPoint => new ContactPointDTO
						{
							Direction = contactPoint.ToString(),
							Id = (int)contactPoint
						})
						.ToList(),
					Feature = new FeatureTypeDTO
					{
						Feature = component.Feature.ToString(),
						Id = (int)component.Feature
					}
				})
				.ToList();

			return ComponentDTOS;
		}

		public ComponentDTO GetComponentByName(string name)
		{
			Component component = _context.Components.Find(name);

			List<ContactPointDTO> contactPointDtos = component.ContactPoints
				.Select(contactPoint => new ContactPointDTO
				{
					Direction = contactPoint.ToString(),
					Id = (int)contactPoint
				})
				.ToList();

			FeatureTypeDTO featureTypeDTO = new FeatureTypeDTO
			{
				Feature = component.Feature.ToString(),
				Id = (int)component.Feature
			};

			return new ComponentDTO
			{
				Name = component.Name,
				ContactPoints = contactPointDtos,
				Feature = featureTypeDTO
			};
		}

		public void InsertComponentInDatabase(string name, List<int> contactPoints, int feature)
		{
			List<ContactPoint> listContactPoints = new();
			foreach(var contactPoint in contactPoints)
			{
				listContactPoints.Add((ContactPoint)contactPoint);
			}
			FeatureType featureType = (FeatureType)feature;
			Component component = new(name, listContactPoints, featureType, ComponentRepository);
			// Add the new student to the DbSet
			_context.Components.Add(component);

			// Save changes to the database
			_context.SaveChanges();
		}

		public bool NameExistsInDatabase(string name)
		{
			Component component = _context.Components.Find(name);

			if (component == null)
			{
				return false;
			}

			return true;
		}
	}
}
