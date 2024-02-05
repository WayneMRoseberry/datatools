const fs = require('fs');
const { NULLVALUEERROR } = require("./commonschema");

class FileBasedSchemaProvider {

    schemaStore = {};
    _storeFilePath = 'schemaStore.json';

    constructor(storeFilePath) {
        if (storeFilePath != null) {
            this._storeFilePath = storeFilePath;
        }
        this.addSchemaDef = function (schemaDef) {
            if (schemaDef == null) {
                throw NULLVALUEERROR;
            }
            if (!Object.keys(this.schemaStore).includes(schemaDef.Namespace)) {
                this.schemaStore[schemaDef.Namespace] = {};
            }
            this.schemaStore[schemaDef.Namespace][schemaDef.SchemaName] = schemaDef;
            var schemaStoreContents = JSON.stringify(this.schemaStore);
            fs.writeFileSync(this._storeFilePath, schemaStoreContents);
        };
        this.getSchemaDef = function (namespace, schemaName) {
            if (Object.keys(this.schemaStore).includes(namespace)) {
                var nameslot = this.schemaStore[namespace];
                if (Object.keys(nameslot).includes(schemaName)) {
                    return this.schemaStore[namespace][schemaName]
                }
            }

            return null;
        };
        this.Namespaces = function () { return Object.keys(this.schemaStore); };

        this.SchemaDefs = function (namespace) {
            if (Object.keys(this.schemaStore).includes(namespace)) {
                return Object.keys(this.schemaStore[namespace]);
            }
            return [];
        };

        var schemaStoreFileContenst = "";
        if (fs.existsSync(this._storeFilePath)) {
            schemaStoreFileContenst = fs.readFileSync(this._storeFilePath);
            if (schemaStoreFileContenst.length != 0) {
                this.schemaStore = JSON.parse(schemaStoreFileContenst);
            }
        }
        else {
            fs.writeFileSync(this._storeFilePath, JSON.stringify(this.schemaStore));
        }

    }
}
exports.FileBasedSchemaProvider = FileBasedSchemaProvider;
