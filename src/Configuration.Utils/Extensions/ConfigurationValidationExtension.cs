using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace Configuration.Utils
{
    public static class ConfigurationValidationExtension
    {
        public static void ValidateConfiguration<T>(this IConfiguration configuration, string sectionName) where T : class, new()
        {
            var settings = new T();
            configuration.GetSection(sectionName).Bind(settings);

            try
            {
                var results = ObjectValidator.Validate(settings);
                if (results.Count > 0)
                {
                    throw new InvalidConfigurationException(sectionName, results);
                }
            }
            catch (ArgumentNullException)
            {
                throw new InvalidConfigurationException(sectionName, [new ValidationResult($"Null value received for section '{sectionName}'")]);
            }
        }
    }
}