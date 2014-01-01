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

        this.ancUtils.twaGetJSON("/SourceTypes/Id", params, $.proxy(this.processData, this));
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
        this.postParams.url = '/SourceTypes/Add';
        this.postParams.data = this.GetSourceTypeRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    },

    saveReturn: function () {
        this.postParams.refreshmethod = function () { window.location = '../HtmlPages/SourceTypesSearch.html' + window.location.hash; };
        this.postParams.url = '/SourceTypes/Add';
        this.postParams.data = this.GetSourceTypeRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    }

};
