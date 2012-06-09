var selection = new Array();
//var url = getHost() + "/SourceTypes/Select";



$(document).ready(function () {

    //   $('#1').html(tableBody);

    createHeader('#1');

    var isActive = getParameterByName('active');

    if (isActive == '1') {
        $('#txtDescription').val(getParameterByName('stdesc'));
       

        getSourceTypes('1');
    }

});

function createQryString(page) {
    updateQryPar('active', '1');
    updateQryPar('stdesc', $('#txtDescription').val());
   
}


function getSourceTypes(showdupes) {

    var params = {};

    var page = getParameterByName('page');
    var sort_col = getParameterByName('sort_col');

    if (!page || isNaN(page))
        page = 0;

    if (!sort_col || sort_col == '')
        sort_col = 'SourceTypeDesc';


    params[0] = String($('#txtDescription').val()); 
    params[1] = String(page);
    params[2] = '30';
    params[3] = sort_col;

    //$.ajaxSetup({ cache: false });
   // $.getJSON(url, params, processData);

    twaGetJSON('/SourceTypes/Select', params, processData);

    createQryString(page);

    return false;
}


function processData(data) {
    //alert('received something');
    var tableBody = '';

    $.each(data.serviceSources, function (source, sourceInfo) {
        //<a href='' class="button" ><span>Main</span></a>
        var hidPID = '<input type="hidden" name="SourceTypeId" id="SourceTypeId" value ="' + sourceInfo.TypeId + '"/>';


        var arIdx = jQuery.inArray(sourceInfo.TypeId, selection);

        if (arIdx >= 0) {
            tableBody += '<tr class = "highLightRow">' + hidPID + hidParID;
        }
        else {
            tableBody += '<tr>' + hidPID;
        }

        var _loc = window.location.hash;
        _loc = updateStrForQry(_loc, 'id', sourceInfo.TypeId);

        tableBody += '<td><a href="" onClick ="processSelect(\'' + sourceInfo.TypeId + '\');return false"><div>' + sourceInfo.Description + '</div></a></td>';
        tableBody += '<td><div>' + sourceInfo.Order + '</div></td>';
        tableBody += '<td><a href="../HtmlPages/SourceTypeEditor.html' + _loc + '"><div> Edit </div></a></td>';

        tableBody += '</tr>';
    });

    if (tableBody != '') {

        $('#search_bdy').html(tableBody);
        //create pager based on results

        $('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));

        $('#reccount').html(data.Total + ' Source Types');
    }
    else {

        $('#search_bdy').html(tableBody);
        $('#reccount').html('0 Source Types');
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

        var quantity = $this.find("#SourceTypeId").val();
        arIdx = jQuery.inArray(quantity, selection);

        if (arIdx == -1) {
            $this.removeClass('highLightRow');
        }
        else {
            $this.addClass('highLightRow');
        }
    }); //end each

}

function sort(sort_col) {


    sort_inner(sort_col);
    getSourceTypes();
}



function getLink(toPage) {

    updateQryPar('page', toPage);
    getSourceTypes();

}



function DeleteRecord() {

    var localurl = getHost() + '/SourceTypes/Delete';
    var theData = {};

    theData.sourceIds = convertToCSV(selection);

    twaPostJSON('/SourceTypes/Delete', theData, '', '', function (args) {
        refreshWithErrorHandler(getSourceTypes, args);
    });

//    var stringy = JSON.stringify(theData);

//    $.ajax({
//        cache: false,
//        type: "POST",
//        async: false,
//        url: localurl,
//        data: stringy,
//        contentType: "application/json",
//        dataType: "json",
//        success: function (department) {
//            getSourceTypes();
//        }

//    });

}


function AddSourceType() {

    //  qry = window.location.search.substring(1);



    var _loc = window.location.hash
    _loc = _loc.replace('#', '');
    var url = '../HtmlPages/SourceTypeEditor.html#?id=0' + '&' + _loc;

    window.location.href = url;
}