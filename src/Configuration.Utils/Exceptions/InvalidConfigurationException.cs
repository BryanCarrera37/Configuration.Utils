using System.ComponentModel.DataAnnotations;

namespace Configuration.Utils
{
    public class InvalidConfigurationException(string sectionName, List<ValidationResult> results) : Exception($"\nInvalid Configuration for '{sectionName}':\n{ObjectValidator.GetErrors(results)}")
    {
        public string Section { get; } = sectionName;
        public List<ValidationResult> ValidationResults { get; } = results;
    }
}