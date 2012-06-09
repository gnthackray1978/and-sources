

var selection = new Array();
//var url = getHost() + "/Parishs/GetParishs/Select";



$(document).ready(function () {

    //   $('#1').html(tableBody);

    createHeader('#1');

    var isActive = getParameterByName('active');

    if (isActive == '1') {
        $('#txtDeposited').val(getParameterByName('dep'));
        $('#txtName').val(getParameterByName('name'));
        $('#txtCounty').val(getParameterByName('count'));


        getParishs('1');
    }

   
    var isPersonImpSelection = getParameterByName('parl');

    if (isPersonImpSelection != null) {
        $("#rLink").removeClass("hidePanel").addClass("displayPanel");
    }
    else {
        $("#rLink").addClass("hidePanel").removeClass("displayPanel");
    }

});


function returnselection() {

    var parl = getParameterByName('parl');
    var parishLst = '';

    $.each(selection, function (idx, val) {
        if (idx > 0) {
            parishLst += ',' + val;
        }
        else {
            parishLst += val;
        }
    });

    updateQryPar('parl', parishLst);



    var sources = getParameterByName('scs');
    //dont lose these if they are there.
    var parishs = getParameterByName('parl');


    var _loc = '#?scs=' + sources + '&parl=' + parishs;



  //  var _loc = window.location.hash; // +'&id=' + sourceInfo.PersonId;
   
    var url = '../HtmlPages/batchEntry.html' + _loc;

    window.location.href = url;
}


function createQryString(page) {


    updateQryPar('active', '1');
    updateQryPar('dep', $('#txtDeposited').val());
    updateQryPar('name', $('#txtName').val());
    updateQryPar('count', $('#txtCounty').val());




}


function getParishs(showdupes) {

    var params = {};

    var page = getParameterByName('page');
    var sort_col = getParameterByName('sort_col');
  
    if (!page || isNaN(page))
        page = 0;

    if (!sort_col || sort_col == '')
        sort_col = 'ParishName';


    params[0] = String($('#txtDeposited').val());
    params[1] = String($('#txtName').val());
    params[2] = String($('#txtCounty').val());
    params[3] = String(page);
    params[4] = '30';
    params[5] = sort_col;

    //$.ajaxSetup({ cache: false });
    //$.getJSON(url, params, processData);

    twaGetJSON('/Parishs/GetParishs/Select', params, processData);

    createQryString(page);

    return false;
}


function processData(data) {
    //alert('received something');
    var tableBody = '';

    $.each(data.serviceParishs, function (source, sourceInfo) {
        
        var hidPID = '<input type="hidden" name="ParishId" id="ParishId" value ="' + sourceInfo.ParishId + '"/>';
        var arIdx = jQuery.inArray(sourceInfo.ParishId, selection);

        if (arIdx >= 0) {
            tableBody += '<tr class = "highLightRow">' + hidPID + hidParID;
        }
        else {
            tableBody += '<tr>' + hidPID;
        }

        var _loc = window.location.hash;
        _loc = updateStrForQry(_loc, 'id', sourceInfo.ParishId);

        tableBody += '<td><a href="" onClick ="processSelect(\'' + sourceInfo.ParishId + '\');return false"><div>' + sourceInfo.ParishName + '</div></a></td>';
        tableBody += '<td><div>' + sourceInfo.ParishDeposited + '</div></td>';
        tableBody += '<td><a href="../HtmlPages/ParishEditor.html' + _loc + '"><div> Edit </div></a></td>';
        tableBody += '<td><div>' + sourceInfo.ParishParent + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.ParishStartYear + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.ParishEndYear + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.ParishCounty + '</div></td>';

        tableBody += '</tr>';
    });

    if (tableBody != '') {

        $('#search_bdy').html(tableBody);
        //create pager based on results

        $('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));

        $('#reccount').html(data.Total + ' Parishs');
    }
    else {

        $('#search_bdy').html(tableBody);
        $('#reccount').html('0 Parishs');
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

        var quantity = $this.find("#ParishId").val();
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
    getParishs('1');
}



function getLink(toPage) {

    updateQryPar('page', toPage);
    getParishs('1');

}


function DeleteRecord() {



 //   var localurl = getHost() + '/Parishs/Delete';
    var theData = {};

    theData.parishIds = convertToCSV(selection);

    twaPostJSON('/Parishs/Delete', theData, '', '', function (args) {
        refreshWithErrorHandler(getParishs, args);
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
//        success: function (message) {
//            refreshWithErrorHandler(getParishs, message);

//            var error = getValueFromKey(message, 'error');

//            if (error != '' && error != null) {
//                showError(error);
//            }
//            else {
//                getParishs('1');
//            }
//        }

//    });




}


function AddParish() {

    //  qry = window.location.search.substring(1);





   // _loc += '&id=00000000-0000-0000-0000-000000000000';
    // _loc = _loc.replace('#', '');

    updateQryPar('id', '00000000-0000-0000-0000-000000000000');

    var _loc = window.location.hash;
    
    var url = '../HtmlPages/ParishEditor.html' + _loc;

    window.location.href = url;
}
 