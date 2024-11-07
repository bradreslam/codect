using System.ComponentModel.DataAnnotations;
using BLL.Exceptions;
using Codect.Classes;
using Interfaces;

namespace BLL.Models
{
	public class Component
	{
		[Key]
		public string Id { get; set; }
		public List<ContactPoint> ContactPoints { get; set; }
		public string Feature { get; set; }

		public Component(List<ContactPoint> contactPoints, string feature)
		{
			if (contactPoints.Count != contactPoints.Distinct().Count())
			{
				throw new ComponentExceptions("The contact points can not contain the same value more than once.");
			}
			if (contactPoints.Count < 2)
			{
				throw new ComponentExceptions("There has to be at least 2 contact points on a valid component");
			}
			{
				ContactPoints = contactPoints;
			}

			Feature = feature;

			string GetContactString(ContactPoint point) => contactPoints.Contains(point) ? "1" : "0";

			Id = GetContactString(ContactPoint.N)
			        + GetContactString(ContactPoint.E)
			        + GetContactString(ContactPoint.S)
			        + GetContactString(ContactPoint.W)
			            + feature;
			
		}
	}
}
