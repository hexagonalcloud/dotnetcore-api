using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace SqlAdventure.Database
{
    public partial class SqlAdventureContext : ISqlAdventureContext
    {
        private readonly ConfigurationOptions _configurationOptions;

        // TODO: usee the .net core options instead
        public SqlAdventureContext(ConfigurationOptions configurationOptions)
        {
            _configurationOptions = configurationOptions;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configurationOptions.SqlConnectionString);
            }
        }
    }
}
