namespace datatools.datamaker
{
	public class SchemaElement
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public object MinValue { get; set; }
		public object MaxValue { get; set; }
		public ElementType Type { get; set; }

		public static bool IsValidElement(SchemaElement element)
		{
			if (element.Type.Equals(ElementType.RangeNumeric))
			{
				if (element.MinValue == null || element.MaxValue == null)
				{
					return false;
				}
				if (((int)element.MinValue) > ((int)element.MaxValue))
				{
					return false;
				}
			}
			else if (element.Type.Equals(ElementType.Reference) && !element.Value.GetType().Equals(typeof(DataSchemaReference)))
			{ 
				return false;
			}
			else
			{
				if(element.Value == null)
				{
					return false;
				}
			}

			return true;
		}
	}

	public enum ElementType
	{
		StaticValue,
		Choice,
		Optional,
		ElementList,
		RangeNumeric,
		Reference
	}
}
