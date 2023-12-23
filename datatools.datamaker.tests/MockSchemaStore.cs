using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatools.datamaker.tests
{
	internal class MockSchemaStore : ISchemaStore
	{
		public void AddDataSchema(string nameSpace, DataSchema dataSchema)
		{
			throw new NotImplementedException();
		}

		public DataSchema GetSchemaElement(DataSchemaReference schemaReference)
		{
			throw new NotImplementedException();
		}
	}
}
