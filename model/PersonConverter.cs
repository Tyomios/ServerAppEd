using System.Text.Json;
using System.Text.Json.Serialization;

namespace HelloApp.model;

//public class PersonConverter : JsonConverter<Person>
//{
//	/// <summary>
//	/// Десериализация JSON в объект <see cref="Person"/>.
//	/// </summary>
//	/// <param name="reader"></param>
//	/// <param name="typeToConvert"></param>
//	/// <param name="options"></param>
//	/// <returns> Десериализованный объект <see cref="Person"/>.</returns>
//    public override Person Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//	{
//		var personName = "Undefined";
//		var personAge = 0;

//		while (reader.Read())
//		{
//			if (reader.TokenType.Equals(JsonTokenType.PropertyName))
//			{
//				var propertyName = reader.GetString();
//				reader.Read();

//				switch (propertyName?.ToLower())
//				{
//					case "age" when reader.TokenType is JsonTokenType.Number:
//						personAge = reader.GetInt32();
//						break;

//					case "age" when reader.TokenType is JsonTokenType.String:
//						string? stringValue = reader.GetString();
						
//						if (int.TryParse(stringValue, out int value))
//						{
//							personAge = value;
//						}
//						break;

//					case "name":
//						string? name = reader.GetString();
//						if (name != null) personName = name;
//						break;
//				}
//			}
//		}
//		return new Person(personName, personAge);
//	}

//    /// <summary>
//    /// Сериализация объекта <see cref="Person"/>.
//    /// </summary>
//    /// <param name="writer"> Испольнитель. </param>
//    /// <param name="person"> Сериализуемый объект. </param>
//    /// <param name="options"> Опции сериализации. </param>
//    public override void Write(Utf8JsonWriter writer, Person person, JsonSerializerOptions options)
//	{
//        using (writer)
//        {
//            writer.WriteString("name", person.Name);
//            writer.WriteNumber("age", person.Age);
//        }
//  //      writer.WriteStartObject();
		

//		//writer.WriteEndObject();
//	}
//}