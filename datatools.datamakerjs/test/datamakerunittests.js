const CommonSchema = require('../datamakerlibs/commonschema');
const DataMaker = require('../datamakerlibs/datamaker'); 
describe('DataMaker test suite', function () {
    it("GetRandomExample StaticSchemaObject", function () {
        let staticObject = new CommonSchema.StaticSchemaObject("val1");
        let schemaDef =  new CommonSchema.SchemaDef("schemadef1", staticObject);
        expect(DataMaker.getRandomExample(null, null, schemaDef).SchemaName).toEqual("schemadef1");
        expect(DataMaker.getRandomExample(null, null, schemaDef).ExampleValue).toEqual("val1");
    });

    it("GetRandomExample SequenceSchemaObject", function () {
        let sequenceSchemaObject = new CommonSchema.SequenceSchemaObject(["val1","val2"]);
        let schemaDef = new CommonSchema.SchemaDef("schemadef1", sequenceSchemaObject);
        let example = DataMaker.getRandomExample(null, null, schemaDef);
        expect(example.SchemaName).toEqual("schemadef1");
        expect(example.ExampleValue).toEqual("val1val2");
    });

});
