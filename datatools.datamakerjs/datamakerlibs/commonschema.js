'use strict';
const MAXALPHMUSTBEGREATERTHANOREQUALTOMINALPHA = "maxalpha must be equal to or larger than minalpha";
const MAXNUMERICMUSTBEGREATERTHANOREQUALTOMINNUMERIC = "maxnumeric must be equal to or larger than minnumeric";
const MUSTBESINGLECHARACTERERROR = "argument must be a single character";
const NULLVALUEERROR = "value must not be null";
const NONARRAYVALUEERROR = "value must be an array";

class ChoiceSchemaObject {
    constructor(choiceArray) {
        if(choiceArray == null) {
            throw NULLVALUEERROR;
        }
        if (!Array.isArray(choiceArray)) {
            throw NONARRAYVALUEERROR;
        }
        this.ChoiceArray = choiceArray;
    }
}

class OptionalSchemaObject {
    constructor(optionalValue) {
        if (optionalValue == null) {
            throw NULLVALUEERROR;
        }

        this.OptionalValue = optionalValue;
    }
}

class RangeAlphaSchemaObject {
    constructor(minAlpha, maxAlpha) {
        if (minAlpha == null || maxAlpha == null) {
            throw NULLVALUEERROR;
        }
        if (minAlpha > maxAlpha) {
            throw MAXALPHMUSTBEGREATERTHANOREQUALTOMINALPHA;
        }
        if (minAlpha.length != 1 || maxAlpha.length != 1) {
            throw MUSTBESINGLECHARACTERERROR;
        }
        this.MinAlpha = minAlpha;
        this.MaxAlpha = maxAlpha;
    }
}

class RangeNumericSchemaObject {
    constructor(minNumeric, maxNumeric) {
        if (minNumeric == null || maxNumeric == null) {
            throw NULLVALUEERROR;
        }
        if (minNumeric > maxNumeric) {
            throw MAXNUMERICMUSTBEGREATERTHANOREQUALTOMINNUMERIC;
        }
        this.MinNumeric = minNumeric;
        this.MaxNumeric = maxNumeric;
    }
}

class ReferenceSchemaObject {
    constructor(namespace, schemaname) {
        if (namespace == null || schemaname == null) {
            throw NULLVALUEERROR;
        }
        this.Namespace = namespace;
        this.SchemaName = schemaname;
    }
}

class SchemaDef {
    constructor(schemaName, rootSchemaObject) {
        if (schemaName == null || rootSchemaObject == null) {
            throw NULLVALUEERROR;
        }
        this.SchemaName = schemaName;
        this.RootSchemaObject = rootSchemaObject;
    }
}

class SequenceSchemaObject {
    constructor(sequenceArray) {
        if (sequenceArray == null) {
            throw NULLVALUEERROR;
        }
        if (!Array.isArray(sequenceArray)) {
            throw NONARRAYVALUEERROR;
        }
        this.SequenceArray = sequenceArray;
    }
}

class StaticSchemaObject {
    constructor(value) {
        this.StaticValue = value;
    }
}

function loadSchemaDef(schemaJson) {
    return JSON.parse(schemaJson);
}

function toJson(schemaDef) {
    return JSON.stringify(schemaDef);
}


module.exports = {
    ChoiceSchemaObject, OptionalSchemaObject, RangeAlphaSchemaObject, RangeNumericSchemaObject, ReferenceSchemaObject, SchemaDef, SequenceSchemaObject, StaticSchemaObject,
    loadSchemaDef, toJson,
    MAXALPHMUSTBEGREATERTHANOREQUALTOMINALPHA, MAXNUMERICMUSTBEGREATERTHANOREQUALTOMINNUMERIC, MUSTBESINGLECHARACTERERROR, NULLVALUEERROR, NONARRAYVALUEERROR
};