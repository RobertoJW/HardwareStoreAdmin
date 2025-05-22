using HardwareStoreAdmin.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace HardwareStoreAdmin.Converter
{
    public class ProductoConverter : JsonConverter<Producto>
    {
        public override Producto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var jsonDoc = JsonDocument.ParseValue(ref reader);
            var root = jsonDoc.RootElement;
            var categoria = root.GetProperty("categoria").GetString()?.ToLower();

            return categoria switch
            {
                "movil" => JsonSerializer.Deserialize<Movil>(root.GetRawText(), options),
                "portatil" => JsonSerializer.Deserialize<Portatil>(root.GetRawText(), options),
                "sobremesa" => JsonSerializer.Deserialize<Sobremesa>(root.GetRawText(), options),
                _ => throw new JsonException($"Tipo desconocido: {categoria}")
            };
        }

        public override void Write(Utf8JsonWriter writer, Producto value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
        }
    }

}
