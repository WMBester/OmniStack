using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WMB.Api.IntegrationTests.Clients;

namespace WMB.Api.IntegrationTests.Setup
{
    public class TestFixtureSetup
    {
        public required IntegrationHttpClient client;
        private TestConfig _config;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            LoadConfig();
            client = new IntegrationHttpClient(_config.BaseUrl, TimeSpan.FromSeconds(_config.TimeoutSeconds));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }

        private void LoadConfig()
        {
            var configPath = Path.Combine(AppContext.BaseDirectory, "testsettings.json");
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"Test config file not found at {configPath}");
            }

            var json = File.ReadAllText(configPath);
            _config = JsonSerializer.Deserialize<TestConfig>(json) ?? throw new Exception(" Failed to deserialize test config");
        }


    }
    public class TestConfig
    {
        public string BaseUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
    }
}
