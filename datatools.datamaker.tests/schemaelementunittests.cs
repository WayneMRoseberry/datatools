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

		[TestMethod]
		public void IsValidElement_rangenumernullvalue()
		{

			Assert.IsFalse(SchemaElement.IsValidElement(new SchemaElement()
			{
				Name = "testelement",
				MinValue = null,
				MaxValue = 1,
				Type = ElementType.RangeNumeric
			}), "Fail if MinValue is null.");
			Assert.IsFalse(SchemaElement.IsValidElement(new SchemaElement()
			{
				Name = "testelement",
				MinValue = 3,
				MaxValue = null,
				Type = ElementType.RangeNumeric
			}), "Fail if MaxValue is null.");
		}

		[TestMethod]
		public void IsValidElement_staticnullvalue()
		{

			Assert.IsFalse(SchemaElement.IsValidElement(new SchemaElement()
			{
				Name = "testelement",
				Value = null,
				Type = ElementType.StaticValue
			}), "Fail if Value is null.");
		}

		[TestMethod]
		public void IsValidElement_optionalnullvalue()
		{

			Assert.IsFalse(SchemaElement.IsValidElement(new SchemaElement()
			{
				Name = "testelement",
				Value = null,
				Type = ElementType.Optional
			}), "Fail if Value is null.");
		}

		[TestMethod]
		public void IsValidElement_choicenullvalue()
		{

			Assert.IsFalse(SchemaElement.IsValidElement(new SchemaElement()
			{
				Name = "testelement",
				Value = null,
				Type = ElementType.Choice
			}), "Fail if Value is null.");
		}
	}
	
}