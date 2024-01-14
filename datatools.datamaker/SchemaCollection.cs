namespace datatools.datamaker
{
	public class SchemaCollection
	{
		private Dictionary<string, Dictionary<string, DataSchema>> _schemas = new Dictionary<string, Dictionary<string, DataSchema>>();

		public Dictionary<string, Dictionary<string, DataSchema>> Schemas { get { return this._schemas; } set { this._schemas = value; } }
	}
}
