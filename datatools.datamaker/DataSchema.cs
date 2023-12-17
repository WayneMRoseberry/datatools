using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datatools.datamaker
{
	public class DataSchema
	{
		private List<SchemaElement> elements = new List<SchemaElement>();
		internal IList<SchemaElement> Elements { get { return elements; } }

		public void AddElement(SchemaElement element) 
		{
			this.elements.Add(element);
		}
	}
}
