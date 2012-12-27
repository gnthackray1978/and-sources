var JSMaster,QryStrUtils,AncUtils,Panels;



$(document).ready(function () {
    var jsMaster = new JSMaster();


    jsMaster.generateHeader('#1', function () {
        var ancPersons = new AncPersons();
        ancPersons.init();

    });

});


var AncPersons = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
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

        $("#main").live("click", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $("#more").live("click", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
        $("#refresh").live("click", $.proxy(function () { this.getPersons("0"); return false; }, this));
        $("#add").live("click", $.proxy(function () { this.addPerson('00000000-0000-0000-0000-000000000000'); return false; }, this));
        $("#delete").live("click", $.proxy(function () { this.DeleteRecord(); return false; }, this));
        $("#print").live("click", $.proxy(function () { this.PrintableResults(); return false; }, this));
        $("#asslocs").live("click", $.proxy(function () { this.AssignLocations(); return false; }, this));
        $("#dupes").live("click", $.proxy(function () { this.SetDuplicates(); return false; }, this));
        $("#upests").live("click", $.proxy(function () { this.UpdateEstimates(); return false; }, this));
        $("#sfat").live("click", $.proxy(function () { this.SetRelation('2'); return false; }, this));
        $("#smot").live("click", $.proxy(function () { this.SetRelation('4'); return false; }, this));
        $("#sson").live("click", $.proxy(function () { this.SetRelation('3'); return false; }, this));
        $("#remove").live("click", $.proxy(function () { this.SetRemoveLink(); return false; }, this));
        $("#merge").live("click", $.proxy(function () { this.SetMergeSources(); return false; }, this));
        $("#sdau").live("click", $.proxy(function () { this.SetRelation('5'); return false; }, this));
        $("#sbro").live("click", $.proxy(function () { this.SetRelation('6'); return false; }, this));
        $("#ssis").live("click", $.proxy(function () { this.SetRelation('7'); return false; }, this));

        $("#sbint").live("click", $.proxy(function () { this.sort('BirthInt'); return false; }, this));
        $("#sdint").live("click", $.proxy(function () { this.sort('DeathInt'); return false; }, this));

        $("#sbloc").live("click", $.proxy(function () { this.sort('BirthLocation'); return false; }, this));
        $("#sname").live("click", $.proxy(function () { this.sort('ChristianName'); return false; }, this));

        $("#ssurname").live("click", $.proxy(function () { this.sort('Surname'); return false; }, this));

        $("#sfather").live("click", $.proxy(function () { this.sort('FatherChristianName'); return false; }, this));
        $("#smother").live("click", $.proxy(function () { this.sort('MotherChristianName'); return false; }, this));
        $("#sdloc").live("click", $.proxy(function () { this.sort('DeathLocation'); return false; }, this));


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

            this.parishId = this.qryStrUtils.getParameterByName('parid', '');

            this.getPersons('1');
        }

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
            "inct": $('#txtLocation').prop('checked'),
            "incb": $('#txtLowerDateRangeLower').prop('checked'),
            "incd": $('#txtLowerDateRangeUpper').prop('checked'),
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
        params[14] = '';
        params[15] = String($('#txtSpouse').val());
        params[16] = this.parishId;
        params[17] = String(this.qryStrUtils.getParameterByName('page', 0));
        params[18] = '30';
        params[19] = String(this.qryStrUtils.getParameterByName('sort_col', 'BirthInt'));

        this.ancUtils.twaGetJSON('/GetPersons/Select', params, $.proxy(this.processData, this));

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
            var hidParID = '<input type="hidden" name="parent_id" id="parent_id" value ="' + sourceInfo.XREF + '"/>';

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
            dupeEvents.push({ key: 'd' + _idx, value: sourceInfo.XREF });


            tableBody += '<td><a href="../HtmlPages/PersonEditor.html' + _loc + '"><div> Edit </div></a></td>';
            tableBody += '<td><div class = "dates" >' + sourceInfo.BirthYear + '-' + sourceInfo.DeathYear + '</div></td>';

            tableBody += '<td><div>' + sourceInfo.BirthLocation + '</div></td>';

            tableBody += '<td><a id= "s' + _idx + '" href="" ><div>' + sourceInfo.ChristianName + '</div></a></td>';
            selectEvents.push({ key: 's' + _idx, value: sourceInfo.PersonId });

            tableBody += '<td><div>' + sourceInfo.Surname + '</div></td>';

            if (sourceInfo.Spouse === '')
                tableBody += '<td><div class = "parent">' + sourceInfo.FatherChristianName + '</div></td>';
            else
                tableBody += '<td><div class = "spouse">' + sourceInfo.Spouse + '</div></td>';


            tableBody += '<td><div>' + sourceInfo.MotherChristianName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.MotherSurname + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.DeathLocation + '</div></td>';

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

            this.ancUtils.createpager(pagerparams);

            $('#reccount').html(data.Total + ' Persons');


        }
        else {
            $('#search_bdy').html(tableBody);
            $('#reccount').html('0 Persons');
        }


        this.ancUtils.addlinks(dupeEvents, this.loadDupes, this);

        this.ancUtils.addlinks(selectEvents, this.processSelect, this);
    },
    loadDupes: function (id) {
        this.qryStrUtils.updateQryPar('_parentId', id);
        this.getPersons('1');
    },
    processSelect: function (evt) {
        this.ancUtils.handleSelection(evt, this.selection, '#search_bdy tr', "#person_id");
    },
    getLink: function (toPage) {
        this.qryStrUtils.updateQryPar('page', toPage);
        this.getPersons('1');
    },
    sort: function (sort_col) {
        this.ancUtils.sort_inner(sort_col);
        this.getPersons('1');
    },
    addPerson: function (path) {
        window.location.href = '../HtmlPages/PersonEditor.html#' + this.qryStrUtils.makeIdQryString('id', path);
    },

    DeleteRecord: function () {
        this.postParams.url = '/Person/Delete';
        this.postParams.data = { personId: this.ancUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },
    PrintableResult: function () {

    },
    AssignLocations: function () {
        this.postParams.url = '/Person/AssignLocats';
        this.ancUtils.twaPostJSON(this.postParams);
    },
    SetDuplicates: function () {
        this.postParams.url = '/Person/SetDuplicate';
        this.postParams.data = { persons: this.ancUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },
    UpdateEstimates: function () {
        this.ancUtils.twaGetJSON('/Person/UpdateDates', '', function () { });
    },
    SetRelation: function (relationid) {
        this.postParams.url = '/Person/SetDuplicate';
        this.postParams.data = { persons: this.ancUtils.convertToCSV(this.selection), relationType: relationid };
        this.ancUtils.twaPostJSON(this.postParams);
    },
    SetRemoveLink: function () {
        this.postParams.url = '/Person/RemoveLinks';
        this.postParams.data = { person: this.ancUtils.convertToCSV(this.selection)};
        this.ancUtils.twaPostJSON(this.postParams);
    },
    SetMergeSources: function(){
        this.postParams.url = '/Person/MergePersons';
        this.postParams.data = { person: this.ancUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    }


};
















