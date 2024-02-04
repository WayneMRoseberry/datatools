class FileBasedSchemaProvider {
    constructor() {
        this.getSchemaDef = function(namespace, schemaName) { throw "not implemented"; };
    }
}
exports.ProviderMock = FileBasedSchemaProvider;
