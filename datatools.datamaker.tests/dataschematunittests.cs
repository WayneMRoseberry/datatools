using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using NuGet.Frameworks;
using System.Reflection.Metadata.Ecma335;

namespace datatools.datamaker.tests
{
	[TestClass]
	public class dataschemaunittests
	{
		[TestMethod]
		public void AssessSchema_emptyschema()
		{
			DataSchema dataSchema = new DataSchema();

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(true, assessment.FullyTerminating, "Fail if the schema is not fully terminating.");
		}

		[TestMethod]
		public void AssessSchema_elementlist_withreference()
		{
			SchemaElement[] elements = new SchemaElement[] 
			{
				new SchemaElement () { Name="elem1", Value="val1", Type = ElementType.StaticValue},
				new SchemaElement () { Name="elem2", Value= new DataSchemaReference() { Name="ref1", NameSpace="name.space1"}, Type=ElementType.Reference }
			};
			DataSchema dataSchema = new DataSchema();
			dataSchema.AddElement(new SchemaElement() { Name = "testelem", Value = elements, Type = ElementType.ElementList });

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(false, assessment.FullyTerminating, "Fail if the schema is fully terminating.");
			Assert.AreEqual(true, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is false.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(false, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is true.");
		}

		[TestMethod]
		public void AssessSchema_elementlist_allstatic()
		{
			SchemaElement[] elements = new SchemaElement[]
			{
				new SchemaElement () { Name="elem1", Value="val1", Type = ElementType.StaticValue},
				new SchemaElement () { Name="elem2", Value= "val2", Type=ElementType.StaticValue }
			};
			DataSchema dataSchema = new DataSchema();
			dataSchema.AddElement(new SchemaElement() { Name = "testelem", Value = elements, Type = ElementType.ElementList });

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(true, assessment.FullyTerminating, "Fail if the schema is not fully terminating.");
			Assert.AreEqual(false, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is true.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(false, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is true.");
		}

		[TestMethod]
		public void AssessSchema_reference()
		{
			DataSchema dataSchema = new DataSchema();
			dataSchema.AddElement(new SchemaElement() { Name = "testelem", Value = new DataSchemaReference() { Name = "ref1", NameSpace = "name.space1" }, Type = ElementType.Reference });

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(false, assessment.FullyTerminating, "Fail if the schema is fully terminating.");
			Assert.AreEqual(true, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is false.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(false, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is true.");
		}

		[TestMethod]
		public void AssessSchema_staticvalue()
		{
			DataSchema dataSchema = new DataSchema();
			dataSchema.AddElement(new SchemaElement() { Name = "testelem", Value = "value1", Type = ElementType.StaticValue });

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(true, assessment.FullyTerminating, "Fail if the schema is not fully terminating.");
			Assert.AreEqual(false, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is true.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(false, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is true.");
		}

		[TestMethod]
		public void AssessSchema_choicereference()
		{
			DataSchema dataSchema = new DataSchema();
			SchemaElement refElement = new SchemaElement()
			{ Name = "testelem", Value = new DataSchemaReference() { Name = "ref1", NameSpace = "name.space1" }, Type = ElementType.Reference };
			SchemaElement choiceElement = new SchemaElement()
			{
				Name = "choiceelem",
				Value = new SchemaElement[] { refElement},
				Type = ElementType.Choice
			};
			dataSchema.AddElement(
				choiceElement);

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(false, assessment.FullyTerminating, "Fail if the schema is fully terminating.");
			Assert.AreEqual(false, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is true.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(true, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is false.");
		}

		[TestMethod]
		public void AssessSchema_optionalrefference()
		{
			DataSchema dataSchema = new DataSchema();
			SchemaElement refElement = new SchemaElement()
			{ Name = "testelem", Value = new DataSchemaReference() { Name = "ref1", NameSpace = "name.space1" }, Type = ElementType.Reference };
			SchemaElement optionalElement = new SchemaElement()
			{
				Name = "optionelem",
				Value = refElement,
				Type = ElementType.Optional
			};
			dataSchema.AddElement(
				optionalElement);

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(false, assessment.FullyTerminating, "Fail if the schema is fully terminating.");
			Assert.AreEqual(false, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is true.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(true, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is false.");
		}
	}
	
}