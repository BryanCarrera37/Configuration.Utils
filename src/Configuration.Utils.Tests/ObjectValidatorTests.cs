using System.ComponentModel.DataAnnotations;

namespace Configuration.Utils.Tests
{
    public class ObjectValidatorTests
    {
        private class SampleModel
        {
            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; } = string.Empty;

            [Range(1, 100)]
            public int Age { get; set; }
        }

        [Fact]
        public void Validate_ShouldThrowArgumentNullException_WhenObjectIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ObjectValidator.Validate<object>(null!));

            Assert.Equal("obj", exception.ParamName);
            Assert.Contains("Object cannot be null", exception.Message);
        }

        [Fact]
        public void Validate_ShouldReturnEmptyList_WhenObjectIsValid()
        {
            var validModel = new SampleModel
            {
                Name = "John",
                Age = 30
            };

            var results = ObjectValidator.Validate(validModel);
            Assert.Empty(results);
        }

        [Fact]
        public void Validate_ShouldReturnValidationErrors_WhenObjectIsInvalid()
        {
            var invalidModel = new SampleModel
            {
                Age = 150
            };

            var results = ObjectValidator.Validate(invalidModel);
            Assert.Equal(2, results.Count);
            Assert.Contains(results, r => r?.ErrorMessage?.Contains("Name is required", StringComparison.OrdinalIgnoreCase) ?? false);
            Assert.Contains(results, r => r?.ErrorMessage?.Contains("between", StringComparison.OrdinalIgnoreCase) ?? false);
        }

        [Fact]
        public void GetErrors_ShouldReturnFormattedString()
        {
            var results = new List<ValidationResult>
            {
                new("Name is required"),
                new("Age must be between 1 and 100")
            };

            var errorMessage = ObjectValidator.GetErrors(results);
            Assert.Equal("- Name is required\n- Age must be between 1 and 100", errorMessage);
        }

        [Fact]
        public void GetErrors_ShouldReturnEmptyString_WhenNoErrors()
        {
            var results = new List<ValidationResult>();
            var errorMessage = ObjectValidator.GetErrors(results);
            Assert.Equal(string.Empty, errorMessage);
        }
    }
}