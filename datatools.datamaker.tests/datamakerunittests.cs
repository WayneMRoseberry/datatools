using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using NuGet.Frameworks;
using System.Reflection.Metadata.Ecma335;

namespace datatools.datamaker.tests
{
	[TestClass]
	public class datamakerunittests
	{

		[TestMethod]
		public void GetExample_choicetwovalues_randomchooser()
		{
			DataSchema schema = new DataSchema();

			SchemaElement[] elemArray = new SchemaElement[]
			{
				new SchemaElement () { Name="choice1", StringValue="ch1", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice2", StringValue="ch2", Type=ElementType.StaticValue}
			};

			SchemaElement element = new SchemaElement()
			{
				Name = "element1",
				ElementListValue = elemArray,
				Type = ElementType.Choice
			};
			schema.AddElement(element);

			string result = DataMaker.GetExample(schema, new RandomChooser(), new MockSchemaStore());

			Assert.IsTrue(result.Equals("ch1") || result.Equals("ch2"), $"Fail if result <> 'ch1' or 'ch2', result={result}");
		}

		[TestMethod]
		public void GetExample_choicetwovalues_mockchooser()
		{

			int passedInLength = -1;
			MockChooser mockChooser = new MockChooser();
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 0; };

			DataSchema schema = new DataSchema();

			SchemaElement[] elemArray = new SchemaElement[]
			{
				new SchemaElement () { Name="choice1", StringValue="ch1", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice2", StringValue="ch2", Type=ElementType.StaticValue}
			};

			SchemaElement element = new SchemaElement()
			{
				Name = "element1",
				ElementListValue = elemArray,
				Type = ElementType.Choice
			};
			schema.AddElement(element);

			Assert.AreEqual("ch1", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not expected choice.");
			Assert.AreEqual(2, passedInLength, "Fail if wrong length passed in to ChooseNumber.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 1; };
			Assert.AreEqual("ch2", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not expected choice.");
			Assert.AreEqual(2, passedInLength, "Fail if wrong length passed in to ChooseNumber.");

		}

		[TestMethod]
		public void GetExample_choicefourvalues()
		{
			DataSchema schema = new DataSchema();

			SchemaElement[] elemArray = new SchemaElement[]
			{
				new SchemaElement () { Name="choice1", StringValue="ch1", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice2", StringValue="ch2", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice3", StringValue="ch3", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice4", StringValue="ch4", Type=ElementType.StaticValue }
			};

			SchemaElement element = new SchemaElement()
			{
				Name = "element1",
				ElementListValue = elemArray,
				Type = ElementType.Choice
			};
			schema.AddElement(element);

			List<string> results = new List<string>();
			for(int i =0; i < 40; i++)
			{

				string result = DataMaker.GetExample(schema, new RandomChooser(), new MockSchemaStore());
				if(!results.Contains(result))
				{
					results.Add(result);
				}

			}

			Assert.IsTrue(results.Contains("ch1"),"Fail if results does not contain ch1");
			Assert.IsTrue(results.Contains("ch2"), "Fail if results does not contain ch1");
			Assert.IsTrue(results.Contains("ch3"), "Fail if results does not contain ch1");
			Assert.IsTrue(results.Contains("ch4"), "Fail if results does not contain ch1");
		}

		[TestMethod]
		public void GetExample_elementlist()
		{
			DataSchema schema = new DataSchema();

			SchemaElement[] elementList = new SchemaElement[] 
			{
				new SchemaElement() { Name="firstthing", StringValue="mefirst", Type=ElementType.StaticValue },
				new SchemaElement() { Name="separator", StringValue="_", Type=ElementType.StaticValue},
				new SchemaElement() { Name="secondthing", StringValue="melast", Type=ElementType.StaticValue}
			};
			schema.AddElement(new SchemaElement()
			{
				Name = "element1",
				ElementListValue = elementList,
				Type = ElementType.ElementList
			}
			);

			string result = DataMaker.GetExample(schema, new RandomChooser(), new MockSchemaStore());

			Assert.AreEqual("mefirst_melast", result);
		}

		[TestMethod]
		public void GetExample_optional_randomChooser()
		{
			DataSchema schema = new DataSchema();

			schema.AddElement(new SchemaElement()
			{
				Name = "prefix",
				StringValue = "firstpart",
				Type = ElementType.StaticValue
			});
			schema.AddElement(new SchemaElement()
			{
				Name = "optionalpostfix",
				ElementValue = new SchemaElement() { Name = "secondoption", StringValue = "secondoption", Type = ElementType.StaticValue },
				Type = ElementType.Optional
			});


			List<string> results = new List<string>();
			for(int i = 0; i < 50; i++)
			{
				string result = DataMaker.GetExample(schema, new RandomChooser(), new MockSchemaStore());
				if(!results.Contains(result))
				{
					results.Add(result);
				}
			}

			string resultsList = string.Join(',', results);
			Logger.LogMessage($"results = [{resultsList}]");

			Assert.IsTrue(results.Contains("firstpart"), "fail if results does not contain 'firstpart'");
			Assert.IsTrue(results.Contains("firstpartsecondoption"), $"fail if results does not contain 'firstpartoptionalpostfix'.");

		}

		[TestMethod]
		public void GetExample_optional_mockChooser()
		{
			int passedInLength = -1;
			MockChooser mockChooser = new MockChooser();
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 0; };
			DataSchema schema = new DataSchema();

			schema.AddElement(new SchemaElement()
			{
				Name = "prefix",
				StringValue = "firstpart",
				Type = ElementType.StaticValue
			});
			schema.AddElement(new SchemaElement()
			{
				Name = "optionalpostfix",
				ElementValue = new SchemaElement() { Name="secondoption", StringValue = "secondoption", Type=ElementType.StaticValue},
				Type = ElementType.Optional
			});

			Assert.AreEqual("firstpart", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not as expected.");
			Assert.AreEqual(2, passedInLength, "Fail if the length passed to ChooseNumber is not as expected.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 1; };
			Assert.AreEqual("firstpartsecondoption", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not as expected.");
			Assert.AreEqual(2, passedInLength, "Fail if the length passed to ChooseNumber is not as expected.");
		}

		[TestMethod]
		public void GetExample_optionalelementlist()
		{
			DataSchema schema = new DataSchema();

			SchemaElement[] elementList = new SchemaElement[] 
			{
				new SchemaElement() { Name="optfirst", StringValue="optionallyfirst", Type=ElementType.StaticValue},
				new SchemaElement() { Name="optsecond", StringValue="optionallysecond", Type=ElementType.StaticValue}
			};

			SchemaElement optionElement = new SchemaElement()
			{
				Name = "optional",
				ElementValue = new SchemaElement() { Name="listthing", ElementListValue=elementList, Type=ElementType.ElementList},
				Type = ElementType.Optional
			};

			schema.AddElement(new SchemaElement()
			{
				Name = "prefix",
				StringValue = "firstpart",
				Type = ElementType.StaticValue
			});
			schema.AddElement(new SchemaElement()
			{
				Name = "optionalpostfix",
				ElementValue = optionElement,
				Type = ElementType.Optional
			});

			List<string> results = new List<string>();
			for (int i = 0; i < 50; i++)
			{
				string result = DataMaker.GetExample(schema, new RandomChooser(), new MockSchemaStore());
				if (!results.Contains(result))
				{
					results.Add(result);
				}
			}

			string resultsList = string.Join(',', results);
			Logger.LogMessage($"results = [{resultsList}]");

			Assert.IsTrue(results.Contains("firstpart"), "fail if results does not contain 'firstpart'");
			Assert.IsTrue(results.Contains("firstpartoptionallyfirstoptionallysecond"), $"fail if results does not contain 'firstpartoptionallyfirstoptionallysecond'.");

		}

		[TestMethod]
		public void GetExample_rangealpha_mockedChooser()
		{
			int passedInLength = -1;
			MockChooser mockChooser = new MockChooser();
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 0; };

			DataSchema schema = new DataSchema();
			schema.AddElement(new SchemaElement()
			{
				Name = "element1",
				AlphaMinValue = 'a',
				AlphaMaxValue = 'z',
				Type = ElementType.RangeAlpha
			}
			);

			Assert.AreEqual("a", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not as expected.");
			Assert.AreEqual(26, passedInLength, "Fail if wrong range was passed to ChooseNumber.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 1; };
			Assert.AreEqual("b", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not as expected.");
			Assert.AreEqual(26, passedInLength, "Fail if wrong range was passed to ChooseNumber.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 2; };
			Assert.AreEqual("c", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not as expected.");
			Assert.AreEqual(26, passedInLength, "Fail if wrong range was passed to ChooseNumber.");
		}

		[TestMethod]
		public void GetExample_rangenumeric_randomchooser()
		{
			DataSchema schema = new DataSchema();
			schema.AddElement(new SchemaElement()
			{
				Name = "element1",
				NumericMinValue = 3,
				NumericMaxValue = 5,
				Type = ElementType.RangeNumeric
			}
			);


			List<int> results = new List<int>();
			for(int i = 0; i < 100; i++)
			{
				int numeric = Convert.ToInt32(DataMaker.GetExample(schema, new RandomChooser(), new MockSchemaStore()));
				if(!results.Contains(numeric))
				{
					results.Add(numeric);
				}
			}

			Assert.IsTrue(results.Contains(3), "Fail if results does not contain 3.");
			Assert.IsTrue(results.Contains(4), "Fail if results does not contain 4.");
			Assert.IsTrue(results.Contains(5), "Fail if results does not contain 5.");
			Assert.AreEqual(3, results.Count, "Fail if results has more than 3 items.");
		}

		[TestMethod]
		public void GetExample_rangenumeric_mockedChooser()
		{
			int passedInLength = -1;
			MockChooser mockChooser = new MockChooser();
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 0; };

			DataSchema schema = new DataSchema();
			schema.AddElement(new SchemaElement()
			{
				Name = "element1",
				NumericMinValue = 3,
				NumericMaxValue = 5,
				Type = ElementType.RangeNumeric
			}
			);

			Assert.AreEqual("3", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not as expected.");
			Assert.AreEqual(3, passedInLength, "Fail if wrong range was passed to ChooseNumber.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 1; };
			Assert.AreEqual("4", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not as expected.");
			Assert.AreEqual(3, passedInLength, "Fail if wrong range was passed to ChooseNumber.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 2; };
			Assert.AreEqual("5", DataMaker.GetExample(schema, mockChooser, new MockSchemaStore()), "Fail if returned string is not as expected.");
			Assert.AreEqual(3, passedInLength, "Fail if wrong range was passed to ChooseNumber.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetExample_rangenumericcrossvalues()
		{
			DataSchema schema = new DataSchema();
			schema.AddElement(new SchemaElement()
			{
				Name = "element1",
				NumericMinValue = 3,
				NumericMaxValue = 2,
				Type = ElementType.RangeNumeric
			}
			);
			string result = DataMaker.GetExample(schema, new RandomChooser(), new MockSchemaStore());
		}

		[TestMethod]
		public void GetExample_reference()
		{
			DataSchemaReference passedInReference = null;
			MockSchemaStore schemaStore = new MockSchemaStore();
			schemaStore.overrideGetSchemaElement = (d) =>
			{
				passedInReference = d;
				DataSchema dataSchema = new DataSchema() { };
				dataSchema.AddElement(new SchemaElement() { Name = "referencedme", StringValue = "this is me", Type = ElementType.StaticValue });
				return dataSchema;
			};
			MockChooser mockChooser = new MockChooser();

			SchemaElement schemaElement = new SchemaElement()
			{
				Name="testelem",
				RefValue = new DataSchemaReference() { Name="testref", NameSpace="test.namespace"},
				Type = ElementType.Reference
			};
			DataSchema inputSchema = new DataSchema();
			inputSchema.AddElement(schemaElement);
			string result = DataMaker.GetExample(inputSchema, mockChooser, schemaStore);

			Assert.AreNotEqual(null, passedInReference, "Fail if passedInReference is null.");
			Assert.AreEqual("testref", passedInReference.Name, "Fail if reference did not pass in expected name.");
			Assert.AreEqual("test.namespace", passedInReference.NameSpace, "Fail if reference did not pass in expected namespace.");
			Assert.AreEqual("this is me", result, "Fail if result is not expected value.");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void GetExample_reference_null()
		{
			MockSchemaStore schemaStore = new MockSchemaStore();
			MockChooser mockChooser = new MockChooser();

			SchemaElement schemaElement = new SchemaElement()
			{
				Name = "testelem",
				RefValue = null,
				Type = ElementType.Reference
			};
			DataSchema inputSchema = new DataSchema();
			inputSchema.AddElement(schemaElement);
			string result = DataMaker.GetExample(inputSchema, mockChooser, schemaStore);
		}

		[TestMethod]
		[ExpectedException(typeof(InfinitelyRecursiveSchemaException))]
		public void GetExample_reference_circular_throws()
		{
			SchemaElement schemaElement = new SchemaElement()
			{
				Name = "testelem",
				RefValue = new DataSchemaReference() { Name = "testref", NameSpace = "test.namespace" },
				Type = ElementType.Reference
			};
			MockSchemaStore schemaStore = new MockSchemaStore();
			schemaStore.overrideGetSchemaElement = (d) =>
			{
				SchemaElement schemaElement2 = new SchemaElement()
				{
					Name = "testelem",
					RefValue = new DataSchemaReference() { Name = "testref", NameSpace = "test.namespace" },
					Type = ElementType.Reference
				};
				DataSchema dataSchema = new DataSchema() { };
				dataSchema.AddElement(schemaElement2);
				return dataSchema;
			};
			MockChooser mockChooser = new MockChooser();
			DataSchema inputSchema = new DataSchema();
			inputSchema.AddElement(schemaElement);
			string result = DataMaker.GetExample(inputSchema, mockChooser, schemaStore);
		}

		[TestMethod]
		public void GetExample_singlestaticvalue()
		{
			DataSchema schema = new DataSchema();
			schema.AddElement(new SchemaElement() 
			{
				Name = "element1",
				StringValue = "val1",
				Type = ElementType.StaticValue
			}
			);

			string result = DataMaker.GetExample( schema, new RandomChooser(), new MockSchemaStore());

			Assert.AreEqual("val1", result);
		}

		[TestMethod]
		public void GetExample_twostaticvalues()
		{
			DataSchema schema = new DataSchema();
			schema.AddElement(new SchemaElement()
			{
				Name = "element1",
				StringValue = "val1",
				Type = ElementType.StaticValue
			}
			);
			schema.AddElement(new SchemaElement()
			{
				Name = "element2",
				StringValue = "val2",
				Type = ElementType.StaticValue
			}
			);

			string result = DataMaker.GetExample(schema, new RandomChooser(), new MockSchemaStore());

			Assert.AreEqual("val1val2", result);
		}
	}
}