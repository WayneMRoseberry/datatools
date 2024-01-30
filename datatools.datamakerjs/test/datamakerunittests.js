const CommonSchema = require('../datamakerlibs/commonschema');
const DataMaker = require('../datamakerlibs/datamaker'); 
describe('DataMaker test suite', function () {
    it("GetRandomExample StaticSchemaObject", function () {
        let staticObject = new CommonSchema.StaticSchemaObject("val1");
        let schemaDef =  new CommonSchema.SchemaDef("schemadef1", staticObject);
        expect(DataMaker.getRandomExample(null, null, schemaDef).SchemaName).toEqual("schemadef1");
        expect(DataMaker.getRandomExample(null, null, schemaDef).ExampleValue).toEqual("val1");
    });

    it("GetRandomExample ReferenceSchemaObject", function () {
        let refSchemaObject = new CommonSchema.ReferenceSchemaObject("namespace1", "refschemadef");
        let testSchemaDef = new CommonSchema.SchemaDef("schemadef1", refSchemaObject);

        let passedInNamespace = "";
        let passedInSchemaName = "";

        class providerMock {

            constructor() {
                this.getSchemaDef = function (namespace, schemaName) {
                    passedInNamespace = namespace;
                    passedInSchemaName = schemaName;

                    let newSchemaObject = new CommonSchema.StaticSchemaObject("refval1");
                    let refSchemaDef = new CommonSchema.SchemaDef("madeupname", newSchemaObject);
                    return refSchemaDef;
                };
            }

        };
        let mock = new providerMock();

        let example = DataMaker.getRandomExample(mock, null, testSchemaDef);
        expect(passedInNamespace).toEqual("namespace1");
        expect(passedInSchemaName).toEqual("refschemadef");
        expect(example.SchemaName).toEqual("schemadef1");
        expect(example.ExampleValue).toEqual("refval1");
    });

    it("GetRandomExample SequenceSchemaObject", function () {
        let sequenceSchemaObject = new CommonSchema.SequenceSchemaObject(["val1","val2"]);
        let schemaDef = new CommonSchema.SchemaDef("schemadef1", sequenceSchemaObject);
        let example = DataMaker.getRandomExample(null, null, schemaDef);
        expect(example.SchemaName).toEqual("schemadef1");
        expect(example.ExampleValue).toEqual("val1val2");
    });

    it("GetRandomExample TwoDeepSequenceSchemaObject", function () {
        let sequenceSchemaObject = new CommonSchema.SequenceSchemaObject(["val1", "val2"]);
        let sequenceSchemaObject2 = new CommonSchema.SequenceSchemaObject([sequenceSchemaObject, "val3"]);
        let schemaDef = new CommonSchema.SchemaDef("schemadef1", sequenceSchemaObject2);
        let example = DataMaker.getRandomExample(null, null, schemaDef);
        expect(example.SchemaName).toEqual("schemadef1");
        expect(example.ExampleValue).toEqual("val1val2val3");
    });

});
