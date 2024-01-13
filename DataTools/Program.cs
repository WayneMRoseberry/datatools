// See https://aka.ms/new-console-template for more information
using datatools.datamaker;
using datatools.datamaker.providers;
using System.Data;

Console.WriteLine("Hello, World!");

FileBasedSchemaStore store = new FileBasedSchemaStore("datamakerschema.txt");

string NameSpace = "datamakernamespace";

SchemaElement alphaslower = new SchemaElement() { Name = "namealphaslower", Type = ElementType.RangeAlpha, MinValue = 'a', MaxValue = 'z' };
SchemaElement alphasupperr = new SchemaElement() { Name = "namealphasupper", Type = ElementType.RangeAlpha, MinValue = 'A', MaxValue = 'Z' };
DataSchema alphas = new DataSchema() { SchemaName="Alphas"};
alphas.AddElement(new SchemaElement() { Name = "alpharanges", Type = ElementType.Choice, Value = new SchemaElement[] { alphaslower, alphasupperr } });

store.AddDataSchema(NameSpace, alphas);

SchemaElement numeric = new SchemaElement() { Name = "namealphasupper", Type = ElementType.RangeAlpha, MinValue = '0', MaxValue = '9' };
DataSchema numerics = new DataSchema() { SchemaName = "Numerics" };
numerics.AddElement(numeric);

store.AddDataSchema(NameSpace, numerics);

SchemaElement namealphanumerics = new SchemaElement()
{
	Name = "namealphanumerics",
	Type = ElementType.ElementList,
	Value = new SchemaElement[]
	{
		new SchemaElement() { Name = "alphaprefix", Type = ElementType.Reference, Value = new DataSchemaReference() { Name = "Alphas", NameSpace = NameSpace } },
		new SchemaElement() { Name = "alphanumcontinuation", Type=ElementType.Optional, Value = new SchemaElement()
		{
			Name="choosealphaornumeric", Type=ElementType.Choice, Value = new SchemaElement[]
			{
				new SchemaElement() { Name="alphachoice", Type=ElementType.Reference, Value= new DataSchemaReference(){ Name="Alphas", NameSpace=NameSpace } },
				new SchemaElement() { Name="numericchoice", Type=ElementType.Reference, Value= new DataSchemaReference(){ Name="Numerics", NameSpace=NameSpace } },
			}
		} }
	}
};

DataSchema username = new DataSchema() { SchemaName = "UserName"};
username.AddElement(namealphanumerics);
username.AddElement(new SchemaElement() 
	{ Name = "NameRecursionChoice", Type = ElementType.Optional, Value = new SchemaElement() 
		{ Name="UserNameRef", Type=ElementType.Reference, Value= new DataSchemaReference() 
			{ Name = "UserName", NameSpace = NameSpace }}  });

store.AddDataSchema(NameSpace, username);

DataSchema domainname = new DataSchema() { SchemaName = "DomainName" };
domainname.AddElement(new SchemaElement() {Name="DomainNamePortion", Type=ElementType.Reference, Value= new DataSchemaReference() { Name = "UserName", NameSpace = NameSpace } });
domainname.AddElement(new SchemaElement()
	{ Name="DomainNameRecursionChoice", Type=ElementType.Optional, Value=new SchemaElement()
		{ Name="DomainDotRecursion", Type=ElementType.ElementList, Value = new SchemaElement[]
			{
				new SchemaElement() {Name="dot", Type=ElementType.StaticValue, Value="."},
				new SchemaElement() {Name="domainrecursion", Type=ElementType.Reference, Value= new DataSchemaReference() {Name="DomainName", NameSpace=NameSpace}}
			}

		}

	});

store.AddDataSchema(NameSpace, domainname);

DataSchema emailschema = new DataSchema() { SchemaName = "EmailAddress" };
emailschema.AddElement(new SchemaElement() { Name="UserName", Type=ElementType.Reference, Value = new DataSchemaReference() {Name="UserName", NameSpace=NameSpace } });
emailschema.AddElement(new SchemaElement() { Name = "AtSeparator", Type = ElementType.StaticValue, Value = "@" });
emailschema.AddElement(new SchemaElement() { Name = "DomainAddress", Type = ElementType.Reference, Value = new DataSchemaReference() { Name = "DomainName", NameSpace = NameSpace } });
emailschema.AddElement(new SchemaElement() { Name = "TopLevelDomain", Type = ElementType.ElementList, Value = new SchemaElement[] 
	{
		new SchemaElement() {Name="Dot", Type=ElementType.StaticValue, Value="." },
		new SchemaElement() {Name="KnownTopDomains", Type=ElementType.Choice, Value=new SchemaElement[] 
		{ 
			new SchemaElement() { Name="com", Type=ElementType.StaticValue, Value="com"},
			new SchemaElement() { Name="org", Type=ElementType.StaticValue, Value="org"},
			new SchemaElement() { Name="gov", Type=ElementType.StaticValue, Value="gov"},
			new SchemaElement() { Name="edu", Type=ElementType.StaticValue, Value="edu"}
		} }
	} });


store.AddDataSchema(NameSpace, emailschema);

var dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailschema, new RandomChooser(), store);
Console.WriteLine(dom);



