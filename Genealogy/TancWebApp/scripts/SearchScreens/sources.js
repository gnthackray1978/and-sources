

$(document).ready(function () {
    var jsMaster = new JSMaster();
    var ancSources = new AncSources();

    jsMaster.generateHeader('#1', function () {
        ancSources.init();

    });

});


var AncSources = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.selection = new Array();
    this.parishId = '';
//var setDefaultPersonUrl = getHost() + "/settreepersons/Set";
//var saveSourceUrl = getHost() + "/SaveTree/Save";

//var deleteSourceUrl = getHost() + "/Source/Delete";

    this.postParams = { 
        url: '',
        data: '',
        idparam: undefined,
        refreshmethod: this.getSources,
        refreshArgs: ['1'],
        Context: this
    };
 
}

AncMarriages.prototype = {

    init: function() {

         var isActive = this.qryStrUtils.getParameterByName('active');

         var panels = new Panels();

         $("#main").live("click", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
         $("#more").live("click", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
         $("#additional").live("click", $.proxy(function () { panels.sourcesShowPanel('3'); return false; }, panels));
         $("#refresh").live("click", $.proxy(function () { this.getSources(); return false; }, this));


         $("#add").live("click", $.proxy(function () { this.addSource('00000000-0000-0000-0000-000000000000'); return false; }, this));
         $("#delete").live("click", $.proxy(function () { this.deleteSources(); return false; }, this));
         $("#print").live("click", $.proxy(function () { this.printableSources(); return false; }, this));
         $("#select_return").live("click", $.proxy(function () { this.returnselection(); return false; }, this));

         $("#sdate").live("click", $.proxy(function () { this.sort("SourceDate"); return false; }, this));
         $("#sref").live("click", $.proxy(function () { this.sort("SourceRef"); return false; }, this));
         $("#sdesc").live("click", $.proxy(function () { this.sort("SourceDescription"); return false; }, this));



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

        this.getSourceTypes();


        var isPersonImpSelection = this.qryStrUtils.getParameterByName('scs', '');

        if (isPersonImpSelection != null) {
            $("#rLink").removeClass("hidePanel").addClass("displayPanel");
        }
        else {
            $("#rLink").addClass("hidePanel").removeClass("displayPanel");
        }

    },

    getSources: function() {

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
        params[9] = 'false';// $('#chkIsThackrayFound').val();
        params[10] = 'false';// $('#chkIsCopyHeld').val();
        params[11] = 'false';// $('#chkIsViewed').val();
        params[12] = 'false';// $('#chkUseOptions').val();
        params[13] = String(this.qryStrUtils.getParameterByName('page', 0));
        params[14] = '30';
        params[15] = this.qryStrUtils.getParameterByName('sort_col', 'sdate'); 
        this.ancUtils.twaGetJSON('/GetSources/Select', params, $.proxy(this.processData, this));
               
        this.createQryString(page);
        return false;
    },

    returnselection: function() {

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


    createQryString: function(page) {
 
        var args = {
            "active": '1',
            "sref": $('#txtSourceRef'),
            "sdesc": $('#txtSourceDescription'),
            "origLoc": $('#txtOriginalLocation'),
            "ldrl": $('#txtLowerDateRangeLower'),
            "ldru": $('#txtLowerDateRangeUpper'),
            "udrl": $('#txtUpperDateRangeLower'),
            "udru": $('#txtUpperDateRangeUpper'),
            "fcount": $('#txtCountNo'
        };

        this.qryStrUtils.updateQry(args);

    },

    //// this can find out what page its on based on the
    //// returned data.
    processData: function(data) {
        //alert('received something');
        var tableBody = '';
        var selectEvents = new Array();
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

            tableBody += '<td class = "source_d" ><div  title="'+ sourceInfo.SourceDesc +'">' + sourceInfo.SourceDesc + '</div></td>';

            tableBody += '</tr>';
            _idx++;

        });

        if (tableBody != '') {

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
    }




}








//$(document).ready(function () {
// //   var monkey = 'test';
//     createHeader('#1', imready);
//});




//function sort(sort_col) {

//    sort_inner(sort_col,'scol');
//    getSources();
//}


//function getLink(toPage) {

//    updateQryPar('page', toPage);
//    getSources();

//}






//function processSelect(evt) {

//    var arIdx = jQuery.inArray(evt, selection);

//    if (arIdx == -1) {
//        selection.push(evt);
//    }
//    else {
//        selection.splice(arIdx, 1);
//    }

//    $('#search_bdy tr').each(function () {
//        $this = $(this)

//        var quantity = $this.find("input").val();
//        arIdx = jQuery.inArray(quantity, selection);

//        if (arIdx == -1) {
//            $this.removeClass('highLightRow');
//        }
//        else {
//            $this.addClass('highLightRow');
//        }
//    }); //end each

//}


//function RefreshSources(form) {


//    var sref = $('[id*=txtSourceRef]').val()
//    var sdesc = $('[id*=txtSourceDescription]').val()

//    var origloc = $('[id*=txtOriginalLocation]').val()
//    var ldrl = $('[id*=txtLowerDateRangeLower]').val()
//    var ldru = $('[id*=txtLowerDateRangeUpper]').val()
//    var udrl = $('[id*=txtUpperDateRangeLower]').val()
//    var udru = $('[id*=txtUpperDateRangeUpper]').val()

//    var isthac = $('[id*=chkIsThackrayFound]').is(':checked')

//    var iscopy = $('[id*=chkIsCopyHeld]').is(':checked')
//    var isview = $('[id*=chkIsViewed]').is(':checked')
//    var isCheck = $('[id*=chkUseOptions]').is(':checked')

//    var count = $('[id*=txtCountNo]').val()

//    var stype = $("[id*=selectedTypes]").val()


//    var qry = 'FilteredSources.aspx?' + 'p=0&sref=' + sref + '&sdesc=' + sdesc + '&origloc=' + origloc + '&ldrl=' + ldrl + '&ldru=' + ldru
//    + '&udrl=' + udrl + '&udru=' + udru + '&isthac=' + isthac + '&iscopy=' + iscopy + '&isview=' + isview + '&isCheck=' + isCheck + '&count=' + count + '&stype=' + stype;


//   

//    if (sref != '' || sdesc != '' || origloc != '' || ldrl != '' || ldru != '' || udrl != '' || udru != '') {
//        window.location.href = qry
//    }


//}

//function RefreshSourcesTypes(form) {

//    var stypedesc = $('[id*=txtDescription]').val()

//    var qry = 'FilteredSourceTypes.aspx?' + 'p=0&stypedesc=' + stypedesc;

//    if (stypedesc != '') {
//        window.location.href = qry
//    }
//}

////function jsgetSrcTypeURL(path) {

////    qry = window.location.search.substring(1);

////    var url = 'FrmSourceTypeEditor.aspx?id=' + path + '&' + qry;

////    window.location.href = url;
////}



//function addSource(path) {


//    window.location.href = '../HtmlPages/SourceEditor.html#' + makeIdQryString('id', path);
//}


////function jsgetSrcURL(path) {

////    qry = window.location.search.substring(1);

////    var url = '../HtmlPages/SourceEditor.html?id=' + path + '&' + qry;
////    /// <reference path="../../HtmlPages/SourceEditor.html" />

////    window.location.href = url;
////}

//function deleteSources() {

//    var theData = {};

//    theData.sourceId = convertToCSV(selection);

//    twaPostJSON('/Source/Delete', theData, '', '', function (args) {
//        refreshWithErrorHandler(getSources, args);
//    });

////    var stringy = JSON.stringify(theData);

////    $.ajax({
////        cache: false,
////        type: "POST",
////        async: false,
////        url: deleteSourceUrl,
////        data: stringy,
////        contentType: "application/json",
////        dataType: "json",
////        success: function (department) {
////            getSources();
////        }
////    });


//}

//function printableSources() {

//}

