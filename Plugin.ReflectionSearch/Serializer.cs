using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Plugin.ReflectionSearch
{
	/// <summary>
	/// JSON serialization helper based on Newtonsoft.Json.
	/// Provides simple serialization and deserialization methods
	/// equivalent to the legacy JavaScriptSerializer behavior.
	/// </summary>
	internal static class Serializer
	{
		private static readonly JsonSerializerSettings DefaultSettings = new JsonSerializerSettings
		{
			// Match legacy behavior as much as possible
			NullValueHandling = NullValueHandling.Ignore,
			MissingMemberHandling = MissingMemberHandling.Ignore,
			Formatting = Formatting.None
		};

		/// <summary>Deserialize a JSON string into a strongly typed object.</summary>
		/// <typeparam name="T">Target object type.</typeparam>
		/// <param name="json">JSON-formatted string.</param>
		/// <returns>Deserialized object instance.</returns>
		public static T JavaScriptDeserialize<T>(String json)
			=> String.IsNullOrEmpty(json)
				? default
				: JsonConvert.DeserializeObject<T>(json, DefaultSettings);

		/// <summary>Serialize an object to a JSON-formatted string.</summary>
		/// <param name="item">Object to serialize.</param>
		/// <returns>JSON-formatted string.</returns>
		public static String JavaScriptSerialize(Object item)
			=> item == null
				? null
				: JsonConvert.SerializeObject(item, DefaultSettings);
	}
}