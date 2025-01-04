﻿using BLL.Classes;
using BLL.Models;
using Codect.Classes;
using DAL;
using DTO;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Svg;

namespace Codect.Controllers
{
	public class ComponentListHub : Hub
	{
		private readonly IComponentRepository _componentRepository;

		public ComponentListHub(IComponentRepository componentRepository)
		{
			_componentRepository = componentRepository;
		}

		public async Task SendIdList()
		{
			ComponentManager cm = new(_componentRepository);
			List<string> idList = cm.GetAllComponentIds();
			await Clients.All.SendAsync("ReceiveIdList", idList);
		}

		public async Task SendComponentImage(string id)
		{
			try
			{
				ComponentManager cm = new(_componentRepository);
				ComponentDTO componentDto = cm.GetComponentBasedOnId(id);
				List<ContactPoint> contactPoints = new();
				ContactPointDictionary cpd = new();

				foreach (string contactPoint in componentDto.ContactPoints)
				{
					contactPoints.Add(cpd.GetContactPoint(contactPoint));
				}

				SpriteFactory sf = new();
				SvgDocument svgDocument = sf.CreateSprite(contactPoints, componentDto.Feature, false);

				string returnSprite = svgDocument.GetXML();
				await Clients.All.SendAsync("ReceiveComponentImage", id, returnSprite);
			}
			catch (Exception ex)
			{
				await Clients.All.SendAsync("ReceiveError", ex.Message);
			}
		}

		public async Task SendComponentInfo(string id)
		{
			ComponentManager cm = new(_componentRepository);
			ComponentDTO componentDto = cm.GetComponentBasedOnId(id);

			Dictionary<string, string> componentInfo;

			if (componentDto.Feature != "")
			{
				FeatureDictionary fd = new();
				FeatureModel component = fd.GetFeatureModel(componentDto.Feature);
				componentInfo = new()
				{
					{ "endPoints", string.Join(",", componentDto.ContactPoints) },
					{ "description", component.description },
					{ "feature", componentDto.Feature }
				};
			}
			else
			{
				componentInfo = new()
				{
					{ "endPoints", string.Join(",", componentDto.ContactPoints) },
					{ "description", null },
					{ "feature", "None" }
				};
			}

			await Clients.All.SendAsync("ReceiveComponentInfo", id, componentInfo);
		}
	}
}
