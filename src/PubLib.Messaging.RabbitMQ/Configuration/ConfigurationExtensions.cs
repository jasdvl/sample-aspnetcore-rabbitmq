using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Text;
using System.Text.Json;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PubLib.Messaging.RabbitMQ.Configuration;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddRabbitMQConfig(this IConfigurationBuilder builder)
    {
        return builder.AddYamlResource("PubLib.Messaging.RabbitMQ.rabbitmq-config.yml");
    }

    private static IConfigurationBuilder AddYamlResource(this IConfigurationBuilder builder, string resourceName)
    {
        var yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        var assembly = Assembly.GetExecutingAssembly();

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            throw new FileNotFoundException($"Resource {resourceName} not found.");
        }

        using var reader = new StreamReader(stream);
        var yamlContent = reader.ReadToEnd();

        var yamlData = yamlDeserializer.Deserialize<Dictionary<string, object>>(yamlContent);

        var jsonString = JsonSerializer.Serialize(yamlData);
        var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

        return builder.AddJsonStream(jsonStream);
    }
}
