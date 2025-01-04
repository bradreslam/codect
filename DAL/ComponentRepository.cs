﻿using System.Data;
using BLL.Models;
using Interfaces;
using Codect.Classes;
using DTO;
using Microsoft.EntityFrameworkCore;
using Component = BLL.Models.Component;

namespace DAL
{
	public class ComponentRepository: IComponentRepository
	{
		private readonly CodectEfCoreDbContext _context;

		public ComponentRepository(CodectEfCoreDbContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public List<string> GetAllComponentIds()
		{
			return _context.Components
				.Select(component => component.Id)  // Select only the ID property
				.ToList();
		}

		public string InsertComponentInDatabase(List<string> contactPoints, string feature)
		{
			List<ContactPoint> listContactPoints = contactPoints
				.Where(s => Enum.IsDefined(typeof(ContactPoint), s))
				.Select(s => (ContactPoint)Enum.Parse(typeof(ContactPoint), s))
				.ToList();
			Component component = new(listContactPoints, feature);

			if (IdExistsInDatabase(component.Id))
			{
				throw new DataException("Component already exists in the database");
			}
			
			_context.Components.Add(component);

			// Save changes to the database
			_context.SaveChanges();
			return component.Id;
		}

		public bool IdExistsInDatabase(string id)
		{
			Component component = _context.Components.Find(id);

			if (component == null)
			{
				return false;
			}

			return true;
		}

		public ComponentDTO GetComponentBasedOnId(string id)
		{
			if (IdExistsInDatabase(id))
			{
				Component component = _context.Components.Find(id);
				ComponentDTO componentDto = new()
				{
					Feature = component.Feature,
					ContactPoints = component.ContactPoints
						.Select(contactPoint => new string(contactPoint.ToString()))
						.ToList(),
				};

				return componentDto;
			}
			else
			{
				throw new FileNotFoundException("Component does not exist in database");
			}
		}
	}
}
