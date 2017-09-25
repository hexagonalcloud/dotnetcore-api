using Microsoft.Extensions.Configuration;

namespace SqlAdventure
{
    public class ConfigurationOptions
    {
        public ConfigurationOptions(IConfiguration configuration)
        {
            SqlConnectionString = configuration.GetConnectionString("SqlAdventure");
        }

        public string SqlConnectionString { get; }
    }
}
