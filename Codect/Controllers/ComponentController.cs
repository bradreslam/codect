using BLL.Models;
using Codect.Classes;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Svg;
using BLL.Classes;
using Interfaces;

namespace Codect.Controllers
{
	[ApiController]
	public class ComponentController(IComponentRepository ComponentRepository) : ControllerBase
	{
		[HttpGet]
		[Route("components/{id}/image")]
		
		public IActionResult GetComponentImage(string id)
		{
			ComponentManager cm = new(ComponentRepository);
			ComponentDTO componentDto = cm.GetComponentBasedOnId(id);
			List<ContactPoint> contactpoints = new();
			ContactPointDictionary cpd = new();

			foreach (string contactpoint in componentDto.ContactPoints)
			{
				contactpoints.Add(cpd.GetContactPoint(contactpoint));
			}

			SpriteFactory sf = new();
			SvgDocument svgDocument = sf.CreateSprite(contactpoints, componentDto.Feature, false);

			var returnSprite = svgDocument.GetXML();

			return Content(returnSprite, "image/svg+xml");
		}

		[HttpGet]
		[Route("features")]
		public List<string> GetFeatureList()
		{
			FeatureDictionary fd = new();
			return (fd.GetKeyList());
		}

		[HttpGet]
		[Route("components/{id}")]
		public Dictionary<string, string> GetComponentInfo(string id)
		{
			ComponentManager cm = new(ComponentRepository);
			ComponentDTO componentDto = cm.GetComponentBasedOnId(id);

			if (componentDto.Feature != "")
			{
				FeatureDictionary fd = new();
				FeatureModel component = fd.GetFeatureModel(componentDto.Feature);
				Dictionary<string, string> componentInfo = new()
				{
					{"endPoints",string.Join( ",", componentDto.ContactPoints)},
					{"description", component.description},
					{"feature",componentDto.Feature}
				};
				return componentInfo;
			}
			else
			{
				Dictionary<string, string> componentInfo = new()
				{
					{"endPoints",string.Join( ",", componentDto.ContactPoints)},
					{"description", null},
					{"feature", "None"}
				};
				return componentInfo;
			}
		}

		[HttpPost]
		[Route("components/new-component")]
		public IActionResult Post([FromBody] ComponentDTO componenDTO)
		{
			if (componenDTO == null)
			{
				return BadRequest("Invalid data.");
			}

			ComponentManager cm = new(ComponentRepository);
			string id;
			try
			{
				id = cm.InsertComponentInDatabase(componenDTO.ContactPoints, componenDTO.Feature);
			}
			catch (Exception ex)
			{
				return BadRequest(new
				{
					message = ex.Message
				});
			} 

			return Ok(id); // Respond with success
		}

		[HttpGet]
		[Route("components/ids")]
		public List<string> GetAllComponentIds()
		{
			ComponentManager cm = new(ComponentRepository);

			return cm.GetAllComponentIds();
		}
	}
}
