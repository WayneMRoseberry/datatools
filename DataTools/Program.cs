// See https://aka.ms/new-console-template for more information
using datatools.datamaker;
using datatools.datamaker.providers;
using DataTools;

var schemaFilePath = System.Configuration.ConfigurationManager.AppSettings["schemafile"];

FileBasedSchemaStore store = new FileBasedSchemaStore(schemaFilePath);

string NameSpace = "basedatatypes";

//SchemaHelpers.CreateBaseSchemaObjects(store, NameSpace);

var emailthing = store.GetSchemaElement(new DataSchemaReference() { Name = "EmailAddress", NameSpace = NameSpace });

var dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
dom = DataMaker.GetExample(emailthing, new RandomChooser(), store);
Console.WriteLine(dom);
