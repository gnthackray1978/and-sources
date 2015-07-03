var JSMaster, QryStrUtils, AncUtils, AncSelectorSources, Panels;

$(document).ready(function () {
    var jsMaster = new JSMaster();


    jsMaster.generateHeader('#1', function () {
        
        var ancPersonEditor = new AncPersonEditor();
        ancPersonEditor.init();

    });

});

var AncPersonEditor = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.DEFAULT_GET_URL = '/PersonService/Person';
    this.DEFAULT_ADD_URL = '/PersonService/Add';
    this.DEFAULT_SEARCHPAGE_URL = '../HtmlPages/PersonSearch.html';

    this.postParams = {

        url: '',
        data: '',
        idparam: 'id',
        refreshmethod: this.load,
        refreshArgs: undefined,
        Context: this
    };
};

AncPersonEditor.prototype = {

    init: function () {


        var panels = new Panels();

        $('body').on("click", "#save", $.proxy(function () { this.save(); return false; }, this));
        $('body').on("click", "#return", $.proxy(function () { this.saveReturn(); return false; }, this));
        $('body').on("click", "#main", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $('body').on("click", "#more", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));

    

        this.load();
        return false;
    },

    load: function () {
        var params = {};
        params[0] = this.qryStrUtils.getParameterByName('id', '');

        this.ancUtils.twaGetJSON(this.DEFAULT_GET_URL, params, $.proxy(this.processData, this));
    },


    processData: function (data) {
        $('#txtNam').val(data.ChristianName);
        $('#txtSur').val(data.Surname);
        $('#txtFatNam').val(data.FatherChristianName);
        $('#txtFatSur').val(data.FatherSurname);
        $('#txtMotNam').val(data.MotherChristianName);
        $('#txtMotSur').val(data.MotherSurname);
        $('#txtBirDat').val(data.Birth);
        $('#txtBapDat').val(data.Baptism);
        $('#txtBirPar').val(data.BirthLocation);
        $('#txtBirCny').val(data.BirthCounty);
        $('#txtSrc').val(data.SourceDescription);
        $('#txtRef').val(data.ReferenceLocation);
        $('#txtRefDat').val(data.ReferenceDate);
        $('#txtSNam').val(data.SpouseChristianName);
        $('#txtSSur').val(data.SpouseSurname);
        $('#txtFOcc').val(data.FatherOccupation);
        $('#txtOcc').val(data.Occupation);
        $('#txtDetDat').val(data.Death);
        $('#txtDetPar').val(data.DeathLocation);
        $('#txtDetCny').val(data.DeathCounty);
        $('#txtNotRdOnly').val(data.Notes);

        this.qryStrUtils.updateQryPar('source_ids', data.Sources);



        var ancSelectorSources = new AncSelectorSources();

        ancSelectorSources.getSources('#sourceselector');

    },
    GetPersonRecord: function () {
        var data = {};
        data.personId = this.qryStrUtils.getParameterByName('id', '');
        data.sources = this.qryStrUtils.getParameterByName('source_ids', '');
        data.christianName = $('#txtNam').val();
        data.surname = $('#txtSur').val();
        data.fatherchristianname = $('#txtFatNam').val();
        data.fathersurname = $('#txtFatSur').val();
        data.motherchristianname = $('#txtMotNam').val();
        data.mothersurname = $('#txtMotSur').val();
        data.datebirthstr = $('#txtBirDat').val();
        data.datebapstr = $('#txtBapDat').val();
        data.birthloc = $('#txtBirPar').val();
        data.birthcounty = $('#txtBirCny').val();
        data.source = $('#txtSrc').val();
        data.refloc = $('#txtRef').val();
        data.refdate = $('#txtRefDat').val();
        data.spousechristianname = $('#txtSNam').val();
        data.spousesurname = $('#txtSSur').val();
        data.fatheroccupation = $('#txtFOcc').val();
        data.occupation = $('#txtOcc').val();
        data.datedeath = $('#txtDetDat').val();
        data.deathloc = $('#txtDetPar').val();
        data.deathcounty = $('#txtDetCny').val();
        data.notes = $('#txtNotRdOnly').val();

        return data;


    },
    save: function () {
        this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetPersonRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    },

    saveReturn: function () {
        this.postParams.refreshmethod = function () { 
        window.location = this.DEFAULT_SEARCHPAGE_URL + window.location.hash; };
        this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetPersonRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    }
 
}



 
 