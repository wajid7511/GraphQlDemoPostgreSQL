using System.Text.Json.Serialization;

namespace GraphQlDemoPostgresQl.Common.Core.RabbitMq;

public class RabbitMqOptions
{
    public const string CONFIG_PATH = "RabbitMQ";

    [JsonPropertyName("HostName")]
    public string HostName { get; set; } = string.Empty;
    [JsonPropertyName("Port")]
    public int Port { get; set; }
    [JsonPropertyName("UserName")]
    public string UserName { get; set; } = string.Empty;
    [JsonPropertyName("Password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("QueueName")]
    public string QueueName { get; set; } = string.Empty;

    [JsonPropertyName("ExchangeName")]
    public string ExchangeName { get; set; } = string.Empty;
}
