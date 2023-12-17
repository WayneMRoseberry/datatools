using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using NuGet.Frameworks;
using System.Reflection.Metadata.Ecma335;

namespace datatools.datamaker.tests
{
	[TestClass]
	public class schemaelementunittests
	{
		[TestMethod]

		public void IsValidElement_crossednumericrange()
		{
			SchemaElement schemaElement = new SchemaElement()
			{
				Name = "testelement",
				MinValue = 3,
				MaxValue = 1,
				Type = ElementType.RangeNumeric
			};

			Assert.IsFalse(SchemaElement.IsValidElement(schemaElement), "Fail if a crossed numeric range returns IsValidElement==true.");
		}
	}
	
}