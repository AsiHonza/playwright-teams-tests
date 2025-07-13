using System.IO;
using System.Text.Json;

namespace TeamsTestProject.Helpers
{
    public class Config
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public static Config Load(string path = "config.json")
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Config>(json) ?? new Config();
        }
    }
}
