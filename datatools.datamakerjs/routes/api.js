'use strict';
var express = require('express');
var router = express.Router();
const config = require('../serviceconfig');
const CommonSchema = require('../datamakerlibs/commonschema');
const Datamaker = require('../datamakerlibs/datamaker');

var provider = config.getSchemaProvider();
var decider = config.getDecider();

/* GET home page. */
router.get('/', function (req, res) {
    const urlBase = getBaseHostURL(req);

    res.send(`<html><body><a href="${urlBase}/api/namespaces">Schema Namespaces</a>`);
});

router.get('/namespaces', function (req, res) {

    const hostBase = getBaseHostURL(req);

    var namespaces = provider.Namespaces();
    var result = [];
    for (const namespace of namespaces) {
        result.push({name:namespace, schemadefs:`${hostBase}/api/schemadefs?namespace=${namespace}`});
    }
    res.send(result);
});
router.get('/schemadefs', function (req, res) {
    var hostBase = getBaseHostURL(req);
    var namespace = req.query.namespace;
    console.log(`schemadefs namespace:${namespace}`);
    var result = [];
    var schemaDefs = provider.SchemaDefs(namespace);
    for (const schemaDef of schemaDefs) {
        result.push({
            name: schemaDef,
            namespace: namespace ,
            definitionUrl: `${hostBase}/api/schemadef?namespace=${namespace}&schemaname=${schemaDef}`,
            getRandomExample: `${hostBase}/api/schemadef/getrandomexample?namespace=${namespace}&schemaname=${schemaDef}&count=1`
        });
    }
    res.send(result);
});

router.get('/schemadef', function (req, res) {
    var namespace = req.query.namespace;
    var schemaname = req.query.schemaname;
    console.log(`schemadef namespace:${namespace}, schemaname:${schemaname}`);
    var schemaDef = provider.getSchemaDef(namespace, schemaname);
    res.send(schemaDef);
});

router.post('/schemadef', function (req, res) {
    console.log('POST schemadef');
    console.log(` schemaDef:${JSON.parse(JSON.stringify(req.body))}`);
    provider.addSchemaDef(JSON.parse(JSON.stringify(req.body)));
    res.send({});
});

router.get('/schemadef/getrandomexample', function (req, res) {
    var namespace = req.query.namespace;
    var schemaname = req.query.schemaname;
    var count = 1;
    if (typeof req.query.count != 'undefined') {
        count = Number(req.query.count);
    }
    console.log(`schemadef namespace:${namespace}, schemaname:${schemaname}`);
    var schemaDef = provider.getSchemaDef(namespace, schemaname);

    var resultset = [];
    for (var i = 0; i < count; i++) {

        var example = Datamaker.getRandomExample(provider, decider, schemaDef);
        resultset.push(example);
    }
    res.send(resultset);
});

module.exports = router;

function getBaseHostURL(req) {
    const proxyHost = req.headers["x-forwarded-host"];
    const host = proxyHost ? proxyHost : req.headers.host;
    const hostBase = `${req.protocol}://${host}`;
    return hostBase;
}
