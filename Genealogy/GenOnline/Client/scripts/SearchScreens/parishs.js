var JSMaster, QryStrUtils, AncUtils,Panels;


$(document).ready(function () {
    var jsMaster = new JSMaster();

    jsMaster.generateHeader('#1', function () {
        var ancParishs = new AncParishs();
        ancParishs.init();
    });
});


var AncParishs = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.selectorTools = new SelectorTools();
    this.pager = new Pager();

    this.DEFAULT_BATCHENTRY_URL = '../HtmlPages/batchEntry.html';
    this.DEFAULT_PARISHEDITOR_URL = '../HtmlPages/ParishEditor.html';
    this.DEFAULT_PARISHSELECT_URL = '/ParishService/GetParishs/Select';
    this.DEFAULT_PARISHDELETE_URL = '/ParishService/Delete';


    this.selection = [];
  
    this.postParams = {
        url: '',
        data: '',
        idparam: undefined,
        refreshmethod: this.getParishs,
        refreshArgs: ['1'],
        Context: this
    };
};



AncParishs.prototype = {

    init: function () {
        var isActive = this.qryStrUtils.getParameterByName('active', '');

        var panels = new Panels();

        $('body').on("click", "#main", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $('body').on("click", "#more", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
        $('body').on("click", "#refresh", $.proxy(function () { this.getParishs(); return false; }, this));


        $('body').on("click", "#add", $.proxy(function () { this.AddParish(); return false; }, this));
        $('body').on("click", "#delete", $.proxy(function () { this.DeleteRecord(); return false; }, this));
        $('body').on("click", "#select_return", $.proxy(function () { this.returnselection(); return false; }, this));

        $('body').on("click", "#pname", $.proxy(function () { this.sort("ParishName"); return false; }, this));
        $('body').on("click", "#pdeposited", $.proxy(function () { this.sort("ParishDeposited"); return false; }, this));
        $('body').on("click", "#pparent", $.proxy(function () { this.sort("ParishParent"); return false; }, this));
        $('body').on("click", "#pstartyear", $.proxy(function () { this.sort("ParishStartYear"); return false; }, this));
        $('body').on("click", "#pendyear", $.proxy(function () { this.sort("ParishEndYear"); return false; }, this));
        $('body').on("click","#pcounty", $.proxy(function () { this.sort("ParishCounty"); return false; }, this));

        if (isActive == '1') {
            $('#txtDeposited').val(this.qryStrUtils.getParameterByName('dep', ''));
            $('#txtName').val(this.qryStrUtils.getParameterByName('name', ''));
            $('#txtCounty').val(this.qryStrUtils.getParameterByName('count', ''));

            this.getParishs('1');
        }


        var isPersonImpSelection = this.qryStrUtils.getParameterByName('parl', '');

        if (isPersonImpSelection !== null) {
            $("#rLink").removeClass("hidePanel").addClass("displayPanel");
        }
        else {
            $("#rLink").addClass("hidePanel").removeClass("displayPanel");
        }

    },
    returnselection: function () {

        //var parl = this.qryStrUtils.getParameterByName('parl', '');
        var parishLst = '';

        $.each(this.selection, function (idx, val) {
            if (idx > 0) {
                parishLst += ',' + val;
            }
            else {
                parishLst += val;
            }
        });

        this.qryStrUtils.updateQryPar('parl', parishLst);

        var sources = this.qryStrUtils.getParameterByName('scs', '');
        //dont lose these if they are there.
        var parishs = this.qryStrUtils.getParameterByName('parl', '');

        var _loc = '#?scs=' + sources + '&parl=' + parishs;

        var url = this.DEFAULT_BATCHENTRY_URL + _loc;

        window.location.href = url;
    },
    createQryString: function () {

        var args = {
            "active": '1',
            "dep": $('#txtDeposited'),
            "name": $('#txtName'),
            "count": $('#txtCounty')
        };

        this.qryStrUtils.updateQry(args);

    },
    getParishs: function () {

        var params = {};

        params[0] = String($('#txtDeposited').val());
        params[1] = String($('#txtName').val());
        params[2] = String($('#txtCounty').val());
        params[3] = String(this.qryStrUtils.getParameterByName('page', 0));
        params[4] = '30';
        params[5] = String(this.qryStrUtils.getParameterByName('sort_col', 'ParishName'));

        this.ancUtils.twaGetJSON(this.DEFAULT_PARISHSELECT_URL, params, $.proxy(this.processData, this));

        this.createQryString();

        return false;
    },
    processData: function (data) {
        //alert('received something');
        var tableBody = '';
        var selectEvents = [];
        var _idx = 0;
        var that = this;


        $.each(data.serviceParishs, function (source, sourceInfo) {

            var hidPID = '<input type="hidden" name="ParishId" id="ParishId" value ="' + sourceInfo.ParishId + '"/>';
            var arIdx = jQuery.inArray(sourceInfo.ParishId, this.selection);

            if (arIdx >= 0) {
                tableBody += '<tr class = "highLightRow">' + hidPID;// + hidParID;
            }
            else {
                tableBody += '<tr>' + hidPID;
            }

            var _loc = window.location.hash;
            _loc = that.qryStrUtils.updateStrForQry(_loc, 'id', sourceInfo.ParishId);

            tableBody += '<td><a id= "s' + _idx + '" href="" ><div>' + sourceInfo.ParishName + '</div></a></td>';
            selectEvents.push({ key: 's' + _idx, value: sourceInfo.ParishId });

            tableBody += '<td><div>' + sourceInfo.ParishDeposited + '</div></td>';
            tableBody += '<td><a href="' + this.DEFAULT_PARISHEDITOR_URL + _loc + '"><div> Edit </div></a></td>';
            tableBody += '<td><div>' + sourceInfo.ParishParent + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.ParishStartYear + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.ParishEndYear + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.ParishCounty + '</div></td>';

            tableBody += '</tr>';
            _idx++;
        });

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

            $('#reccount').html(data.Total + ' Parishs');
        }
        else {

            $('#search_bdy').html(tableBody);
            $('#reccount').html('0 Parishs');
        }

        this.selectorTools.addlinks(selectEvents, this.processSelect, this);
    },
    processSelect: function (evt) {
        this.selectorTools.handleSelection(evt, this.selection, '#search_bdy tr', "#ParishId");
    },
    sort: function (sort_col) {
        this.qryStrUtils.sort_inner(sort_col);
        this.getParishs('1');
    },
    getLink: function (toPage) {
        this.qryStrUtils.updateQryPar('page', toPage);
        this.getParishs('1');
    },
    DeleteRecord: function () {
        this.postParams.url = this.DEFAULT_PARISHDELETE_URL;
        this.postParams.data = { parishIds: this.qryStrUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },
    AddParish: function () {
        window.location.href = this.DEFAULT_PARISHEDITOR_URL + '#' + this.qryStrUtils.makeIdQryString('id', '00000000-0000-0000-0000-000000000000');
    }
};



 