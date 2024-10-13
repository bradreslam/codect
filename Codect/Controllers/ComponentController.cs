using BLL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Codect.Controllers
{
	[Route("/api/ComponentController")]
	[ApiController]
	public class ComponentController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get()
		{
			var components = new List<string>
			{
				"Component1",
				"Component2",
				"Component3",
				"Component4"
			};
			return Ok(components);
		}

		[HttpPost]
		public Component Post()
		{
			return null;
		}
	}
}
