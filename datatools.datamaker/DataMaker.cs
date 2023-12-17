using System.Security.Authentication.ExtendedProtection;

namespace datatools.datamaker
{
	public class DataMaker
	{
		static Random rand = new Random();

		/// <summary>
		/// Produces a valid random string where the requirements
		/// for the data string are described in the DataSchema object
		/// passed to the method.
		/// </summary>
		/// <param name="schema">Describes the requirements for a valid data object.</param>
		/// <returns>A random string that is valid according to the schema.</returns>
		/// <exception cref="ArgumentException">In the case where any element in schema is invalid.</exception>
		public static string GetRandomExample(DataSchema schema)
		{
			string result = string.Empty;

			foreach(SchemaElement element in schema.Elements)
			{
				if(!SchemaElement.IsValidElement(element))
				{
					throw new ArgumentException($"Element is invalid: {element.Name}");
				}
				result = EvaluateElement(result, element);
			}

			return result;
		}

		private static string EvaluateElement(string result, SchemaElement element)
		{
			if(element.Type.Equals(ElementType.StaticValue))
			{
				result = result + GetElementValue(element);
			}
			if (element.Type.Equals(ElementType.Choice))
			{
				SchemaElement[] elements = (SchemaElement[])element.Value;
				int chosen = rand.Next(elements.Length);
				result = EvaluateElement(result, elements[chosen]);
			}
			if (element.Type.Equals(ElementType.Optional))
			{
				int coinflip = rand.Next(2);
				if(coinflip > 0)
				{
					result = result + GetElementValue(element);
				}
			}
			if (element.Type.Equals(ElementType.ElementList))
			{
				SchemaElement[] elements = (SchemaElement[])element.Value;
				foreach (SchemaElement e in elements)
				{
					result = EvaluateElement(result, e);
				}
			}
			if (element.Type.Equals(ElementType.RangeNumeric))
			{
				int min = (int) element.MinValue;
				int max = (int) element.MaxValue;
				int number = min + rand.Next((max - min) +1);
				result = result + number.ToString();
			}
			return result;
		}

		private static string? GetElementValue(SchemaElement element)
		{
			if(element.Value.GetType().Equals(typeof(string)))
			{
				return element.Value.ToString();
			}

			if (element.Value.GetType().Equals(typeof(SchemaElement[])))
			{
				SchemaElement[] elements = (SchemaElement[])element.Value;
				string result = string.Empty;
				foreach(SchemaElement e in elements)
				{
					result = EvaluateElement(result, e);
				}
				return result;
			}

			return EvaluateElement(string.Empty, (SchemaElement) element.Value);
		}
	}
}
