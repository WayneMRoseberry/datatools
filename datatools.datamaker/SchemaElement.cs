using System.Text.Json;

namespace datatools.datamaker
{
	public class SchemaElement
	{
		public string Name { get; set; }
		public SchemaElement ElementValue { get; set; }
		public string StringValue { get; set; }
		public char AlphaMinValue { get; set; }
		public char AlphaMaxValue { get; set; }
		public int NumericMinValue { get; set; }
		public int NumericMaxValue { get; set; }
		public DataSchemaReference RefValue { get; set; }
		public SchemaElement[] ElementListValue { get; set; }

		public ElementType Type { get; set; }

		public static bool IsValidElement(SchemaElement element)
		{
			if (element.Type.Equals(ElementType.RangeNumeric))
			{
				if (element.NumericMinValue == null || element.NumericMaxValue == null)
				{
					return false;
				}
				if (((int)element.NumericMinValue) > ((int)element.NumericMaxValue))
				{
					return false;
				}
			}
			else if (element.Type.Equals(ElementType.ElementList) || element.Type.Equals(ElementType.Choice))
			{
				if(element.ElementListValue== null)
				{
					return true;
				}
			}
			else if (element.Type.Equals(ElementType.RangeAlpha))
			{
				if (element.AlphaMinValue == null || element.AlphaMaxValue == null)
				{
					return false;
				}
				if (((char)element.AlphaMinValue) > ((char)element.AlphaMaxValue))
				{
					return false;
				}
			}
			else if (element.Type.Equals(ElementType.Reference))
			{
				if(element.RefValue==null)
				{
					return false;
				}
			}
			else if(element.Type.Equals(ElementType.StaticValue))
			{
				if(element.StringValue==null)
				{
					return false;
				}
			}
			else if(element.Type.Equals(ElementType.Optional))
			{
				if(element.StringValue==null && element.ElementValue==null)
				{
					return false;
				}
			}
			else
			{
				if(element.ElementValue == null)
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
		Reference,
		RangeAlpha
	}

	public class InvalidSchemaElementException : Exception
	{
		public string ElementName { get; private set; }

		public InvalidSchemaElementException(string elementName)
		{
			ElementName = elementName;
		}
	}
}
