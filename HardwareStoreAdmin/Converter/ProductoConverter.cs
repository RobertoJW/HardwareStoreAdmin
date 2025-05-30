using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using HardwareStoreAdmin.Modelo;

public class ProductoConverter : JsonConverter<Producto>
{
    public override Producto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using (var jsonDoc = JsonDocument.ParseValue(ref reader))
        {
            var jsonObject = jsonDoc.RootElement;

            string categoria = jsonObject.GetProperty("categoria").GetString();

            Producto producto;

            switch (categoria)
            {
                case "Movil":
                    producto = JsonSerializer.Deserialize<Movil>(jsonObject.GetRawText(), options);
                    break;
                case "Portatil":
                    producto = JsonSerializer.Deserialize<Portatil>(jsonObject.GetRawText(), options);
                    break;
                case "Sobremesa":
                    producto = JsonSerializer.Deserialize<Sobremesa>(jsonObject.GetRawText(), options);
                    break;
                default:
                    producto = JsonSerializer.Deserialize<Producto>(jsonObject.GetRawText(), options);
                    break;
            }

            return producto;
        }
    }

    public override void Write(Utf8JsonWriter writer, Producto value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, (object)value, value.GetType(), options);
    }
}
