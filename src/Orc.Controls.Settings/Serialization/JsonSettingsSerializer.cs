namespace Orc.Controls.Settings;

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

public class JsonSettingsSerializer<TSettings> : ISettingsSerializer<TSettings>
    where TSettings : class
{
    private readonly JsonSerializerOptions _serializationOptions;

    public JsonSettingsSerializer()
    {
        _serializationOptions = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,    
            // Handle circular references
            ReferenceHandler = ReferenceHandler.Preserve,
            // Increase max depth if needed
            MaxDepth = 128
        };

        // Add the custom Type converter
        _serializationOptions.Converters.Add(new TypeConverter());
        _serializationOptions.Converters.Add(new ColorConverter());
    }

    public async Task<TSettings?> DeserializeAsync(string data)
    {
        try
        {
            return await Task.FromResult(JsonSerializer.Deserialize<TSettings>(data, _serializationOptions));
        }
        catch
        {
            return null;
        }
    }

    public async Task<string> SerializeAsync(TSettings settings)
    {
        return await Task.FromResult(JsonSerializer.Serialize(settings, _serializationOptions));
    }
}
