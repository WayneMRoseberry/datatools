﻿namespace datatools.datamaker
{
	public class DataMaker
	{

		/// <summary>
		/// Produces a valid random string where the requirements
		/// for the data string are described in the DataSchema object
		/// passed to the method.
		/// </summary>
		/// <param name="schema">Describes the requirements for a valid data object.</param>
		/// <returns>A random string that is valid according to the schema.</returns>
		/// <exception cref="ArgumentException">In the case where any element in schema is invalid.</exception>
		public static string GetExample(DataSchema schema, IChooser chooser, ISchemaStore schemaStore)
		{
			string result = string.Empty;

			foreach(SchemaElement element in schema.Elements)
			{
				if(!SchemaElement.IsValidElement(element))
				{
					throw new ArgumentException($"Element is invalid: {element.Name}");
				}
				result = EvaluateElement(result, element, chooser, schemaStore);
			}

			return result;
		}

		private static string EvaluateElement(string result, SchemaElement element, IChooser chooser, ISchemaStore schemaStore)
		{
			if(element.Type.Equals(ElementType.StaticValue))
			{
				result = result + GetElementValue(element, chooser, schemaStore);
			}
			if (element.Type.Equals(ElementType.Choice))
			{
				SchemaElement[] elements = element.ElementListValue;
				int length = elements.Length;
				int chosen = chooser.ChooseNumber(length);
				result = EvaluateElement(result, elements[chosen], chooser, schemaStore);
			}
			if (element.Type.Equals(ElementType.Optional))
			{
				int coinflip = chooser.ChooseNumber(2);
				if(coinflip > 0)
				{
					result = result + GetElementValue(element, chooser, schemaStore);
				}
			}
			if (element.Type.Equals(ElementType.ElementList))
			{
				SchemaElement[] elements = element.ElementListValue;
				foreach (SchemaElement e in elements)
				{
					result = EvaluateElement(result, e, chooser, schemaStore);
				}
			}
			if (element.Type.Equals(ElementType.RangeNumeric))
			{
				int min = (int) element.NumericMinValue;
				int max = (int) element.NumericMaxValue;
				int number = min + chooser.ChooseNumber((max - min) +1);
				result = result + number.ToString();
			}
			if (element.Type.Equals(ElementType.RangeAlpha))
			{
				int min = (char)element.AlphaMinValue;
				int max = (char)element.AlphaMaxValue;
				int number = min + chooser.ChooseNumber((max - min) + 1);
				result = ((char) number).ToString();
			}
			if (element.Type.Equals(ElementType.Reference))
			{
				DataSchema referencedSchema = schemaStore.GetSchemaElement((DataSchemaReference) element.RefValue);
				result = result + GetExample(referencedSchema, chooser, schemaStore);
			}
			return result;
		}

		private static string? GetElementValue(SchemaElement element, IChooser chooser, ISchemaStore schemaStore)
		{
			if(element.Type.Equals(ElementType.StaticValue))
			{
				return element.StringValue;
			}

			return EvaluateElement(string.Empty, (SchemaElement) element.ElementValue, chooser, schemaStore);
		}
	}

	public class InfinitelyRecursiveSchemaException : ArgumentException { }
}
