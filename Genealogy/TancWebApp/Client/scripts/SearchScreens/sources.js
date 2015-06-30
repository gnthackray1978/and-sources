var JSMaster, QryStrUtils,AncUtils,Panels,SourceTypeLookup;

//var setDefaultPersonUrl = getHost() + "/settreepersons/Set";
//var saveSourceUrl = getHost() + "/SaveTree/Save";

//var deleteSourceUrl = getHost() + "/Source/Delete";


$(document).ready(function () {
    var jsMaster = new JSMaster();


    jsMaster.generateHeader('#1', function () {
        var ancSources = new AncSources();
        ancSources.init();

    });

});


var AncSources = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.selection = [];
    this.parishId = '';

    this.postParams = { 
        url: '',
        data: '',
        idparam: undefined,
        refreshmethod: this.getSources,
        refreshArgs: ['1'],
        Context: this
    };
 
};

AncSources.prototype = {

    init: function () {


        var isActive = this.qryStrUtils.getParameterByName('active');

        var panels = new Panels();

        $("body").on("click", "#main", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $("body").on("click", "#more", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
        $("body").on("click", "#additional", $.proxy(function () { panels.sourcesShowPanel('3'); return false; }, panels));
        $("body").on("click", "#refresh", $.proxy(function () { this.getSources(); return false; }, this));


        $("body").on("click", "#add", $.proxy(function () { this.addSource('00000000-0000-0000-0000-000000000000'); return false; }, this));
        $("body").on("click", "#delete", $.proxy(function () { this.deleteSources(); return false; }, this));
        $("body").on("click", "#print", $.proxy(function () { this.printableSources(); return false; }, this));
        $("body").on("click", "#select_return", $.proxy(function () { this.returnselection(); return false; }, this));

        $("body").on("click", "#sdate", $.proxy(function () { this.sort("SourceDate"); return false; }, this));
        $("body").on("click", "#sref", $.proxy(function () { this.sort("SourceRef"); return false; }, this));
        $("body").on("click", "#sdesc", $.proxy(function () { this.sort("SourceDescription"); return false; }, this));



        if (isActive == '1') {

            $('#txtSourceRef').val(this.qryStrUtils.getParameterByName('sref', ''));
            $('#txtSourceDescription').val(this.qryStrUtils.getParameterByName('sdesc', ''));
            $('#txtOriginalLocation').val(this.qryStrUtils.getParameterByName('origLoc', ''));

            $('#txtLowerDateRangeLower').val(this.qryStrUtils.getParameterByName('ldrl', ''));
            $('#txtLowerDateRangeUpper').val(this.qryStrUtils.getParameterByName('ldru', ''));
            $('#txtUpperDateRangeLower').val(this.qryStrUtils.getParameterByName('udrl', ''));
            $('#txtUpperDateRangeUpper').val(this.qryStrUtils.getParameterByName('udru', ''));

            $('#txtCountNo').val(this.qryStrUtils.getParameterByName('fcount', ''));
            $('#chkIsThackrayFound').val(this.qryStrUtils.getParameterByName('ist', ''));
            $('#chkIsCopyHeld').val(this.qryStrUtils.getParameterByName('isc', ''));
            $('#chkIsViewed').val(this.qryStrUtils.getParameterByName('isv', ''));
            $('#chkUseOptions').val(this.qryStrUtils.getParameterByName('isuo', ''));


            this.getSources();
        }



        var sourceTypeLookup = new SourceTypeLookup();
        sourceTypeLookup.init();

        var isPersonImpSelection = this.qryStrUtils.getParameterByName('scs', '');

        if (isPersonImpSelection !== null) {
            $("#rLink").removeClass("hidePanel").addClass("displayPanel");
        }
        else {
            $("#rLink").addClass("hidePanel").removeClass("displayPanel");
        }

    }
    ,
    getSources: function () {

        var params = {};
        params[0] = this.qryStrUtils.getParameterByName('stids', '');
        params[1] = $('#txtSourceRef').val();
        params[2] = $('#txtSourceDescription').val();
        params[3] = $('#txtOriginalLocation').val();
        params[4] = $('#txtLowerDateRangeLower').val();
        params[5] = $('#txtLowerDateRangeUpper').val();
        params[6] = $('#txtUpperDateRangeLower').val();
        params[7] = $('#txtUpperDateRangeUpper').val();
        params[8] = $('#txtCountNo').val();
        params[9] = 'false'; // $('#chkIsThackrayFound').val();
        params[10] = 'false'; // $('#chkIsCopyHeld').val();
        params[11] = 'false'; // $('#chkIsViewed').val();
        params[12] = 'false'; // $('#chkUseOptions').val();
        params[13] = String(this.qryStrUtils.getParameterByName('page', 0));
        params[14] = '30';
        params[15] = this.qryStrUtils.getParameterByName('sort_col', 'sdate');
    
        this.ancUtils.twaGetJSON('/Sources/Select', params, $.proxy(this.processData, this));

        this.createQryString();
        return false;
    },

    returnselection: function () {

        var parishLst = '';

        $.each(this.selection, function (idx, val) {
            if (idx > 0) {
                parishLst += ',' + val;
            }
            else {
                parishLst += val;
            }
        });

        this.qryStrUtils.updateQryPar('scs', parishLst);

        var sources = this.qryStrUtils.getParameterByName('scs', '');
        //dont lose these if they are there.
        var parishs = this.qryStrUtils.getParameterByName('parl', '');

        var _loc = '#?scs=' + sources + '&parl=' + parishs;

        var url = '../HtmlPages/batchEntry.html' + _loc;

        window.location.href = url;
    },


    createQryString: function () {

        var args = {
            "active": '1',
            "sref": $('#txtSourceRef'),
            "sdesc": $('#txtSourceDescription'),
            "origLoc": $('#txtOriginalLocation'),
            "ldrl": $('#txtLowerDateRangeLower'),
            "ldru": $('#txtLowerDateRangeUpper'),
            "udrl": $('#txtUpperDateRangeLower'),
            "udru": $('#txtUpperDateRangeUpper'),
            "fcount": $('#txtCountNo')
        };

        this.qryStrUtils.updateQry(args);

    },

    //// this can find out what page its on based on the
    //// returned data.
    processData: function (data) {
        //alert('received something');
        var tableBody = '';
        var selectEvents = [];
        var _idx = 0;
        var that = this;


        $.each(data.serviceSources, function (source, sourceInfo) {
            //<a href='' class="button" ><span>Main</span></a>
            var hidfield = '<input type="hidden" name="source_id" id="source_id" value ="' + sourceInfo.SourceId + '"/>';

            tableBody += '<tr>' + hidfield;
            tableBody += '<td><div>' + sourceInfo.SourceYear + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.SourceYearTo + '</div></td>';

            var _loc = window.location.hash;
            _loc = that.qryStrUtils.updateStrForQry(_loc, 'id', sourceInfo.SourceId);

            tableBody += '<td><a href="../HtmlPages/SourceEditor.html' + _loc + '"><div> Edit </div></a></td>';

            tableBody += '<td class = "sourceref" ><a  id= "s' + _idx + '" href="" ><div title="' + sourceInfo.SourceRef + '">' + sourceInfo.SourceRef + '</div></a></td>';

            selectEvents.push({ key: 's' + _idx, value: sourceInfo.SourceId });

            tableBody += '<td class = "source_d" ><div  title="' + sourceInfo.SourceDesc + '">' + sourceInfo.SourceDesc + '</div></td>';

            tableBody += '</tr>';
            _idx++;

        });

        if (tableBody !== '') {

            $('#search_bdy').html(tableBody);

            //create pager based on results

            $('#reccount').html(data.Total + ' Sources');

            // $('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));

            var pagerparams = { ParentElement: 'pager',
                Batch: data.Batch,
                BatchLength: data.BatchLength,
                Total: data.Total,
                Function: this.getLink,
                Context: this
            };

            this.ancUtils.createpager(pagerparams);
        }
        else {
            $('#search_bdy').html(tableBody);
            $('#reccount').html('0 Sources');
        }


        this.ancUtils.addlinks(selectEvents, this.processSelect, this);
    },
    processSelect: function (evt) {
        this.ancUtils.handleSelection(evt, this.selection, '#search_bdy tr', "#source_id");
    },
    sort: function (sort_col) {
        this.ancUtils.sort_inner(sort_col);
        this.getSources();
    },
    getLink: function (toPage) {
        this.qryStrUtils.updateQryPar('page', toPage);
        this.getSources();
    },
    addSource: function (path) {
        window.location.href = '../HtmlPages/SourceEditor.html#' + this.qryStrUtils.makeIdQryString('id', path);
    },
    deleteSources: function () {
        this.postParams.url = '/Sources/Delete';
        this.postParams.data = { sourceId: this.ancUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },
    printableSources: function () {
        //not implemented yet.
    }

}

