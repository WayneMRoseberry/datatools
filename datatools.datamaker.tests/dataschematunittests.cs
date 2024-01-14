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
				new SchemaElement () { Name="elem1", StringValue="val1", Type = ElementType.StaticValue},
				new SchemaElement () { Name="elem2", RefValue= new DataSchemaReference() { Name="ref1", NameSpace="name.space1"}, Type=ElementType.Reference }
			};
			DataSchema dataSchema = new DataSchema();
			dataSchema.AddElement(new SchemaElement() { Name = "testelem", ElementListValue = elements, Type = ElementType.ElementList });

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
				new SchemaElement () { Name="elem1", StringValue="val1", Type = ElementType.StaticValue},
				new SchemaElement () { Name="elem2", StringValue= "val2", Type=ElementType.StaticValue }
			};
			DataSchema dataSchema = new DataSchema();
			dataSchema.AddElement(new SchemaElement() { Name = "testelem", ElementListValue = elements, Type = ElementType.ElementList });

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
			dataSchema.AddElement(new SchemaElement() { Name = "testelem", RefValue = new DataSchemaReference() { Name = "ref1", NameSpace = "name.space1" }, Type = ElementType.Reference });

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
			dataSchema.AddElement(new SchemaElement() { Name = "testelem", StringValue = "value1", Type = ElementType.StaticValue });

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(true, assessment.FullyTerminating, "Fail if the schema is not fully terminating.");
			Assert.AreEqual(false, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is true.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(false, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is true.");
		}

		[TestMethod]
		public void AssessSchema_choicereference_onechoice()
		{
			DataSchema dataSchema = new DataSchema();
			SchemaElement refElement = new SchemaElement()
			{ Name = "testelem", RefValue = new DataSchemaReference() { Name = "ref1", NameSpace = "name.space1" }, Type = ElementType.Reference };
			SchemaElement choiceElement = new SchemaElement()
			{
				Name = "choiceelem",
				ElementListValue = new SchemaElement[] { refElement},
				Type = ElementType.Choice
			};
			dataSchema.AddElement(
				choiceElement);

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(false, assessment.FullyTerminating, "Fail if the schema is fully terminating.");
			Assert.AreEqual(true, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is false.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(true, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is false.");
		}

		[TestMethod]
		public void AssessSchema_choicereference_tworefchoice()
		{
			DataSchema dataSchema = new DataSchema();
			SchemaElement refElement = new SchemaElement()
			{ Name = "testelem", RefValue = new DataSchemaReference() { Name = "ref1", NameSpace = "name.space1" }, Type = ElementType.Reference };
			SchemaElement choiceElement = new SchemaElement()
			{
				Name = "choiceelem",
				ElementListValue = new SchemaElement[] { refElement },
				Type = ElementType.Choice
			};
			SchemaElement refElement2 = new SchemaElement()
			{ Name = "testelem2", RefValue = new DataSchemaReference() { Name = "ref1", NameSpace = "name.space1" }, Type = ElementType.Reference };
			SchemaElement choiceElement2 = new SchemaElement()
			{
				Name = "choiceelem2",
				ElementListValue = new SchemaElement[] { refElement2 },
				Type = ElementType.Choice
			};
			dataSchema.AddElement(
				choiceElement);
			dataSchema.AddElement(
				choiceElement2);

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(false, assessment.FullyTerminating, "Fail if the schema is fully terminating.");
			Assert.AreEqual(true, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is false.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(true, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is false.");
		}

		[TestMethod]
		public void AssessSchema_choiceallstatic()
		{
			DataSchema dataSchema = new DataSchema();
			SchemaElement static1 = new SchemaElement()
			{ Name = "testelem1", StringValue = "val1", Type = ElementType.StaticValue };
			SchemaElement choiceElement = new SchemaElement()
			{
				Name = "choiceelem1",
				ElementListValue = new SchemaElement[] { static1 },
				Type = ElementType.Choice
			};
			SchemaElement static2 = new SchemaElement()
			{ Name = "testelem1", StringValue = "val1", Type = ElementType.StaticValue };
			SchemaElement choiceElement2 = new SchemaElement()
			{
				Name = "choiceelem2",
				ElementListValue = new SchemaElement[] { static2 },
				Type = ElementType.Choice
			};
			dataSchema.AddElement(
				choiceElement);
			dataSchema.AddElement(
				choiceElement2);

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(true, assessment.FullyTerminating, "Fail if the schema is not fully terminating.");
			Assert.AreEqual(false, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is true.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(false, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is true.");
		}

		[TestMethod]
		public void AssessSchema_choicestaticrefmix()
		{
			DataSchema dataSchema = new DataSchema();
			SchemaElement staticElement = new SchemaElement()
			{
				Name = "choiceelem1",
				StringValue = "val1",
				Type = ElementType.StaticValue
			};
			SchemaElement refElem = new SchemaElement()
			{ Name = "testelem2", RefValue = new DataSchemaReference() { Name="ref1", NameSpace="name.space1" }, Type = ElementType.Reference };
			SchemaElement choiceElement2 = new SchemaElement()
			{
				Name = "choiceelem2",
				ElementListValue = new SchemaElement[] { refElem, staticElement },
				Type = ElementType.Choice
			};
			dataSchema.AddElement(
				choiceElement2);

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
			{ Name = "testelem", RefValue = new DataSchemaReference() { Name = "ref1", NameSpace = "name.space1" }, Type = ElementType.Reference };
			SchemaElement optionalElement = new SchemaElement()
			{
				Name = "optionelem",
				ElementValue = refElement,
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

		[TestMethod]
		public void AssessSchema_optionalallstatic()
		{
			DataSchema dataSchema = new DataSchema();
			SchemaElement static1 = new SchemaElement()
			{ Name = "testelem", StringValue = "val1", Type = ElementType.StaticValue };
			SchemaElement optionalElement = new SchemaElement()
			{
				Name = "staticelem",
				ElementValue = static1,
				Type = ElementType.Optional
			};
			dataSchema.AddElement(
				optionalElement);

			DataSchemaAssessment assessment = DataSchema.AssessSchema(dataSchema);
			Assert.AreEqual(true, assessment.FullyTerminating, "Fail if the schema is not fully terminating.");
			Assert.AreEqual(false, assessment.HasRequiredReferences, "Fail if HasRequiredReferences is true.");
			Assert.AreEqual(false, assessment.HasRequiredSelfReference, "Fail if HasRequiredSelfReference is true.");
			Assert.AreEqual(false, assessment.HasOptionalSelfReference, "Fail if HasOptionalSelfReferences is true.");
			Assert.AreEqual(false, assessment.HasOptionalReferences, "Fail if HasOptionalReferences is true.");
		}

		[TestMethod]
		public void GetJsonFromSchema_singlestatic()
		{
			DataSchema dataSchema = new DataSchema() { SchemaName = "testschema" };
			SchemaElement element = new SchemaElement() { Name = "testelem", StringValue = "testval", Type = ElementType.StaticValue };
			dataSchema.AddElement(element);
			string json = DataSchema.GetJsonFromSchema(dataSchema);
			DataSchema roundTripSchema = DataSchema.LoadFromJson(json);
			SchemaElement roundTripElement = roundTripSchema.Elements[0];

			Assert.AreEqual(dataSchema.SchemaName, roundTripSchema.SchemaName, "Fail if schema names do not match.");
			Assert.AreEqual(element.Name, roundTripElement.Name, "Fail if first element name does not match.");
			Assert.AreEqual(element.Type, roundTripElement.Type, "Fail if first element type does not match.");
			Assert.AreEqual(element.StringValue, roundTripElement.StringValue, "Fail if first element stringvalue does not match.");
		}

		[TestMethod]
		public void GetJsonFromSchema_rangenumeric()
		{
			DataSchema dataSchema = new DataSchema() { SchemaName = "testschema" };
			SchemaElement element = new SchemaElement() { Name = "testelem", NumericMinValue = 10, NumericMaxValue=20, Type = ElementType.RangeNumeric };
			dataSchema.AddElement(element);
			string json = DataSchema.GetJsonFromSchema(dataSchema);
			DataSchema roundTripSchema = DataSchema.LoadFromJson(json);
			SchemaElement roundTripElement = roundTripSchema.Elements[0];

			Assert.AreEqual(dataSchema.SchemaName, roundTripSchema.SchemaName, "Fail if schema names do not match.");
			Assert.AreEqual(element.Name, roundTripElement.Name, "Fail if first element name does not match.");
			Assert.AreEqual(element.Type, roundTripElement.Type, "Fail if first element type does not match.");
			Assert.AreEqual(element.NumericMinValue, roundTripElement.NumericMinValue, "Fail if first element minvalue does not match.");
			Assert.AreEqual(element.NumericMaxValue, roundTripElement.NumericMaxValue, "Fail if first element max does not match.");
		}

		[TestMethod]
		public void GetJsonFromSchema_rangealpha()
		{
			DataSchema dataSchema = new DataSchema() { SchemaName = "testschema" };
			SchemaElement element = new SchemaElement() { Name = "testelem", AlphaMinValue = 'l', AlphaMaxValue = 't', Type = ElementType.RangeAlpha };
			dataSchema.AddElement(element);
			string json = DataSchema.GetJsonFromSchema(dataSchema);
			DataSchema roundTripSchema = DataSchema.LoadFromJson(json);
			SchemaElement roundTripElement = roundTripSchema.Elements[0];

			Assert.AreEqual(dataSchema.SchemaName, roundTripSchema.SchemaName, "Fail if schema names do not match.");
			Assert.AreEqual(element.Name, roundTripElement.Name, "Fail if first element name does not match.");
			Assert.AreEqual(element.Type, roundTripElement.Type, "Fail if first element type does not match.");
			Assert.AreEqual(element.AlphaMinValue, roundTripElement.AlphaMinValue, "Fail if first element minvalue does not match.");
			Assert.AreEqual(element.AlphaMaxValue, roundTripElement.AlphaMaxValue, "Fail if first element max does not match.");
		}

		[TestMethod]
		public void LoadFromJson_minimalstaticvalue()
		{
			DataSchema dataSchema = DataSchema.LoadFromJson("{\"SchemaName\":\"testschema\",\"Elements\":[{\"Name\":\"testelem\",\"StringValue\":\"testval\",\"Type\":0}]}");
			SchemaElement element = dataSchema.Elements[0];

			Assert.AreEqual("testschema", dataSchema.SchemaName, "Fail if the schema name does not match.");
			Assert.AreEqual("testelem", element.Name, "Fail if element name does not match.");
			Assert.AreEqual("testval", element.StringValue, "Fail if stringvalue does not match.");
			Assert.AreEqual(ElementType.StaticValue, element.Type, "Fail if element type does not match.");
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidSchemaElementException))]
		public void LoadFromJson_minimalstaticvalue_nullvalue()
		{
			DataSchema dataSchema = DataSchema.LoadFromJson("{\"SchemaName\":\"testschema\",\"Elements\":[{\"Name\":\"testelem\",\"Type\":0}]}");
			SchemaElement element = dataSchema.Elements[0];

			Assert.AreEqual("testschema", dataSchema.SchemaName, "Fail if the schema name does not match.");
			Assert.AreEqual("testelem", element.Name, "Fail if element name does not match.");
			Assert.AreEqual("testval", element.StringValue, "Fail if stringvalue does not match.");
			Assert.AreEqual(ElementType.StaticValue, element.Type, "Fail if element type does not match.");
		}
	}
	
}