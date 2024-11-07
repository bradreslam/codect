using BLL.Models;
using Codect.Classes;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Svg;
using System.Collections.Generic;
using System.Linq;
using BLL.Classes;
using DAL;
using Interfaces;

namespace Codect.Controllers
{
	[Route("/api/components")]
	[ApiController]
	public class ComponentController(IComponentRepository ComponentRepository) : ControllerBase
	{
		[HttpGet]
		[Route("{Name}/image.svg")]
		
		public IActionResult GetComponentImage(string Id)
		{
			SpriteFactory sf = new();
			List<ContactPoint> contactPoints = new();
			contactPoints.Add(ContactPoint.N);
			contactPoints.Add(ContactPoint.E);
			ComponentSpriteDictionary cd = new();
			SvgDocument svgDocument = sf.CreateSprite(contactPoints, cd.FindComponentSprite("LedOff"), false);

			var returnSprite = svgDocument.GetXML();

			return Content(returnSprite, "image/svg+xml");
		}

		[HttpGet]
		[Route("featureList")]
		public List<string> GetFeatureList()
		{
			FeatureDictionary fd = new();
			return (fd.GetKeyList());
		}

		[HttpGet]
		[Route("ComponentInfo")]
		public Dictionary<string, string> GetComponentInfo(string Id)
		{
			ComponentManager cm = new(ComponentRepository);
			ComponentDTO componentDto = cm.GetComponentBasedOnId(Id);

			Dictionary<string, string> componentInfo = new()
			{
				{"endPoints",componentDto.ContactPoints.ToString()},
				{"feature",componentDto.Feature}
			};

			return componentInfo;
		}

		[HttpPost]
		[Route("createComponent")]
		public IActionResult Post([FromBody] ComponentDTO componenDTO)
		{
			if (componenDTO == null)
			{
				return BadRequest("Invalid data.");
			}

			ComponentManager cm = new(ComponentRepository);
			try
			{
				cm.InsertComponentInDatabase(componenDTO.ContactPoints, componenDTO.Feature);
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			} 

			return Ok("Component created successfully."); // Respond with success
		}
	}
}
