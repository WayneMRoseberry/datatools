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
                for(const schemaItem of schemaObject.SequenceArray)
                {
                    randomValue = randomValue + evaluateSchemaObject(schemaProvider, decider, schemaItem);
                }
                break;
            }
        case "StaticSchemaObject":
            {
                randomValue = schemaObject.StaticValue;
                break;
            }
        case "ReferenceSchemaObject":
            {
                let schemaDef = schemaProvider.getSchemaDef(schemaObject.Namespace, schemaObject.SchemaName);
                randomValue = evaluateSchemaObject(schemaProvider, decider, schemaDef.RootSchemaObject);
                break;
            }
        default:
            {
                randomValue = schemaObject;
                break;
            }
    }
    return randomValue;
}
