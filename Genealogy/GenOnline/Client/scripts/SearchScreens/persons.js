var JSMaster,QryStrUtils,AncUtils,Panels;



$(document).ready(function () {
    var jsMaster = new JSMaster();

    console.log('person ready');

    jsMaster.generateHeader('#1', function () {
        console.log('person header generated');
        var ancPersons = new AncPersons();
        ancPersons.init();

    });

});


var AncPersons = function () {
    this.qryStrUtils = new QryStrUtils();
    this.selectorTools = new SelectorTools();
    this.ancUtils = new AncUtils();
    this.pager = new Pager();
    this.DEFAULT_PERSONSELECT_URL = '/PersonService/Get/Select';
    this.DEFAULT_PERSONDELETE_URL = '/PersonService/Delete';
    this.DEFAULT_PERSONASSIGNLOCATS_URL = '/PersonService/AssignLocats';
    this.DEFAULT_PERSONSETDUPES_URL = '/PersonService/SetDuplicate';
    this.DEFAULT_PERSONUPDATEDATES_URL = '/PersonService/UpdateDates';
    this.DEFAULT_PERSONREMOVELINKS_URL = '/PersonService/RemoveLinks';
    this.DEFAULT_PERSONMERGE_URL = '/PersonService/MergePersons';
    this.DEFAULT_SETSOURCE_URL = '/Sources/AddPersonTreeSource';
    this.DEFAULT_REMOVESOURCE_URL = '/Sources/RemoveTreeSources';

    this.DEFAULT_SOURCESELECT_URL = '/Sources/Select';
    this.DEFAULT_PERSONEDITOR_URL = '../HtmlPages/PersonEditor.html';
    this.DEFAULT_SOURCEEDITOR_URL = '../HtmlPages/SourceEditor.html';

    this.selection = [];
    this.parishId = '';

    this.postParams = {
        url: '',
        data: '',
        idparam: undefined,
        refreshmethod: this.getPersons,
        refreshArgs: ['1'],
        Context: this
    };
};



AncPersons.prototype = {

    init: function () {
        var isActive = this.qryStrUtils.getParameterByName('active', '');


        var panels = new Panels();

        $('body').on("click", "#main", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $('body').on("click", "#more", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
        $('body').on("click", "#refresh", $.proxy(function () { this.getPersons("0"); return false; }, this));
        $('body').on("click", "#add", $.proxy(function () { this.addPerson('00000000-0000-0000-0000-000000000000'); return false; }, this));
        $('body').on("click", "#delete", $.proxy(function () { this.DeleteRecord(); return false; }, this));
        $('body').on("click", "#print", $.proxy(function () { this.PrintableResults(); return false; }, this));
        $('body').on("click", "#asslocs", $.proxy(function () { this.AssignLocations(); return false; }, this));
        $('body').on("click", "#dupes", $.proxy(function () { this.SetDuplicates(); return false; }, this));
        $('body').on("click", "#upests", $.proxy(function () { this.UpdateEstimates(); return false; }, this));
        $('body').on("click", "#remove-trees", $.proxy(function () { this.RemoveSources(); return false; }, this));
        $('body').on("click", "#add-tree", $.proxy(function () { this.SetSources(); return false; }, this));
        
      //  $('body').on("click", "#sson", $.proxy(function () { this.SetRelation('3'); return false; }, this));
        $('body').on("click", "#remove", $.proxy(function () { this.SetRemoveLink(); return false; }, this));
        $('body').on("click", "#merge", $.proxy(function () { this.SetMergeSources(); return false; }, this));
    //    $('body').on("click", "#sdau", $.proxy(function () { this.SetRelation('5'); return false; }, this));
     //   $('body').on("click", "#sbro", $.proxy(function () { this.SetRelation('6'); return false; }, this));
    //    $('body').on("click", "#ssis", $.proxy(function () { this.SetRelation('7'); return false; }, this));

        $('body').on("click", "#sbint", $.proxy(function () { this.sort('BirthInt'); return false; }, this));
        $('body').on("click", "#sdint", $.proxy(function () { this.sort('DeathInt'); return false; }, this));

        $('body').on("click", "#sbloc", $.proxy(function () { this.sort('BirthLocation'); return false; }, this));
        $('body').on("click", "#sname", $.proxy(function () { this.sort('ChristianName'); return false; }, this));

        $('body').on("click", "#ssurname", $.proxy(function () { this.sort('Surname'); return false; }, this));

        $('body').on("click", "#sfather", $.proxy(function () { this.sort('FatherChristianName'); return false; }, this));
        $('body').on("click", "#smother", $.proxy(function () { this.sort('MotherChristianName'); return false; }, this));
        $('body').on("click", "#sinfo", $.proxy(function () { this.sort('SourceRef'); return false; }, this));


        if (isActive == '1') {
            $('#txtCName').val(this.qryStrUtils.getParameterByName('cname', ''));
            $('#txtSName').val(this.qryStrUtils.getParameterByName('sname', ''));
            $('#txtFCName').val(this.qryStrUtils.getParameterByName('fcname', ''));
            $('#txtFSName').val(this.qryStrUtils.getParameterByName('fsname', ''));
            $('#txtMCName').val(this.qryStrUtils.getParameterByName('mcname', ''));
            $('#txtMSName').val(this.qryStrUtils.getParameterByName('msname', ''));
            $('#txtLocation').val(this.qryStrUtils.getParameterByName('locat', ''));
            $('#txtCounty').val(this.qryStrUtils.getParameterByName('count', ''));
            $('#txtLowerDateRangeLower').val(this.qryStrUtils.getParameterByName('ldrl', ''));
            $('#txtLowerDateRangeUpper').val(this.qryStrUtils.getParameterByName('ldru', ''));

            if (this.qryStrUtils.getParameterByName('inct', '') == 'false') {
                $('#chkIncludeTree').prop('checked', false);
            }
            else {
                $('#chkIncludeTree').prop('checked', true);
            }


            if (this.qryStrUtils.getParameterByName('incb', '') == 'false') {
                $('#chkIncludeBirths').prop('checked', false);
            }
            else {
                $('#chkIncludeBirths').prop('checked', true);
            }


            if (this.qryStrUtils.getParameterByName('incd', '') == 'false') {
                $('#chkIncludeDeaths').prop('checked', false);
            }
            else {
                $('#chkIncludeDeaths').prop('checked', true);
            }
            

            if (this.qryStrUtils.getParameterByName('incs', '') == 'false') {
                $('#chkIncludeSources').prop('checked', false);
            }
            else {
                $('#chkIncludeSources').prop('checked', true);
            }
            

            this.parishId = this.qryStrUtils.getParameterByName('parid', '');
            console.log('person calling get data');
            this.getPersons('1');
        }

        
        this.getSources();
        

     //'remove-trees'  
                   
         //         'add-tree' 
    },

    createQryString: function () {

        var args = {
            "active": '1',
            "cname": $('#txtCName'),
            "sname": $('#txtSName'),
            "fcname": $('#txtFCName'),
            "fsname": $('#txtFSName'),
            "mcname": $('#txtMCName'),
            "msname": $('#txtMSName'),
            "locat": $('#txtLocation'),
            "count": $('#txtCounty'),
            "ldrl": $('#txtLowerDateRangeLower'),
            "ldru": $('#txtLowerDateRangeUpper'),
            "inct": $('#chkIncludeTree').prop('checked'),
            "incb": $('#chkIncludeBirths').prop('checked'),
            "incd": $('#chkIncludeDeaths').prop('checked'),
            "incs": $('#chkIncludeSources').prop('checked'),
            "parid": this.parishId
        };


        this.qryStrUtils.updateQry(args);
    },

    getPersons: function (showdupes) {

        var params = {};
        var parentId = '';

        if (showdupes == '0') {
            this.qryStrUtils.updateQryPar('_parentId', parentId);
        }
        else {
            parentId = this.qryStrUtils.getParameterByName('_parentId', '');
        }

        params[0] = parentId;
        params[1] = String($('#txtCName').val());
        params[2] = String($('#txtSName').val());
        params[3] = String($('#txtFCName').val());
        params[4] = String($('#txtFSName').val());
        params[5] = String($('#txtMCName').val());
        params[6] = String($('#txtMSName').val());

        params[7] = String($('#txtLocation').val());
        params[8] = String($('#txtCounty').val());

        params[9] = String($('#txtLowerDateRangeLower').val());
        params[10] = String($('#txtLowerDateRangeUpper').val());

        params[11] = String($('#chkIncludeTree').prop('checked'));
        params[12] = String($('#chkIncludeBirths').prop('checked'));
        params[13] = String($('#chkIncludeDeaths').prop('checked'));
        params[14] = String($('#chkIncludeSources').prop('checked'));
        params[15] = String(this.qryStrUtils.getParameterByName('sids', ''));
        params[16] = String($('#txtSpouse').val());
        params[17] = this.parishId;
        params[18] = String(this.qryStrUtils.getParameterByName('page', 0));
        params[19] = '30';
        params[20] = String(this.qryStrUtils.getParameterByName('sort_col', 'BirthInt'));

        this.ancUtils.twaGetJSON(this.DEFAULT_PERSONSELECT_URL, params, $.proxy(this.processData, this));

        this.createQryString();

        return false;
    },

    processData: function (data) {

        var tableBody = '';
        var visibleRecords = [];
        var dupeEvents = [];
        var selectEvents = [];
        var _idx = 0;
        var that = this;


        $.each(data.servicePersons, function (source, sourceInfo) {

            var hidPID = '<input type="hidden" name="person_id" id="person_id" value ="' + sourceInfo.PersonId + '"/>';
            var hidParID = '<input type="hidden" name="parent_id" id="parent_id" value ="' + sourceInfo.UniqueReference + '"/>';

            var arIdx = jQuery.inArray(sourceInfo.PersonId, that.selection);

            if (arIdx >= 0) {
                tableBody += '<tr class = "highLightRow">' + hidPID + hidParID;
            }
            else {
                tableBody += '<tr>' + hidPID + hidParID;
            }

            var _loc = window.location.hash;
            _loc = that.qryStrUtils.updateStrForQry(_loc, 'id', sourceInfo.PersonId);

            tableBody += '<td><a id= "d' + _idx + '" href=""><div>' + sourceInfo.Events + '</div></a></td>';
            dupeEvents.push({ key: 'd' + _idx, value: sourceInfo.UniqueReference });


            tableBody += '<td><a href="' + this.DEFAULT_PERSONEDITOR_URL + _loc + '"><div> Edit </div></a></td>';


            if (sourceInfo.SourceDateStr == '')
                tableBody += '<td><div class = "dates" >' + sourceInfo.BirthYear + '-' + sourceInfo.DeathYear + '</div></td>';
            else  
                tableBody += '<td><div class = "dates" >' + sourceInfo.SourceDateStr + '</div></td>';
             



            if (sourceInfo.SourceParishName == '') {
                
                if (sourceInfo.DeathLocation == '') {
                    tableBody += '<td><div>' + sourceInfo.BirthLocation + '</div></td>';
                } else {
                    tableBody += '<td><div>' + sourceInfo.BirthLocation + ' -> '+sourceInfo.DeathLocation + '</div></td>';
                }
                
            } else {
                tableBody += '<td><div>' + sourceInfo.SourceParishName + '</div></td>';
            }


            tableBody += '<td><a id= "s' + _idx + '" href="" ><div>' + sourceInfo.ChristianName + '</div></a></td>';
            selectEvents.push({ key: 's' + _idx, value: sourceInfo.PersonId });


            if (sourceInfo.LinkedTrees !== '')
                tableBody += '<td><div class = "associatedTrees" title="' + sourceInfo.LinkedTrees + '">' + sourceInfo.Surname + '</div></td>';
            else
                tableBody += '<td><div>' + sourceInfo.Surname + '</div></td>';
            


            if (sourceInfo.Spouse === '')
                tableBody += '<td><div class = "parent">' + sourceInfo.FatherChristianName + '</div></td>';
            else
                tableBody += '<td><div class = "spouse">' + sourceInfo.Spouse + '</div></td>';




            tableBody += '<td><div>' + sourceInfo.MotherChristianName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.MotherSurname + '</div></td>';
         //   <td><div>' + sourceInfo.SourceRef + '</div></td>';

            _loc = window.location.hash;
            _loc = that.qryStrUtils.updateStrForQry(_loc, 'id', sourceInfo.SourceId);

            tableBody += '<td><a href="' + this.DEFAULT_SOURCEEDITOR_URL + _loc + '"><div>' + sourceInfo.SourceRef + '</div></a></td>';
            
            
            tableBody += '</tr>';

            visibleRecords.push(sourceInfo.PersonId);
            _idx++;
        });

        if (this.selection !== undefined) {
            this.selection = this.selection.RemoveInvalid(visibleRecords);
        }

        if (tableBody !== '') {

            $('#search_bdy').html(tableBody);

            //create pager based on results
            var pagerparams = { ParentElement: 'pager',
                Batch: data.Batch,
                BatchLength: data.BatchLength,
                Total: data.Total,
                Function: this.getLink,
                Context: this
            };

            this.pager.createpager(pagerparams);

            $('#reccount').html(data.Total + ' Persons');


        }
        else {
            $('#search_bdy').html(tableBody);
            $('#reccount').html('0 Persons');
        }


        this.selectorTools.addlinks(dupeEvents, this.loadDupes, this);

        this.selectorTools.addlinks(selectEvents, this.processSelect, this);
    },

    getSources: function () {

        var params = {};
        params[0] = '87';
        params[1] = '';
        params[2] = '';
        params[3] = '';
        params[4] = '0';
        params[5] = '0';
        params[6] = '2000';
        params[7] = '2000';
        params[8] = '';
        params[9] = 'false';
        params[10] = 'false';
        params[11] = 'false';
        params[12] = 'false';
        params[13] = '0';
        params[14] = '30';
        params[15] = 'sdate';

        this.ancUtils.twaGetJSON(this.DEFAULT_SOURCESELECT_URL, params, function (data) {
            var tableBody = '';
            $.each(data.serviceSources, function (source, sourceInfo) {
                //<option value="volvo">Volvo</option> //sourceInfo.SourceDesc           
                tableBody += '<option value="' + sourceInfo.SourceId + '">' + sourceInfo.SourceRef + '</option>';
            });
            if (tableBody !== '') $('#tree-select').html(tableBody);
        });

        return false;
    },

    loadDupes: function (id) {
        this.qryStrUtils.updateQryPar('_parentId', id);
        this.getPersons('1');
    },

    processSelect: function (evt) {
        this.selectorTools.handleSelection(evt, this.selection, '#search_bdy tr', "#person_id");
    },

    getLink: function (toPage) {
        this.qryStrUtils.updateQryPar('page', toPage);
        this.getPersons('1');
    },

    sort: function (sort_col) {
        this.qryStrUtils.sort_inner(sort_col);
        this.getPersons('1');
    },

    addPerson: function (path) {
        window.location.href = this.DEFAULT_PERSONEDITOR_URL + '#' + this.qryStrUtils.makeIdQryString('id', path);
    },

    DeleteRecord: function () {
        this.postParams.url = this.DEFAULT_PERSONDELETE_URL;
        this.postParams.data = { personId: this.qryStrUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },

    PrintableResult: function () {

    },

    AssignLocations: function () {
        this.postParams.url = this.DEFAULT_PERSONASSIGNLOCATS_URL;
        this.ancUtils.twaPostJSON(this.postParams);
    },

    SetDuplicates: function () {
        this.postParams.url = this.DEFAULT_PERSONSETDUPES_URL;
        this.postParams.data = { persons: this.qryStrUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },

    UpdateEstimates: function () {
        this.ancUtils.twaGetJSON(this.DEFAULT_PERSONUPDATEDATES_URL, '', function () { });
    },

    SetRelation: function (relationid) {
        this.postParams.url = this.DEFAULT_PERSONSETDUPES_URL;
        this.postParams.data = { persons: this.qryStrUtils.convertToCSV(this.selection), relationType: relationid };
        this.ancUtils.twaPostJSON(this.postParams);
    },

    SetRemoveLink: function () {
        this.postParams.url = this.DEFAULT_PERSONREMOVELINKS_URL;
        this.postParams.data = { person: this.qryStrUtils.convertToCSV(this.selection)};
        this.ancUtils.twaPostJSON(this.postParams);
    },
    
    SetMergeSources: function(){
        this.postParams.url = this.DEFAULT_PERSONMERGE_URL;
        this.postParams.data = { person: this.qryStrUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },

    SetSources: function () {
        this.postParams.url = this.DEFAULT_SETSOURCE_URL;
        this.postParams.data = { record:this.qryStrUtils.convertToCSV(this.selection),sources: $("#tree-select").val()};
        this.ancUtils.twaPostJSON(this.postParams);
    },

    RemoveSources: function () {
        this.postParams.url = this.DEFAULT_REMOVESOURCE_URL;
        this.postParams.data = { record: this.qryStrUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    }

};
















