using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace BLL.ValidationAttributes
{
	public class NoDuplicatesAttribute : ValidationAttribute
	{
		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value is IEnumerable list)
			{
				var hashSet = new HashSet<object>();
				foreach (var item in list)
				{
					if (!hashSet.Add(item))
					{
						return new ValidationResult("The list contains duplicate values.");
					}
				}
			}

			return ValidationResult.Success;
		}
	}
}
