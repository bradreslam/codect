using BLL.Models;
using System.ComponentModel.DataAnnotations;

namespace BLL.ValidationAttributes;

public class AllowedValuesAttribute : ValidationAttribute
{
	private readonly Type _dictionaryType;

	public AllowedValuesAttribute(Type dictionaryType)
	{
		_dictionaryType = dictionaryType;
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		// Check if the dictionary type implements FeatureDictionary
		if (!typeof(FeatureDictionary).IsAssignableFrom(_dictionaryType))
		{
			throw new InvalidOperationException($"{_dictionaryType.Name} must inherit from FeatureDictionary.");
		}

		// Get an instance of the dictionary
		var dictionary = (FeatureDictionary)Activator.CreateInstance(_dictionaryType);
		var keys = dictionary.GetKeyList();

		// Validate the value
		if (value is string featureName && keys.Contains(featureName))
		{
			return ValidationResult.Success;
		}

		return new ValidationResult(ErrorMessage ?? $"Value '{value}' is not allowed.");
	}
}