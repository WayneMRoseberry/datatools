using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using NuGet.Frameworks;
using System.Reflection.Metadata.Ecma335;

namespace datatools.datamaker.tests
{
	[TestClass]
	public class datamakerunittests
	{

		[TestMethod]
		public void GetRandomExample_choicetwovalues()
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
		public void GetRandomExample_choicefourvalues()
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
		public void GetRandomExample_elementlist()
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
		public void GetRandomExample_optional()
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
		public void GetRandomExample_optionalelementlist()
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
		public void GetRandomExample_rangenumeric()
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
		[ExpectedException(typeof(ArgumentException))]
		public void GetRandomExample_rangenumericcrossvalues()
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
		public void GetRandomExample_singlestaticvalue()
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
		public void GetRandomExample_twostaticvalues()
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