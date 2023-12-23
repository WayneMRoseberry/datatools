namespace datatools.datamaker
{
	public interface ISchemaStore
	{
		void AddDataSchema(string nameSpace, DataSchema dataSchema);
		DataSchema GetSchemaElement(DataSchemaReference schemaReference);
	}
}
