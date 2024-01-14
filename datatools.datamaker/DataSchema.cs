namespace datatools.datamaker
{
	public class DataSchema
	{
		public string SchemaName { get; set; }
		private List<SchemaElement> elements = new List<SchemaElement>();
		public IList<SchemaElement> Elements
		{
			get { return elements; }
			set { elements.AddRange(value); }
		}

		public void AddElement(SchemaElement element) 
		{
			if(!SchemaElement.IsValidElement(element))
			{
				throw new ArgumentException("Schema element is invalid.");
			}
			this.elements.Add(element);
		}

		public static DataSchema LoadFromJson(string json)
		{
			DataSchema dataSchema = System.Text.Json.JsonSerializer.Deserialize<DataSchema>(json);

			List<SchemaElement> elementList	= dataSchema.elements;
			ThrowIfAnyElementsInListAreInvalid(elementList);

			return dataSchema;
		}

		private static void ThrowIfAnyElementsInListAreInvalid(List<SchemaElement> elementList)
		{
			foreach (var element in elementList)
			{
				if (!SchemaElement.IsValidElement(element))
				{
					throw new InvalidSchemaElementException(element.Name);
				}
				if (element.Type == ElementType.Choice || element.Type == ElementType.ElementList)
				{
					ThrowIfAnyElementsInListAreInvalid(element.ElementListValue.ToList());
				}
				if (element.Type == ElementType.Optional)
				{
					ThrowIfAnyElementsInListAreInvalid(new List<SchemaElement>() { element.ElementValue });
				}
			}
		}

		public static string GetJsonFromSchema(DataSchema dataSchema)
		{
			return System.Text.Json.JsonSerializer.Serialize(dataSchema);
		}

		public static DataSchemaAssessment AssessSchema(DataSchema dataSchema)
		{
			DataSchemaAssessment dataSchemaAssessment = new DataSchemaAssessment() { FullyTerminating = true };
			foreach(SchemaElement element in dataSchema.Elements)
			{
				dataSchemaAssessment = AssessSchemaElement(dataSchemaAssessment, element);
			}
			return dataSchemaAssessment;
		}

		private static DataSchemaAssessment AssessSchemaElement(DataSchemaAssessment dataSchemaAssessment, SchemaElement element)
		{
			DataSchemaAssessment tempAssessment = new DataSchemaAssessment()
			{
				FullyTerminating = dataSchemaAssessment.FullyTerminating,
				HasOptionalReferences = dataSchemaAssessment.HasOptionalReferences,
				HasOptionalSelfReference = dataSchemaAssessment.HasOptionalSelfReference,
				HasRequiredReferences = dataSchemaAssessment.HasRequiredReferences,
				HasRequiredSelfReference = dataSchemaAssessment.HasRequiredSelfReference
			};
			if (element.Type.Equals(ElementType.StaticValue))
			{
				tempAssessment.FullyTerminating = true;
				tempAssessment.HasOptionalReferences = false;
				tempAssessment.HasRequiredReferences = false;
			}
			if (element.Type.Equals(ElementType.Reference))
			{
				tempAssessment.FullyTerminating = false;
				tempAssessment.HasRequiredReferences = true;
			}
			if (element.Type.Equals(ElementType.Choice))
			{
				SchemaElement[] elements = element.ElementListValue;
				int fullyTermCount = 0;
				foreach(SchemaElement e in elements)
				{
					DataSchemaAssessment elementAssessment = AssessSchemaElement(tempAssessment, e);
					if(!elementAssessment.FullyTerminating)
					{
						tempAssessment.FullyTerminating = false;
						tempAssessment.HasOptionalReferences = true;
					}
					else
					{
						fullyTermCount++;
					}
				}
				if(!tempAssessment.FullyTerminating)
				{
					if(fullyTermCount > 0)
					{
						tempAssessment.HasRequiredReferences = false;
						tempAssessment.HasOptionalReferences = true;
					}
					else
					{
						tempAssessment.HasRequiredReferences = true;
						tempAssessment.HasOptionalSelfReference = false;
					}
				}
			}
			if (element.Type.Equals(ElementType.Optional))
			{
				DataSchemaAssessment elementAssessment = AssessSchemaElement(tempAssessment,(SchemaElement) element.ElementValue);
				if (!elementAssessment.FullyTerminating)
				{
					tempAssessment.FullyTerminating = false;
					tempAssessment.HasOptionalReferences = true;
				}
			}
			if (element.Type.Equals(ElementType.ElementList))
			{
				SchemaElement[] elements = (SchemaElement[])element.ElementListValue;
				foreach (SchemaElement e in elements)
				{
					DataSchemaAssessment elementAssessment = AssessSchemaElement(tempAssessment, e);
					if (!elementAssessment.FullyTerminating)
					{
						tempAssessment.FullyTerminating = false;
						tempAssessment.HasOptionalReferences = elementAssessment.HasOptionalReferences;
						tempAssessment.HasRequiredReferences = elementAssessment.HasRequiredReferences;
					}
				}

			}
			dataSchemaAssessment = tempAssessment;
			return dataSchemaAssessment;
		}
	}

	public class DataSchemaReference
	{
		public string NameSpace { get; set; }
		public string Name { get; set; }
	}

	public class DataSchemaAssessment
	{
		public bool FullyTerminating { get; set; } = false;
		public bool HasRequiredReferences { get; set; } = false;
		public bool HasRequiredSelfReference { get; set; } = false;	
		public bool HasOptionalReferences { get; set; } = false;
		public bool HasOptionalSelfReference { get; set; } = false;
	}
}
