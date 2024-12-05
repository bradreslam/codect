using Codect.Classes;

namespace BLL.Models
{
	public class ContactPointDictionary
	{
		private readonly Dictionary<string, ContactPoint> contactPointKey = new()
		{
			{"N", ContactPoint.N},
			{"E", ContactPoint.E},
			{"S", ContactPoint.S},
			{"W", ContactPoint.W}
		};

		public ContactPoint GetContactPoint(string point)
		{
			if (contactPointKey.TryGetValue(point, out var contactPoint))
			{
				return contactPoint;
			}
			throw new Exception($"Contact point {point} was not found.");
		}
	}
}
