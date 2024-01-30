'use strict';
const CommonSchema = require('./commonschema');

function getRandomExample(schemaProvider, decider, schemaDef) {
    let root = schemaDef.RootSchemaObject;
    let randomValue = evaluateSchemaObject(schemaProvider, decider, root);

    return new CommonSchema.SchemaExample(schemaDef.SchemaName, randomValue);
}

module.exports = { getRandomExample};

function evaluateSchemaObject(schemaProvider, decider, schemaObject) {
    if (typeof schemaObject == 'undefined') {
        throw "cannot evaluate null schemaObject";
    }
    let randomValue = "";
    let typeName = "";
    if (typeof schemaObject.ObjectTypeName != 'undefined') {
        typeName = schemaObject.ObjectTypeName;
    }
    switch (typeName) {
        case "ChoiceSchemaObject":
            {
                let chosen = decider.chooseItem(schemaObject.ChoiceArray);
                randomValue = evaluateSchemaObject(schemaProvider, decider, chosen);
                break;
            }
        case "OptionalSchemaObject":
            {
                if (decider.optionChosen(schemaObject.OptionalValue)) {
                    randomValue = evaluateSchemaObject(schemaProvider, decider, schemaObject.OptionalValue);
                }
                break;
            }
        case "RangeAlphaSchemaObject":
            {
                return decider.chooseAlphaRange(schemaObject.MinAlpha, schemaObject.MaxAlpha);
                break;
            }
        case "RangeNumericSchemaObject":
            {
                return decider.chooseNumericRange(schemaObject.MinNumeric, schemaObject.MaxNumeric);
                break;
            }
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
