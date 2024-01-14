namespace datatools.datamaker
{
	public class SchemaCollection
	{
		private Dictionary<string, Dictionary<string, DataSchema>> _schemas = new Dictionary<string, Dictionary<string, DataSchema>>();

		public Dictionary<string, Dictionary<string, DataSchema>> Schemas { get { return this._schemas; } set { this._schemas = value; } }

		public static SchemaCollection LoadFromJson(string json)
		{
			SchemaCollection collection = System.Text.Json.JsonSerializer.Deserialize<SchemaCollection>(json);
			return collection;
		}

		public static string GetJsonFromSchemaCollection(SchemaCollection collection)
		{
			return System.Text.Json.JsonSerializer.Serialize(collection);
		}
	}
}
