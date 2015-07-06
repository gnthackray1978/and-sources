var JSMaster, QryStrUtils, AncUtils,AncParishEditor;

$(document).ready(function () {
    var jsMaster = new JSMaster();

    jsMaster.generateHeader('#1', function () {
        
        var ancParishEditor = new AncParishEditor();
        ancParishEditor.init();

    });

});

var AncParishEditor = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.DEFAULT_GET_URL = '/ParishService/GetParish';
    this.DEFAULT_ADD_URL = '/ParishService/Add';
    this.DEFAULT_SEARCHPAGE_URL = '../HtmlPages/ParishSearch.html';

    this.postParams = {

        url: '',
        data: '',
        idparam: 'id',
        refreshmethod: this.load,
        refreshArgs: undefined,
        Context: this
    };
};

AncParishEditor.prototype = {

    init: function () {
     
        $('body').on("click","#save", $.proxy(function () { this.save(); return false; }, this));
        $('body').on("click", "#return", $.proxy(function () { this.saveReturn(); return false; }, this));

        this.load();
        return false;
    },

    load: function () {
        console.log('marriage editor load');
        var params = {};
        params[0] = this.qryStrUtils.getParameterByName('id', '');
        
        this.ancUtils.twaGetJSON(this.DEFAULT_GET_URL, params, $.proxy(this.processData, this));
    },

    processData: function (data) {

        console.log('marriage editor recieved data');


        this.qryStrUtils.updateQryPar('id', data.ParishId);

        $('#txtParishName').val(data.ParishName);
        $('#txtParishCounty').val(data.ParishCounty);
        $('#txtParishDeposited').val(data.ParishDeposited);
        $('#txtParentParish').val(data.ParishParent);      
        $('#txtParStartYear').val(data.ParishStartYear);
        $('#txtParEndYear').val(data.ParishEndYear);
        $('#txtXLocat').val(data.ParishLat);
        $('#txtYLocat').val(data.ParishLong);
        $('#txtNotes').val(data.ParishNote);
    },

    GetParishRecord: function (rowIdx) {
        
        var data = {};
    
        data.ParishId = this.qryStrUtils.getParameterByName('id', '');
        data.ParishName = $('#txtParishName').val();
        data.ParishCounty = $('#txtParishCounty').val();
        data.ParishDeposited = $('#txtParishDeposited').val();
        data.ParishParent = $('#txtParentParish').val();
        data.ParishStartYear = $('#txtParStartYear').val();
        data.ParishEndYear = $('#txtParEndYear').val();
        data.ParishLat = $('#txtXLocat').val();
        data.ParishLong = $('#txtYLocat').val();
        data.ParishNote = $('#txtNotes').val();
    
        return data;
    
    },
    
    save: function () {
        this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetParishRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    },

    saveReturn: function () {
        this.postParams.refreshmethod = function () { window.location = this.DEFAULT_SEARCHPAGE_URL + window.location.hash; };
        this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetParishRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    }

};




