
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

    this.DEFAULT_GET_URL = '/Sources/GetSource';
    this.DEFAULT_ADD_URL = '/Sources/Add';
    this.DEFAULT_SEARCHPAGE_URL = '../HtmlPages/SourceSearch.html';

    this.ancSelectorParishs = new AncSelectorParishs();
    this.ancSelectorSourceTypes = new AncSelectorSourceTypes();
    this.fileTable = null;
    this.fileHistory =[];

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

       $('body').on("click","#save", $.proxy(function () { this.save(); return false; }, this));
       
       $('body').on("click", "#return", $.proxy(function () { this.saveReturn(); return false; }, this));
       
       $('body').on("click", "#main", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
       
       $('body').on("click", "#notes", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
       
       $('body').on("click", "#files", $.proxy(function () { panels.sourcesShowPanel('3'); return false; }, panels));



       this.load();

       return false;

   },

   load: function(){
       var params = {};
       params[0] = this.qryStrUtils.getParameterByName('id', '');

       this.ancUtils.twaGetJSON(this.DEFAULT_GET_URL, params, $.proxy(this.processData, this));
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
       

       //get file data and write it 

        this.createFileTable(data.Files);
       
        this.fileTable = $('#file-urls').dataTable();
        this.fileTable.fnSort([[0, 'asc'], [1, 'asc']]);

        $('body').on("click", ".file-edit", $.proxy(function (event) {
            
            $('#new-id').val($(event.currentTarget).closest('tr').attr('data-id'));
            $('#new-url').val($(event.currentTarget).closest('tr').attr('data-url'));
            $('#new-descrip').val($(event.currentTarget).closest('tr').attr('data-desc'));
            return false;
        }, this));



        $('body').on("click", ".file-remove", $.proxy(function (event) {
            var row = $(event.currentTarget).closest('tr').get(0);//.remove();
            this.fileTable.fnDeleteRow(this.fileTable.fnGetPosition(row));
            
            this.fileHistory.push({ id: $(event.currentTarget).closest('tr').attr('data-id'), operation: 'remove' });
            return false;
        }, this));
       
        $('body').on("click", "#new-save", $.proxy(function (event) {

            var b = this.fileTable.fnFindRow($('#new-id').val(), 'data-id');

            var urlLink = '<a href ="' + $('#new-url').val() + '">' + $('#new-descrip').val() + '</a>';

            if (b.idx == -1) {

                var g = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                    var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                    return v.toString(16);
                });

                $('#new-id').val(g);

               var tp= this.fileTable.fnAddData([urlLink, '<a href ="" class = "file-edit">Edit</a>', '<a href ="" class = "file-remove">Remove</a>']);

                b = this.fileTable.fnGetNodes(tp[0]);

              
                $(b).attr("data-id", $('#new-id').val());
                $(b).attr("data-url", $('#new-url').val());
                $(b).attr("data-desc", $('#new-descrip').val());
                
            } else {
               
                this.fileTable.fnUpdate([urlLink, '<a href ="" class = "file-edit">Edit</a>', '<a href ="" class = "file-remove">Remove</a>'], b.idx); // Row

                b.row.attr("data-id", $('#new-id').val());
                b.row.attr("data-url", $('#new-url').val());
                b.row.attr("data-desc", $('#new-descrip').val());

            }

            this.fileHistory.push({ id: $('#new-id').val(), operation: 'edit' });

            return false;
        }, this));
       
       
        $('body').on("click", "#new-add", $.proxy(function (event) {
            
           $('#new-id').val('');
           $('#new-url').val('');
           $('#new-descrip').val('');

            return false;
        }, this));
       
   },
   createFileTable:function(files) {
       var idx = 0;

    //   if (this.fileTable != null || this.fileTable != undefined)
      //  this.fileTable.fnClearTable();
       this.fileTable = null;
       
       $('#file-list').html('');

       var newData = '';

       while (idx < files.length) {


           newData += '<tr class ="file-row" data-id="' + files[idx].FileId + '" data-url="' + files[idx].Url + '"  data-desc="' + files[idx].Description + '"  >' +
               ' <td> <a href ="' + files[idx].Url + '">' + files[idx].Description + '</a></td><td><a href ="" class = "file-edit">Edit</a></td><td> <a href ="" class = "file-remove">Remove</a> </td></tr>';
           idx++;
       }
       
       $('#file-list').html(newData);
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
        data.fileIds = this.GetFileData();
       

        return data;
    },

   GetFileData:function() {
        
        var nodes = this.fileTable.fnGetNodes();
        var hidx = 0;
        var idx = 0;
        var result = [];

        if (this.fileHistory.length > 0) {
            while (idx < nodes.length) {

                var dataid = $(nodes[idx]).attr('data-id');

                hidx = 0;
                var found = false;
                while (hidx < this.fileHistory.length) {
                    if (this.fileHistory[hidx].id == dataid)
                        found = true;

                    hidx++;
                }
                if (found)
                    result.push({ id: dataid, desc: $(nodes[idx]).attr('data-desc'), url: $(nodes[idx]).attr('data-url') });
                idx++;
            }

            hidx = 0;

            while (hidx < this.fileHistory.length) {
                if (this.fileHistory[hidx].operation == 'remove')
                    result.push({ id: this.fileHistory[hidx].id, desc: '', url: '' });
                
                hidx++;
            }
        }
        
        return JSON.stringify(result);;
    },

   save: function () {
       this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetSourceRecord();
        this.ancUtils.twaPostJSON(this.postParams);
   },

   saveReturn: function () {
        this.postParams.refreshmethod = function () { 
        window.location = this.DEFAULT_SEARCHPAGE_URL + window.location.hash; };
        this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetSourceRecord();
        this.ancUtils.twaPostJSON(this.postParams);
   }

};