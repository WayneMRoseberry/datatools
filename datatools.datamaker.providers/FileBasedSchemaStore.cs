using System.Text.Json;

namespace datatools.datamaker.providers
{
	public class FileBasedSchemaStore : ISchemaStore
	{
		private string _schemaFilePath;
		private SchemaCollection _schemaDict = new SchemaCollection();

		public FileBasedSchemaStore(string schemaFilePath) 
		{
			this._schemaFilePath = schemaFilePath;
			if(System.IO.File.Exists(_schemaFilePath))
			{
				var file = System.IO.File.ReadAllText(_schemaFilePath);
				_schemaDict = SchemaCollection.LoadFromJson(file);
			}
			else
			{
				WriteSchemaDictionaryToFile();
			}
		}

		private void WriteSchemaDictionaryToFile()
		{
			var schemaText = SchemaCollection.GetJsonFromSchemaCollection(this._schemaDict);
			System.IO.File.WriteAllText(_schemaFilePath, schemaText);
		}

		public void AddDataSchema(string nameSpace, DataSchema dataSchema)
		{
			if(!_schemaDict.Schemas.ContainsKey(nameSpace))
			{
				_schemaDict.Schemas.Add(nameSpace, new Dictionary<string, DataSchema>());
			}
			var namespaceDict = _schemaDict.Schemas[nameSpace];
			if(!namespaceDict.ContainsKey(dataSchema.SchemaName))
			{
				namespaceDict.Add(dataSchema.SchemaName, dataSchema);
			}
			else
			{
				namespaceDict[dataSchema.SchemaName] = dataSchema;
			}
			WriteSchemaDictionaryToFile();
		}

		public DataSchema GetSchemaElement(DataSchemaReference schemaReference)
		{
			return _schemaDict.Schemas[schemaReference.NameSpace][schemaReference.Name];
		}
	}
}
