﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			if(element.Type.Equals(ElementType.RangeNumeric))
			{
				if (((int)element.MinValue) > ((int)element.MaxValue))
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
		RangeNumeric
	}
}