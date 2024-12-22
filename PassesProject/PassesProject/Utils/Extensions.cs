namespace PassesProject.Utils;

public static class Extensions
{
    public static IConfigurationBuilder AddJsonConfig(this IConfigurationBuilder configuration, string environmentName)
    {
        return configuration
            .AddJsonFile("appsettings.json", true, false)
            .AddJsonFile($"appsettings.{environmentName}.json", true, false);
    }
}