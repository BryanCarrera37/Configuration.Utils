using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace Configuration.Utils.Tests.Extensions
{
    public class ConfigurationValidationExtensionTests
    {
        private static readonly string _sectionName = "Service";

        private class Settings
        {
            [Required(ErrorMessage = "Host is required")]
            public string Host { get; set; } = string.Empty;

            [Range(1, 65535, ErrorMessage = "Port must be between 1 and 65535")]
            public int Port { get; set; }
        }

        [Fact]
        public void ValidateConfiguration_ShouldNotThrow_WhenConfigIsValid()
        {
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(ConfigurationValidationExtensionTests.CreateInMemorySettings("localhost", 8080))
                .Build();

            config.ValidateConfiguration<Settings>(_sectionName);
        }

        [Fact]
        public void ValidateConfiguration_ShouldThrowInvalidConfigurationException_WhenConfigIsInvalid()
        {

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(ConfigurationValidationExtensionTests.CreateInMemorySettings("", 70000))
                .Build();

            var ex = Assert.Throws<InvalidConfigurationException>(() =>
                config.ValidateConfiguration<Settings>(_sectionName));

            Assert.Equal(_sectionName, ex.Section);
            Assert.NotEmpty(ex.ValidationResults);
            Assert.Contains(ex.ValidationResults, r => r?.ErrorMessage?.Contains("host is required", StringComparison.OrdinalIgnoreCase) ?? false);
            Assert.Contains(ex.ValidationResults, r => r?.ErrorMessage?.Contains("port must be between 1 and", StringComparison.OrdinalIgnoreCase) ?? false);
        }

        [Fact]
        public void ValidateConfiguration_ShouldThrowInvalidConfigurationException_WhenSectionIsMissing()
        {
            var config = new ConfigurationBuilder().Build();
            var ex = Assert.Throws<InvalidConfigurationException>(() =>
                config.ValidateConfiguration<Settings>("MissingSection"));

            Assert.Equal("MissingSection", ex.Section);
            Assert.Contains(ex.ValidationResults, r => r?.ErrorMessage?.Contains("host is required", StringComparison.OrdinalIgnoreCase) ?? false);
            Assert.Contains(ex.ValidationResults, r => r?.ErrorMessage?.Contains("port must be between 1 and", StringComparison.OrdinalIgnoreCase) ?? false);
        }

        private static Dictionary<string, string?> CreateInMemorySettings(string host, int port)
        {
            return new Dictionary<string, string?>
            {
                {$"{_sectionName}:Host", host},
                {$"{_sectionName}:Port", port.ToString()}
            };
        }
    }
}
