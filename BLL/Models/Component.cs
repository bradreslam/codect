using System.ComponentModel.DataAnnotations;
using BLL.ValidationAttributes;
using Codect.Classes;
using AllowedValuesAttribute = BLL.ValidationAttributes.AllowedValuesAttribute;

namespace BLL.Models
{
	public class Component
	{
		[Key]
		public string Id { get; set; }
		[Required]
		[MinLength(2, ErrorMessage = "There can be no less than 2 contact points")]
		[NoDuplicates]
		public List<ContactPoint> ContactPoints { get; set; }
		[AllowedValues(typeof(FeatureDictionary), ErrorMessage = "Feature has to exist in dictionary")]
		public string Feature { get; set; }

		public Component(List<ContactPoint> contactPoints, string feature)
		{
			ContactPoints = contactPoints;

			Feature = feature;

			Validate();

			string GetContactString(ContactPoint point) => contactPoints.Contains(point) ? "1" : "0";

			Id = GetContactString(ContactPoint.N)
			        + GetContactString(ContactPoint.E)
			        + GetContactString(ContactPoint.S)
			        + GetContactString(ContactPoint.W)
			            + feature;
			
		}

		private void Validate()
		{
			var context = new ValidationContext(this);
			Validator.ValidateObject(this, context, validateAllProperties: true);
		}
	}
}
