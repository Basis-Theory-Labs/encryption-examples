using System.Text.Json;
using System.Text.Json.Serialization;
using Encryption.Structs;

namespace Encryption.Converters;

public class NullableEncryptionAlgorithmJsonConverter : JsonConverter<EncryptionAlgorithm?>
{
    public override EncryptionAlgorithm? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        if (string.IsNullOrEmpty(value)) return null;

        return new EncryptionAlgorithm(value);
;
    }

    public override void Write(Utf8JsonWriter writer, EncryptionAlgorithm? value, JsonSerializerOptions options)
    {
        if (value == null)
            return;

        writer.WriteStringValue(value?.ToString());
    }
}
