const CommonSchema = require('../datamakerlibs/commonschema');
const DataMaker = require('../datamakerlibs/datamaker'); 
describe('DataMaker test suite', function () {

    it("GetRandomExample ChoiceSchemaObject", function () {
        let choiceSchemaObject = new CommonSchema.ChoiceSchemaObject(["val1", "val2"]);
        let schemaDef = new CommonSchema.SchemaDef("schemadef1", choiceSchemaObject);

        let passedInArray = null;

        class deciderMock {
            constructor() {
                this.chooseItem = function (itemArray) {
                    passedInArray = itemArray;
                    return itemArray[0];
                }
            }
        }

        let mock = new deciderMock();

        let example = DataMaker.getRandomExample(null, mock, schemaDef);
        expect(passedInArray.length).toEqual(2);
        expect(passedInArray[0]).toEqual("val1");
        expect(passedInArray[1]).toEqual("val2");
        expect(example.SchemaName).toEqual("schemadef1");
        expect(example.ExampleValue).toEqual("val1");

        mock.chooseItem = function (itemArray) {
            return itemArray[1];
        };
        example = DataMaker.getRandomExample(null, mock, schemaDef);
        expect(example.ExampleValue).toEqual("val2");

    });

    it("GetRandomExample OptionalSchemaObject", function () {
        let optionalSchemaObject = new CommonSchema.OptionalSchemaObject("val1");
        let schemaDef = new CommonSchema.SchemaDef("schemadef1", optionalSchemaObject);

        let passedInObject = null;

        class deciderMock {
            constructor() {
                this.optionChosen = function (schemaObject) {
                    passedInObject = schemaObject;
                    return true;
                }
            }
        }

        let mock = new deciderMock();

        let example = DataMaker.getRandomExample(null, mock, schemaDef);
        expect(passedInObject).toEqual("val1");
        expect(example.SchemaName).toEqual("schemadef1");
        expect(example.ExampleValue).toEqual("val1");

        mock.optionChosen = function (schemaObject) {
            return false;
        };
        example = DataMaker.getRandomExample(null, mock, schemaDef);
        expect(example.ExampleValue).toEqual("");

    });

    it("GetRandomExample RangeAlphaSchemaObject", function () {
        let rangeAlphaSchemaObject = new CommonSchema.RangeAlphaSchemaObject("a","z");
        let schemaDef = new CommonSchema.SchemaDef("schemadef1", rangeAlphaSchemaObject);

        let passedInMin = null;
        let passedInMax = null;

        class deciderMock {
            constructor() {
                this.chooseAlphaRange = function (minAlpha,maxAlpha) {
                    passedInMin = minAlpha;
                    passedInMax = maxAlpha;
                    return "b";
                }
            }
        }

        let mock = new deciderMock();

        let example = DataMaker.getRandomExample(null, mock, schemaDef);
        expect(passedInMin).toEqual("a");
        expect(passedInMax).toEqual("z");
        expect(example.SchemaName).toEqual("schemadef1");
        expect(example.ExampleValue).toEqual("b");
    });

    it("GetRandomExample RangeNumericSchemaObject", function () {
        let rangeNumericSchemaObject = new CommonSchema.RangeNumericSchemaObject(1, 100);
        let schemaDef = new CommonSchema.SchemaDef("schemadef1", rangeNumericSchemaObject);

        let passedInMin = -1;
        let passedInMax = -1;

        class deciderMock {
            constructor() {
                this.chooseNumericRange = function (minNumeric, maxNumeric) {
                    passedInMin = minNumeric;
                    passedInMax = maxNumeric;
                    return 12;
                }
            }
        }

        let mock = new deciderMock();

        let example = DataMaker.getRandomExample(null, mock, schemaDef);
        expect(passedInMin).toEqual(1);
        expect(passedInMax).toEqual(100);
        expect(example.SchemaName).toEqual("schemadef1");
        expect(example.ExampleValue).toEqual(12);
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

    it("GetRandomExample StaticSchemaObject", function () {
        let staticObject = new CommonSchema.StaticSchemaObject("val1");
        let schemaDef =  new CommonSchema.SchemaDef("schemadef1", staticObject);
        expect(DataMaker.getRandomExample(null, null, schemaDef).SchemaName).toEqual("schemadef1");
        expect(DataMaker.getRandomExample(null, null, schemaDef).ExampleValue).toEqual("val1");
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
