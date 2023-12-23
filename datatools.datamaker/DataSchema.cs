namespace datatools.datamaker
{
	public class DataSchema
	{
		public string Name { get; set; }
		private List<SchemaElement> elements = new List<SchemaElement>();
		internal IList<SchemaElement> Elements { get { return elements; } }

		public void AddElement(SchemaElement element) 
		{
			this.elements.Add(element);
		}
	}

	public class DataSchemaReference
	{
		public string NameSpace { get; set; }
		public string Name { get; set; }
	}
}
