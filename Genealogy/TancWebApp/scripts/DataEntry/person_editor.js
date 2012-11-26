
$(document).ready(function () {
    var jsMaster = new JSMaster();
    var ancPersonEditor = new AncPersonEditor();

    jsMaster.generateHeader('#1', function () {
        ancPersonEditor.init();

    });

});





var AncPersonEditor = function () {
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
}

AncPersonEditor.prototype = {

    init: function () {


        var panels = new Panels();

        $("#save").live("click", $.proxy(function () { this.save(); return false; }, this));
        $("#return").live("click", $.proxy(function () { this.saveReturn(); return false; }, this));
        $("#main").live("click", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $("#more").live("click", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
        this.load();
        return false;
    },

    load: function () {
        var params = {};
        params[0] = this.qryStrUtils.getParameterByName('id', '');

        this.ancUtils.twaGetJSON("/GetPerson/Select", params, $.proxy(this.processData, this));
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
        this.postParams.url = '/Person/Add';
        this.postParams.data = this.GetPersonRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    },

    saveReturn: function () {             
        this.postParams.refreshmethod = function () { window.location = '../HtmlPages/PersonSearch.html' + window.location.hash; };
        this.postParams.url = '/Person/Add';
        this.postParams.data = this.GetPersonRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    }
 
}



 
 