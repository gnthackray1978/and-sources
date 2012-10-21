
var setDefaultPersonUrl = getHost() + "/settreepersons/Set";
var saveSourceUrl = getHost() + "/SaveTree/Save";

var selection = new Array();
//var url = getHost() + "/GetSources/Select";

var deleteSourceUrl = getHost() + "/Source/Delete";





$(document).ready(function () {
 //   var monkey = 'test';
     createHeader('#1', imready);
});




function imready() {

    var isActive = getParameterByName('active');

    if (isActive == '1') {

        $('#txtSourceRef').val(getParameterByName('sref', ''));
        $('#txtSourceDescription').val(getParameterByName('sdesc', ''));
        $('#txtOriginalLocation').val(getParameterByName('origLoc', ''));

        $('#txtLowerDateRangeLower').val(getParameterByName('ldrl', ''));
        $('#txtLowerDateRangeUpper').val(getParameterByName('ldru', ''));
        $('#txtUpperDateRangeLower').val(getParameterByName('udrl', ''));
        $('#txtUpperDateRangeUpper').val(getParameterByName('udru', ''));

        $('#txtCountNo').val(getParameterByName('fcount', ''));
        $('#chkIsThackrayFound').val(getParameterByName('ist', ''));
        $('#chkIsCopyHeld').val(getParameterByName('isc', ''));
        $('#chkIsViewed').val(getParameterByName('isv', ''));
        $('#chkUseOptions').val(getParameterByName('isuo', ''));


        getSources();
    }

    getSourceTypes();


    var isPersonImpSelection = getParameterByName('scs', '');

    if (isPersonImpSelection != null) {
        $("#rLink").removeClass("hidePanel").addClass("displayPanel");
    }
    else {
        $("#rLink").addClass("hidePanel").removeClass("displayPanel");
    }

}




function returnselection() {


    var parishLst = '';

    $.each(selection, function (idx, val) {
        if (idx > 0) {
            parishLst += ',' + val;
        }
        else {
            parishLst += val;
        }
    });

    updateQryPar('scs', parishLst);





    var sources = getParameterByName('scs', '');
    //dont lose these if they are there.
    var parishs = getParameterByName('parl', '');


    var _loc = '#?scs=' + sources + '&parl=' + parishs;


    var url = '../HtmlPages/batchEntry.html' + _loc;

    window.location.href = url;
}


function sort(sort_col) {

    sort_inner(sort_col,'scol');
    getSources();
}





function getSources() {

    var params = {};



    var page = getParameterByName('page');

    if (!page || isNaN(page))
        page = 0;

    var sort_col = getParameterByName('scol');

    if (!sort_col )
        sort_col = 'date';

    var source_types = getParameterByName('stids', '');



    params[0] = source_types;  //$('#selected_types').val(); // sourcetypes
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
    params[13] = page;
    params[14] = '30';
    params[15] = sort_col;

  //  $.ajaxSetup({ cache: false });
  //  $.getJSON(url, params, processData);

    twaGetJSON('/GetSources/Select', params, processData);

    createQryString(page);

    return false;
}






function getLink(toPage) {

    updateQryPar('page', toPage);
    getSources();

}

// this can find out what page its on based on the
// returned data.
function processData(data) {
    //alert('received something');
    var tableBody = '';

    $.each(data.serviceSources, function (source, sourceInfo) {
        //<a href='' class="button" ><span>Main</span></a>
        var hidfield = '<input type="hidden" name="source_id" id="source_id" value ="' + sourceInfo.SourceId + '"/>';

        tableBody += '<tr>' + hidfield;
        tableBody += '<td><div>' + sourceInfo.SourceYear + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.SourceYearTo + '</div></td>';

       // var _loc = window.location.hash + '&id=' + sourceInfo.SourceId;
        //_loc = _loc.replace('#', '');


        var _loc = window.location.hash;
        _loc = updateStrForQry(_loc, 'id', sourceInfo.SourceId);

        tableBody += '<td><a href="../HtmlPages/SourceEditor.html' + _loc + '"><div> Edit </div></a></td>';

        tableBody += '<td class = "sourceref" ><a href="" onClick ="processSelect(\'' + sourceInfo.SourceId + '\');return false"><div title="' + sourceInfo.SourceRef + '">' + sourceInfo.SourceRef + '</div></a></td>';
        tableBody += '<td class = "source_d" ><div  title="'+ sourceInfo.SourceDesc +'">' + sourceInfo.SourceDesc + '</div></td>';

        tableBody += '</tr>';
    });

    if (tableBody != '') {

        $('#search_bdy').html(tableBody);

        //create pager based on results

        $('#reccount').html(data.Total + ' Sources');

        $('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));
    }
    else {
        $('#search_bdy').html(tableBody);
        $('#reccount').html('0 Sources');
    }


}




function processSelect(evt) {

    var arIdx = jQuery.inArray(evt, selection);

    if (arIdx == -1) {
        selection.push(evt);
    }
    else {
        selection.splice(arIdx, 1);
    }

    $('#search_bdy tr').each(function () {
        $this = $(this)

        var quantity = $this.find("input").val();
        arIdx = jQuery.inArray(quantity, selection);

        if (arIdx == -1) {
            $this.removeClass('highLightRow');
        }
        else {
            $this.addClass('highLightRow');
        }
    }); //end each

}


function RefreshSources(form) {


    var sref = $('[id*=txtSourceRef]').val()
    var sdesc = $('[id*=txtSourceDescription]').val()

    var origloc = $('[id*=txtOriginalLocation]').val()
    var ldrl = $('[id*=txtLowerDateRangeLower]').val()
    var ldru = $('[id*=txtLowerDateRangeUpper]').val()
    var udrl = $('[id*=txtUpperDateRangeLower]').val()
    var udru = $('[id*=txtUpperDateRangeUpper]').val()

    var isthac = $('[id*=chkIsThackrayFound]').is(':checked')

    var iscopy = $('[id*=chkIsCopyHeld]').is(':checked')
    var isview = $('[id*=chkIsViewed]').is(':checked')
    var isCheck = $('[id*=chkUseOptions]').is(':checked')

    var count = $('[id*=txtCountNo]').val()

    var stype = $("[id*=selectedTypes]").val()


    var qry = 'FilteredSources.aspx?' + 'p=0&sref=' + sref + '&sdesc=' + sdesc + '&origloc=' + origloc + '&ldrl=' + ldrl + '&ldru=' + ldru
    + '&udrl=' + udrl + '&udru=' + udru + '&isthac=' + isthac + '&iscopy=' + iscopy + '&isview=' + isview + '&isCheck=' + isCheck + '&count=' + count + '&stype=' + stype;


   

    if (sref != '' || sdesc != '' || origloc != '' || ldrl != '' || ldru != '' || udrl != '' || udru != '') {
        window.location.href = qry
    }


}

function RefreshSourcesTypes(form) {

    var stypedesc = $('[id*=txtDescription]').val()

    var qry = 'FilteredSourceTypes.aspx?' + 'p=0&stypedesc=' + stypedesc;

    if (stypedesc != '') {
        window.location.href = qry
    }
}

//function jsgetSrcTypeURL(path) {

//    qry = window.location.search.substring(1);

//    var url = 'FrmSourceTypeEditor.aspx?id=' + path + '&' + qry;

//    window.location.href = url;
//}



function addSource(path) {


    window.location.href = '../HtmlPages/SourceEditor.html#' + makeIdQryString('id', path);
}


//function jsgetSrcURL(path) {

//    qry = window.location.search.substring(1);

//    var url = '../HtmlPages/SourceEditor.html?id=' + path + '&' + qry;
//    /// <reference path="../../HtmlPages/SourceEditor.html" />

//    window.location.href = url;
//}

function deleteSources() {

    var theData = {};

    theData.sourceId = convertToCSV(selection);

    twaPostJSON('/Source/Delete', theData, '', '', function (args) {
        refreshWithErrorHandler(getSources, args);
    });

//    var stringy = JSON.stringify(theData);

//    $.ajax({
//        cache: false,
//        type: "POST",
//        async: false,
//        url: deleteSourceUrl,
//        data: stringy,
//        contentType: "application/json",
//        dataType: "json",
//        success: function (department) {
//            getSources();
//        }
//    });


}

function printableSources() {

}


function createQryString(page) {
    //window.location.hash = '?active=1&desc='+params[0]+'&page=' + page;
    // params[0] = ''; // sourcetypes

    //var sourcesPage = getParameterByName('page');
    //var sourceTypesPage = getParameterByName('stpage');

    updateQryPar('active', '1');
    updateQryPar('sref', $('#txtSourceRef').val());
    updateQryPar('sdesc', $('#txtSourceDescription').val());
    updateQryPar('origLoc', $('#txtOriginalLocation').val());
    updateQryPar('ldrl', $('#txtLowerDateRangeLower').val());
    updateQryPar('ldru', $('#txtLowerDateRangeUpper').val());
    updateQryPar('udrl', $('#txtUpperDateRangeLower').val());
    updateQryPar('udru', $('#txtUpperDateRangeUpper').val());
    updateQryPar('fcount', $('#txtCountNo').val());
    //    updateQryPar('ist', $('#chkIsThackrayFound').val());
    //    updateQryPar('isc', $('#chkIsCopyHeld').val());
    //    updateQryPar('isv', $('#chkIsViewed').val());
    //    updateQryPar('isuo', $('#chkUseOptions').val());

}