using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatools.datamaker.tests
{
	internal class MockSchemaStore : ISchemaStore
	{
		public Func<DataSchemaReference, DataSchema> overrideGetSchemaElement = (d) => { throw new NotImplementedException(); };

		public void AddDataSchema(string nameSpace, DataSchema dataSchema)
		{
			throw new NotImplementedException();
		}

		public DataSchema GetSchemaElement(DataSchemaReference schemaReference)
		{
			return overrideGetSchemaElement(schemaReference);
		}
	}
}
