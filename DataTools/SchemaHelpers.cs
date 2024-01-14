using datatools.datamaker;
using datatools.datamaker.providers;

namespace DataTools
{
	public class SchemaHelpers
	{
		static void CreateAlphas(FileBasedSchemaStore store, string NameSpace)
		{
			SchemaElement alphaslower = new SchemaElement() { Name = "namealphaslower", Type = ElementType.RangeAlpha, AlphaMinValue = 'a', AlphaMaxValue = 'z' };
			SchemaElement alphasupperr = new SchemaElement() { Name = "namealphasupper", Type = ElementType.RangeAlpha, AlphaMinValue = 'A', AlphaMaxValue = 'Z' };
			DataSchema alphas = new DataSchema() { SchemaName = "Alphas" };
			alphas.AddElement(new SchemaElement() { Name = "alpharanges", Type = ElementType.Choice, ElementListValue = new SchemaElement[] { alphaslower, alphasupperr } });

			store.AddDataSchema(NameSpace, alphas);
		}

		static void CreateNumerics(FileBasedSchemaStore store, string NameSpace)
		{
			SchemaElement numeric = new SchemaElement() { Name = "namealphasupper", Type = ElementType.RangeAlpha, AlphaMinValue = '0', AlphaMaxValue = '9' };
			DataSchema numerics = new DataSchema() { SchemaName = "Numerics" };
			numerics.AddElement(numeric);

			store.AddDataSchema(NameSpace, numerics);
		}

		static void CreateUserName(FileBasedSchemaStore store, string NameSpace)
		{
			SchemaElement namealphanumerics = new SchemaElement()
			{
				Name = "namealphanumerics",
				Type = ElementType.ElementList,
				ElementListValue = new SchemaElement[]
				{
		new SchemaElement() { Name = "alphaprefix", Type = ElementType.Reference, RefValue = new DataSchemaReference() { Name = "Alphas", NameSpace = NameSpace } },
		new SchemaElement() { Name = "alphanumcontinuation", Type=ElementType.Optional, ElementValue = new SchemaElement()
		{
			Name="choosealphaornumeric", Type=ElementType.Choice, ElementListValue = new SchemaElement[]
			{
				new SchemaElement() { Name="alphachoice", Type=ElementType.Reference, RefValue= new DataSchemaReference(){ Name="Alphas", NameSpace=NameSpace } },
				new SchemaElement() { Name="numericchoice", Type=ElementType.Reference, RefValue= new DataSchemaReference(){ Name="Numerics", NameSpace=NameSpace } },
			}
		} }
				}
			};

			DataSchema username = new DataSchema() { SchemaName = "UserName" };
			username.AddElement(namealphanumerics);
			username.AddElement(new SchemaElement()
			{
				Name = "NameRecursionChoice",
				Type = ElementType.Optional,
				ElementValue = new SchemaElement()
				{
					Name = "UserNameRef",
					Type = ElementType.Reference,
					RefValue = new DataSchemaReference()
					{ Name = "UserName", NameSpace = NameSpace }
				}
			});

			store.AddDataSchema(NameSpace, username);
		}

		static void CreateDomainName(FileBasedSchemaStore store, string NameSpace)
		{
			DataSchema domainname = new DataSchema() { SchemaName = "DomainName" };
			domainname.AddElement(new SchemaElement() { Name = "DomainNamePortion", Type = ElementType.Reference, RefValue = new DataSchemaReference() { Name = "UserName", NameSpace = NameSpace } });
			domainname.AddElement(new SchemaElement()
			{
				Name = "DomainNameRecursionChoice",
				Type = ElementType.Optional,
				ElementValue = new SchemaElement()
				{
					Name = "DomainDotRecursion",
					Type = ElementType.ElementList,
					ElementListValue = new SchemaElement[]
						{
				new SchemaElement() {Name="dot", Type=ElementType.StaticValue, StringValue="."},
				new SchemaElement() {Name="domainrecursion", Type=ElementType.Reference, RefValue= new DataSchemaReference() {Name="DomainName", NameSpace=NameSpace}}
						}

				}

			});

			store.AddDataSchema(NameSpace, domainname);
		}

		static void CreateEmailName(FileBasedSchemaStore store, string NameSpace)
		{
			DataSchema emailschema = new DataSchema() { SchemaName = "EmailAddress" };
			emailschema.AddElement(new SchemaElement() { Name = "UserName", Type = ElementType.Reference, RefValue = new DataSchemaReference() { Name = "UserName", NameSpace = NameSpace } });
			emailschema.AddElement(new SchemaElement() { Name = "AtSeparator", Type = ElementType.StaticValue, StringValue = "@" });
			emailschema.AddElement(new SchemaElement() { Name = "DomainAddress", Type = ElementType.Reference, RefValue = new DataSchemaReference() { Name = "DomainName", NameSpace = NameSpace } });
			emailschema.AddElement(new SchemaElement()
			{
				Name = "TopLevelDomain",
				Type = ElementType.ElementList,
				ElementListValue = new SchemaElement[]
				{
		new SchemaElement() {Name="Dot", Type=ElementType.StaticValue, StringValue="." },
		new SchemaElement() {Name="KnownTopDomains", Type=ElementType.Choice, ElementListValue=new SchemaElement[]
		{
			new SchemaElement() { Name="com", Type=ElementType.StaticValue, StringValue="com"},
			new SchemaElement() { Name="org", Type=ElementType.StaticValue, StringValue="org"},
			new SchemaElement() { Name="gov", Type=ElementType.StaticValue, StringValue="gov"},
			new SchemaElement() { Name="edu", Type=ElementType.StaticValue, StringValue="edu"}
		} }
				}
			});


			store.AddDataSchema(NameSpace, emailschema);
		}

		public static void CreateBaseSchemaObjects(FileBasedSchemaStore store, string NameSpace)
		{
			CreateAlphas(store, NameSpace);

			CreateNumerics(store, NameSpace);

			CreateUserName(store, NameSpace);

			CreateDomainName(store, NameSpace);

			CreateEmailName(store, NameSpace);
		}
	}
}
