var JSMaster, QryStrUtils, AncUtils,Panels;

 


$(document).ready(function () {
    var jsMaster = new JSMaster();


    jsMaster.generateHeader('#1', function () {
        var ancMarriages = new AncMarriages();
        ancMarriages.init();

    });

});


var AncMarriages = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.selection = [];
    this.parishId = '';
    this.postParams = { 
        url: '',
        data: '',
        idparam: undefined,
        refreshmethod: this.getMarriages,
        refreshArgs: ['1'],
        Context: this
    };
 
};

AncMarriages.prototype = {

    init: function () {
        var isActive = this.qryStrUtils.getParameterByName('active', '');


        var panels = new Panels();



        $("#main").live("click", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $("#more").live("click", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));

        $("#refresh").live("click", $.proxy(function () { this.getMarriages("0"); return false; }, this));

        $("#add").live("click", $.proxy(function () { this.addMarriage('00000000-0000-0000-0000-000000000000'); return false; }, this));
        $("#delete").live("click", $.proxy(function () { this.DeleteRecord(); return false; }, this));
        $("#print").live("click", $.proxy(function () { this.PrintableResults(); return false; }, this));
        $("#dupe").live("click", $.proxy(function () { this.SetDuplicates(); return false; }, this));
        $("#merge").live("click", $.proxy(function () { this.SetMergeMarriages(); return false; }, this));
        $("#remove").live("click", $.proxy(function () { this.SetRemoveLink(); return false; }, this));

        $("#year_hed").live("click", $.proxy(function () { this.sort('MarriageDate'); return false; }, this));
        $("#mcname_hed").live("click", $.proxy(function () { this.sort('MaleCName'); return false; }, this));
        $("#msname_hed").live("click", $.proxy(function () { this.sort('MaleSName'); return false; }, this));
        $("#fcname_hed").live("click", $.proxy(function () { this.sort('FemaleCName'); return false; }, this));
        $("#fsname_hed").live("click", $.proxy(function () { this.sort('FemaleSName'); return false; }, this));

        $("#locat_hed").live("click", $.proxy(function () { this.sort('MarriageLocation'); return false; }, this));




        if (isActive == '1') {
            $('#txtMaleCName').val(this.qryStrUtils.getParameterByName('mcname', ''));
            $('#txtMaleSName').val(this.qryStrUtils.getParameterByName('msname', ''));
            $('#txtFemaleCName').val(this.qryStrUtils.getParameterByName('fcname', ''));
            $('#txtFemaleSName').val(this.qryStrUtils.getParameterByName('fsname', ''));
            $('#txtLocation').val(this.qryStrUtils.getParameterByName('locat', ''));

            $('#txtLowerDateRangeLower').val(this.qryStrUtils.getParameterByName('ldrl', ''));
            $('#txtLowerDateRangeUpper').val(this.qryStrUtils.getParameterByName('ldru', ''));

            this.parishId = this.qryStrUtils.getParameterByName('parid', '');

            this.getMarriages('1');
        }


    },

    createQryString: function () {

        var args = {
            "active": '1',
            "mcname": $('#txtMaleCName'),
            "msname": $('#txtMaleSName'),
            "fcname": $('#txtFemaleCName'),
            "fsname": $('#txtFemaleSName'),
            "locat": $('#txtLocation'),
            "ldrl": $('#txtLowerDateRangeLower'),
            "ldru": $('#txtLowerDateRangeUpper'),
            "parid": this.parishId
        };


        this.qryStrUtils.updateQry(args);

    },

    getMarriages: function (showdupes) {
        console.time('getMarriages');
        var parentId = '';
        if (showdupes == '0') {
            this.qryStrUtils.updateQryPar('_parentId', parentId);
        }
        else {
            parentId = this.qryStrUtils.getParameterByName('_parentId', '');
        }

        var params = {};
        params[0] = parentId;
        params[1] = String($('#txtMaleCName').val());
        params[2] = String($('#txtMaleSName').val());
        params[3] = String($('#txtFemaleCName').val());
        params[4] = String($('#txtFemaleSName').val());
        params[5] = String($('#txtLocation').val());
        params[6] = String($('#txtLowerDateRangeLower').val());
        params[7] = String($('#txtLowerDateRangeUpper').val());
        params[8] = '';
        params[9] = this.qryStrUtils.getParameterByName('parid', '');
        params[10] = String(this.qryStrUtils.getParameterByName('page', 0));
        params[11] = '30';
        params[12] = this.qryStrUtils.getParameterByName('sort_col', 'MarriageDate');

        this.ancUtils.twaGetJSON('/Marriages/GetMarriages/Select', params, $.proxy(this.marriageResult, this));

        this.createQryString();

        return false;
    },


    marriageResult: function (data) {

        console.time('result');

        var tableBody = '';
        var visibleRecords = [];

        var dupeEvents = [];
        var selectEvents = [];

        var _idx = 0;

        var that = this;

        $.each(data.serviceMarriages, function (source, sourceInfo) {

            var hidPID = '<input type="hidden" name="MarriageId" id="MarriageId" value ="' + sourceInfo.MarriageId + '"/>';
            var hidParID = '<input type="hidden" name="parent_id" id="parent_id" value ="' + sourceInfo.XREF + '"/>';

            var arIdx = jQuery.inArray(sourceInfo.MarriageId, that.selection);

            if (arIdx >= 0) {
                tableBody += '<tr class = "highLightRow">' + hidPID + hidParID;
            }
            else {
                tableBody += '<tr>' + hidPID + hidParID;
            }

            var _loc = window.location.hash;
            _loc = that.qryStrUtils.updateStrForQry(_loc, 'id', sourceInfo.MarriageId);

            tableBody += '<td><a id= "d' + _idx + '" href=""><div>' + sourceInfo.Events + '</div></a></td>';
            dupeEvents.push({ key: 'd' + _idx, value: sourceInfo.XREF });

            tableBody += '<td><a id= "s' + _idx + '" href=""><div>' + sourceInfo.MarriageDate + '</div></a></td>';
            selectEvents.push({ key: 's' + _idx, value: sourceInfo.MarriageId });

            tableBody += '<td><a href="../HtmlPages/MarriageEditor.html' + _loc + '"><div> Edit </div></a></td>';

            tableBody += '<td><div>' + sourceInfo.MaleCName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.MaleSName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.FemaleCName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.FemaleSName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.MarriageLocation + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.Witnesses + '</div></td>';
            tableBody += '</tr>';

            visibleRecords.push(sourceInfo.MarriageId);

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

            $('#reccount').html(data.Total + ' Marriages');
        }
        else {

            $('#search_bdy').html(tableBody);
            $('#reccount').html('0 Marriages');
        }


        this.ancUtils.addlinks(dupeEvents, this.loadDupes, this);

        this.ancUtils.addlinks(selectEvents, this.processSelect, this);
        console.timeEnd('getMarriages');
        console.timeEnd('result');
        console.log('result ended');
    },

    loadDupes: function (id) {
        this.qryStrUtils.updateQryPar('_parentId', id);
        this.getMarriages('1');
    },

    processSelect: function (evt) {
        this.ancUtils.handleSelection(evt, this.selection, '#search_bdy tr', "#MarriageId");
    },

    getLink: function (toPage) {
        this.qryStrUtils.updateQryPar('page', toPage);
        this.getMarriages('1');
    },

    sort: function (sort_col) {
        this.ancUtils.sort_inner(sort_col);
        this.getMarriages('1');
    },

    DeleteRecord: function () {
        this.postParams.url = '/Marriages/Delete';
        this.postParams.data = { marriageIds: this.ancUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },

    SetDuplicates: function () {
        this.postParams.url = '/Marriages/SetDuplicate';
        this.postParams.data = { marriages: this.ancUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },

    SetRemoveLink: function () {
        this.postParams.url = '/Marriages/RemoveLinks';
        this.postParams.data = { marriage: this.ancUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },

    SetMergeMarriages: function () {
        this.postParams.url = '/Marriages/MergeMarriages';
        this.postParams.data = { marriage: this.ancUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },

    addMarriage: function (path) {
        window.location.href = '../HtmlPages/MarriageEditor.html#' + this.qryStrUtils.makeIdQryString('id', path);
    }
};


