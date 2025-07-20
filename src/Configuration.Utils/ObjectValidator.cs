using System.ComponentModel.DataAnnotations;

namespace Configuration.Utils
{
    public static class ObjectValidator
    {
        public static List<ValidationResult> Validate<T>(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj), "Object cannot be null");

            var context = new ValidationContext(obj);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(obj, context, results, true);
            return results;
        }

        public static string GetErrors(List<ValidationResult> results)
        {
            return string.Join("\n", results.Select(r => $"- {r.ErrorMessage}"));
        }
    }
}