namespace datatools.datamaker.providers.tests
{
	[TestClass]
	public class FileBasedSchemaStoreTests
	{
		[TestMethod]
		public void Instantiate_fileiscreated()
		{
			string testfilepath = "testfile.txt";
			try
			{
				if(System.IO.File.Exists(testfilepath))
				{
					System.IO.File.Delete(testfilepath);
				}
				FileBasedSchemaStore store = new FileBasedSchemaStore(testfilepath);

				Assert.IsTrue(System.IO.File.Exists(testfilepath));
			}
			finally
			{
				System.IO.File.Delete(testfilepath);
			}
		}
		[TestMethod]
		public void Instantiate_existingfileused()
		{
			string testfilepath = "testexistingfile.txt";
			string testschemajson = "{\"testspace\":{\"testschema\":{\"SchemaName\":\"testschema\"}}}";
			System.IO.File.WriteAllText(testfilepath, testschemajson);

			try
			{
				FileBasedSchemaStore store = new FileBasedSchemaStore(testfilepath);
				Assert.IsTrue(System.IO.File.Exists(testfilepath));

				var thinginstore = store.GetSchemaElement(new DataSchemaReference() { Name = "testschema", NameSpace = "testspace" });
				Assert.AreEqual("testschema", thinginstore.SchemaName, "Fail if the expected schema is not available.");
			}
			finally
			{
				System.IO.File.Delete(testfilepath);
			}
		}

		[TestMethod]
		public void AddSchema()
		{
			DataSchema dataSchema = new DataSchema() { SchemaName="testschema"};
			dataSchema.AddElement(new SchemaElement() { Name = "testelement", Type = ElementType.StaticValue, StringValue = "testvalue" });

			const string SchemaFilePath = "testschemafile.txt";
			FileBasedSchemaStore store = new FileBasedSchemaStore(SchemaFilePath);
			store.AddDataSchema("testspace", dataSchema);
			DataSchema returnedSchema = store.GetSchemaElement(new DataSchemaReference() { Name = "testschema", NameSpace = "testspace" });
			Assert.AreEqual(dataSchema.SchemaName, returnedSchema.SchemaName);
			FileBasedSchemaStore store2 = new FileBasedSchemaStore(SchemaFilePath);
			returnedSchema = store2.GetSchemaElement(new DataSchemaReference() { Name = "testschema", NameSpace = "testspace" });
			Assert.AreEqual(dataSchema.SchemaName, returnedSchema.SchemaName, "Fail if the added schema did not persist so that new stores based on same file see it.");
		}
	}
}