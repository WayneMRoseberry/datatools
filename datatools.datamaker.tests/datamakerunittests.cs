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
				new SchemaElement () { Name="choice1", Value="ch1", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice2", Value="ch2", Type=ElementType.StaticValue}
			};

			SchemaElement element = new SchemaElement()
			{
				Name = "element1",
				Value = elemArray,
				Type = ElementType.Choice
			};
			schema.AddElement(element);

			string result = DataMaker.GetExample(schema, new RandomChooser());

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
				new SchemaElement () { Name="choice1", Value="ch1", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice2", Value="ch2", Type=ElementType.StaticValue}
			};

			SchemaElement element = new SchemaElement()
			{
				Name = "element1",
				Value = elemArray,
				Type = ElementType.Choice
			};
			schema.AddElement(element);

			Assert.AreEqual("ch1", DataMaker.GetExample(schema, mockChooser), "Fail if returned string is not expected choice.");
			Assert.AreEqual(2, passedInLength, "Fail if wrong length passed in to ChooseNumber.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 1; };
			Assert.AreEqual("ch2", DataMaker.GetExample(schema, mockChooser), "Fail if returned string is not expected choice.");
			Assert.AreEqual(2, passedInLength, "Fail if wrong length passed in to ChooseNumber.");

		}

		[TestMethod]
		public void GetExample_choicefourvalues()
		{
			DataSchema schema = new DataSchema();

			SchemaElement[] elemArray = new SchemaElement[]
			{
				new SchemaElement () { Name="choice1", Value="ch1", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice2", Value="ch2", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice3", Value="ch3", Type=ElementType.StaticValue },
				new SchemaElement () { Name="choice4", Value="ch4", Type=ElementType.StaticValue }
			};

			SchemaElement element = new SchemaElement()
			{
				Name = "element1",
				Value = elemArray,
				Type = ElementType.Choice
			};
			schema.AddElement(element);

			List<string> results = new List<string>();
			for(int i =0; i < 40; i++)
			{

				string result = DataMaker.GetExample(schema, new RandomChooser());
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
				new SchemaElement() { Name="firstthing", Value="mefirst", Type=ElementType.StaticValue },
				new SchemaElement() { Name="separator", Value="_", Type=ElementType.StaticValue},
				new SchemaElement() { Name="secondthing", Value="melast", Type=ElementType.StaticValue}
			};
			schema.AddElement(new SchemaElement()
			{
				Name = "element1",
				Value = elementList,
				Type = ElementType.ElementList
			}
			);

			string result = DataMaker.GetExample(schema, new RandomChooser());

			Assert.AreEqual("mefirst_melast", result);
		}

		[TestMethod]
		public void GetExample_optional_randomChooser()
		{
			DataSchema schema = new DataSchema();

			schema.AddElement(new SchemaElement()
			{
				Name = "prefix",
				Value = "firstpart",
				Type = ElementType.StaticValue
			});
			schema.AddElement(new SchemaElement()
			{
				Name = "optionalpostfix",
				Value = "secondoption",
				Type = ElementType.Optional
			});


			List<string> results = new List<string>();
			for(int i = 0; i < 50; i++)
			{
				string result = DataMaker.GetExample(schema, new RandomChooser());
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
				Value = "firstpart",
				Type = ElementType.StaticValue
			});
			schema.AddElement(new SchemaElement()
			{
				Name = "optionalpostfix",
				Value = "secondoption",
				Type = ElementType.Optional
			});

			Assert.AreEqual("firstpart", DataMaker.GetExample(schema, mockChooser), "Fail if returned string is not as expected.");
			Assert.AreEqual(2, passedInLength, "Fail if the length passed to ChooseNumber is not as expected.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 1; };
			Assert.AreEqual("firstpartsecondoption", DataMaker.GetExample(schema, mockChooser), "Fail if returned string is not as expected.");
			Assert.AreEqual(2, passedInLength, "Fail if the length passed to ChooseNumber is not as expected.");
		}

		[TestMethod]
		public void GetExample_optionalelementlist()
		{
			DataSchema schema = new DataSchema();

			SchemaElement[] elementList = new SchemaElement[] 
			{
				new SchemaElement() { Name="optfirst", Value="optionallyfirst", Type=ElementType.StaticValue},
				new SchemaElement() { Name="optsecond", Value="optionallysecond", Type=ElementType.StaticValue}
			};

			SchemaElement optionElement = new SchemaElement()
			{
				Name = "optional",
				Value = elementList,
				Type = ElementType.Optional
			};

			schema.AddElement(new SchemaElement()
			{
				Name = "prefix",
				Value = "firstpart",
				Type = ElementType.StaticValue
			});
			schema.AddElement(new SchemaElement()
			{
				Name = "optionalpostfix",
				Value = optionElement,
				Type = ElementType.Optional
			});

			List<string> results = new List<string>();
			for (int i = 0; i < 50; i++)
			{
				string result = DataMaker.GetExample(schema, new RandomChooser());
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
		public void GetExample_rangenumeric_randomchooser()
		{
			DataSchema schema = new DataSchema();
			schema.AddElement(new SchemaElement()
			{
				Name = "element1",
				MinValue = 3,
				MaxValue = 5,
				Type = ElementType.RangeNumeric
			}
			);


			List<int> results = new List<int>();
			for(int i = 0; i < 100; i++)
			{
				int numeric = Convert.ToInt32(DataMaker.GetExample(schema, new RandomChooser()));
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
				MinValue = 3,
				MaxValue = 5,
				Type = ElementType.RangeNumeric
			}
			);

			Assert.AreEqual("3", DataMaker.GetExample(schema, mockChooser), "Fail if returned string is not as expected.");
			Assert.AreEqual(3, passedInLength, "Fail if wrong range was passed to ChooseNumber.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 1; };
			Assert.AreEqual("4", DataMaker.GetExample(schema, mockChooser), "Fail if returned string is not as expected.");
			Assert.AreEqual(3, passedInLength, "Fail if wrong range was passed to ChooseNumber.");
			mockChooser.overrideChooseNumber = (i) => { passedInLength = i; return 2; };
			Assert.AreEqual("5", DataMaker.GetExample(schema, mockChooser), "Fail if returned string is not as expected.");
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
				MinValue = 3,
				MaxValue = 2,
				Type = ElementType.RangeNumeric
			}
			);
			string result = DataMaker.GetExample(schema, new RandomChooser());
		}

		[TestMethod]
		public void GetExample_singlestaticvalue()
		{
			DataSchema schema = new DataSchema();
			schema.AddElement(new SchemaElement() 
			{
				Name = "element1",
				Value = "val1",
				Type = ElementType.StaticValue
			}
			);

			string result = DataMaker.GetExample( schema, new RandomChooser());

			Assert.AreEqual("val1", result);
		}

		[TestMethod]
		public void GetExample_twostaticvalues()
		{
			DataSchema schema = new DataSchema();
			schema.AddElement(new SchemaElement()
			{
				Name = "element1",
				Value = "val1",
				Type = ElementType.StaticValue
			}
			);
			schema.AddElement(new SchemaElement()
			{
				Name = "element2",
				Value = "val2",
				Type = ElementType.StaticValue
			}
			);

			string result = DataMaker.GetExample(schema, new RandomChooser());

			Assert.AreEqual("val1val2", result);
		}
	}
}