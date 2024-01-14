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
				NumericMinValue = 3,
				NumericMaxValue = 1,
				Type = ElementType.RangeNumeric
			};

			Assert.IsFalse(SchemaElement.IsValidElement(schemaElement), "Fail if a crossed numeric range returns IsValidElement==true.");
		}

		[TestMethod]
		public void IsValidElement_staticnullvalue()
		{

			Assert.IsFalse(SchemaElement.IsValidElement(new SchemaElement()
			{
				Name = "testelement",
				StringValue = null,
				Type = ElementType.StaticValue
			}), "Fail if Value is null.");
		}

		[TestMethod]
		public void IsValidElement_optionalnullvalue()
		{

			Assert.IsFalse(SchemaElement.IsValidElement(new SchemaElement()
			{
				Name = "testelement",
				ElementValue = null,
				Type = ElementType.Optional
			}), "Fail if Value is null.");
		}

		[TestMethod]
		public void IsValidElement_choicenullvalue()
		{

			Assert.IsFalse(SchemaElement.IsValidElement(new SchemaElement()
			{
				Name = "testelement",
				ElementListValue = null,
				Type = ElementType.Choice
			}), "Fail if Value is null.");
		}
	}
	
}