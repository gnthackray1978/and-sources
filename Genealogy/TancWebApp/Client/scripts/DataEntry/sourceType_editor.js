var JSMaster, QryStrUtils, AncUtils;

$(document).ready(function () {
    var jsMaster = new JSMaster();
 
    jsMaster.generateHeader('#1', function () {        
        var ancSourceTypeEditor = new AncSourceTypeEditor();
        ancSourceTypeEditor.init();

    });

});



var AncSourceTypeEditor = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();

    this.DEFAULT_GET_URL = '/SourceTypes/Id';
    this.DEFAULT_ADD_URL = '/SourceTypes/Add';
    this.DEFAULT_SEARCHPAGE_URL = '../HtmlPages/SourceTypesSearch.html';

    this.postParams = {
        url: '',
        data: '',
        idparam: 'id',
        refreshmethod: this.load,
        refreshArgs: undefined,
        Context: this
    };
};


AncSourceTypeEditor.prototype = {

    init: function () {                
        $('body').on("click","#save", $.proxy(function () { this.save(); return false; }, this));
        $('body').on("click","#return", $.proxy(function () { this.saveReturn(); return false; }, this));

        this.load();
        return false;
    },

    load: function () {      
        var params = {};
        params[0] = this.qryStrUtils.getParameterByName('id', '');

        this.ancUtils.twaGetJSON(this.DEFAULT_GET_URL, params, $.proxy(this.processData, this));
    },
    processData: function (data) {
        $('#txtOrder').val(data.Order);
        $('#txtDescription').val(data.Description);
    },

    GetSourceTypeRecord: function () {

        var data = {};
    
        data.TypeId = this.qryStrUtils.getParameterByName('id', '');
        data.Description = $('#txtDescription').val();
        data.Order = $('#txtOrder').val();
        
        return data;
    
    },
    save: function () {
        this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetSourceTypeRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    },

    saveReturn: function () {
        this.postParams.refreshmethod = function () { window.location = this.DEFAULT_SEARCHPAGE_URL + window.location.hash; };
        this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetSourceTypeRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    }

};
