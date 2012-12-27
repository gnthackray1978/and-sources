
var JSMaster, QryStrUtils, AncUtils,AncSelectorParishs,AncSelectorSourceTypes,Panels;

$(document).ready(function () {
    var jsMaster = new JSMaster();

    jsMaster.generateHeader('#1', function () {
        var ancSourceEditor = new AncSourceEditor();
            ancSourceEditor.init();

    });

});

var AncSourceEditor = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.ancSelectorParishs = new AncSelectorParishs();
    this.ancSelectorSourceTypes = new AncSelectorSourceTypes();

    this.postParams = {

        url: '',
        data: '',
        idparam: 'id',
        refreshmethod: this.load,
        refreshArgs: undefined,
        Context: this
    };
}

AncSourceEditor.prototype = {

   init :function() {

       var panels = new Panels();

       $("#save").live("click", $.proxy(function () { this.save(); return false; }, this));
       $("#return").live("click", $.proxy(function () { this.saveReturn(); return false; }, this));
       $("#main").live("click", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
       $("#notes").live("click", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
       $("#files").live("click", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));

       this.load();

       return false;

   },

   load: function(){
       var params = {};
       params[0] = this.qryStrUtils.getParameterByName('id', '');

       this.ancUtils.twaGetJSON("/Sources/GetSource", params, $.proxy(this.processData, this));
   },

   processData: function(data) {

    // public string AddPerson(string personId, string birthparishId,string deathparishId, string referenceparishId, string ismale, string years, string months, string weeks, string days)

        $('#txtSourceRef').val(data.SourceRef);
        $('#txtSourceDescription').val(data.SourceDesc);

        $('#txtLowerDate').val(data.SourceDateStr);
        $('#txtUpperDate').val(data.SourceDateStrTo);
        $('#txtOriginalLoc').val(data.OriginalLocation);


        $('#txtNotRdOnly').val(data.SourceNotes);

        if (data.IsCopyHeld === false) {
            $('#chkIsCopyHeld').prop('checked', false);
        }
        else {
            $('#chkIsCopyHeld').prop('checked', true);
        }


        if (data.IsViewed === false) {
            $('#chkIsViewed').prop('checked', false);
        }
        else {
            $('#chkIsViewed').prop('checked', true);
        }


        if (data.IsThackrayFound === false) {
            $('#chkIsThackrayFound').prop('checked', false);
        }
        else {
            $('#chkIsThackrayFound').prop('checked', true);
        }

        this.qryStrUtils.updateQryPar('stypes', data.SourceTypes);

        this.qryStrUtils.updateQryPar('pids', data.Parishs);

        this.qryStrUtils.updateQryPar('fids', data.FileIds);



        this.ancSelectorParishs.init('#parishselector');

        this.ancSelectorSourceTypes.getSourceTypes('#sourcetypeselector');

    },
   GetSourceRecord:function (rowIdx) {
        var data = {};
        data.sourceId = this.qryStrUtils.getParameterByName('id','');

        //data.sources = getParameterByName('source_ids');

        data.sourceRef = $('#txtSourceRef').val();
        data.sourceDesc = $('#txtSourceDescription').val();
        data.sourceDateStr = $('#txtLowerDate').val();
        data.sourceDateStrTo = $('#txtUpperDate').val();
        data.sourceNotes =$('#txtNotes').val();
        data.originalLocation = $('#txtOriginalLoc').val();
        data.isCopyHeld = $('#chkIsCopyHeld').prop('checked');
        data.isViewed = $('#chkIsViewed').prop('checked');
        data.isThackrayFound = $('#chkIsThackrayFound').prop('checked');
        data.parishs = this.qryStrUtils.getParameterByName('pids', '');
        data.sourceTypes = this.qryStrUtils.getParameterByName('stypes', '');
        data.fileIds = this.qryStrUtils.getParameterByName('fids', '');
        return data;
    },

   save: function () {
        this.postParams.url = '/Sources/Add';
        this.postParams.data = this.GetSourceRecord();
        this.ancUtils.twaPostJSON(this.postParams);
   },

   saveReturn: function () {
        this.postParams.refreshmethod = function () { 
        window.location = '../HtmlPages/SourceSearch.html' + window.location.hash; };
        this.postParams.url = '/Sources/Add';
        this.postParams.data = this.GetSourceRecord();
        this.ancUtils.twaPostJSON(this.postParams);
   }

};