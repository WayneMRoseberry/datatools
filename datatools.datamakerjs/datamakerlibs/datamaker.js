'use strict';
const CommonSchema = require('./commonschema');

function getRandomExample(schemaProvider, decider, schemaDef) {
    let root = schemaDef.RootSchemaObject;
    let randomValue = evaluateSchemaObject(schemaProvider, decider, root);

    return new CommonSchema.SchemaExample(schemaDef.SchemaName, randomValue);
}

module.exports = { getRandomExample};

function evaluateSchemaObject(schemaProvider, decider, schemaObject) {
    let randomValue = "";

    switch (schemaObject.ObjectTypeName) {
        case "SequenceSchemaObject":
            {
                randomValue = schemaObject.SequenceArray.join("");
                break;
            }
        case "StaticSchemaObject":
            {
                randomValue = schemaObject.StaticValue;
                break;
            }
    }
    return randomValue;
}
