
var selection = new Array();
//var url = getHost() + "/GetPersons/Select";
var parishId = '';


Array.prototype.RemoveInvalid = function (selection) {

    var filteredArray = new Array();


    for (var si = 0; si < selection.length; si++) {

        for (var i = 0; i < this.length; i++) {
            if (this[i] == selection[si]) {
                filteredArray.push(this[i]);
                break;
            }
        }


    }


    return filteredArray;

}



$(document).ready(function () {

    createHeader('#1', imready);
});


function imready() {

    var isActive = getParameterByName('active', '');

    if (isActive == '1') {
        $('#txtCName').val(getParameterByName('cname', ''));
        $('#txtSName').val(getParameterByName('sname', ''));
        $('#txtFCName').val(getParameterByName('fcname', ''));
        $('#txtFSName').val(getParameterByName('fsname', ''));
        $('#txtMCName').val(getParameterByName('mcname', ''));
        $('#txtMSName').val(getParameterByName('msname', ''));
        $('#txtLocation').val(getParameterByName('locat', ''));
        $('#txtCounty').val(getParameterByName('count', ''));
        $('#txtLowerDateRangeLower').val(getParameterByName('ldrl', ''));
        $('#txtLowerDateRangeUpper').val(getParameterByName('ldru', ''));

        if (getParameterByName('inct', '') == 'false') {
            $('#chkIncludeTree').prop('checked', false);
        }
        else {
            $('#chkIncludeTree').prop('checked', true);
        }


        if (getParameterByName('incb', '') == 'false') {
            $('#chkIncludeBirths').prop('checked', false);
        }
        else {
            $('#chkIncludeBirths').prop('checked', true);
        }


        if (getParameterByName('incd', '') == 'false') {
            $('#chkIncludeDeaths').prop('checked', false);
        }
        else {
            $('#chkIncludeDeaths').prop('checked', true);
        }

        parishId = getParameterByName('parid', '');

        getPersons('1');
    }

}



function createQryString(page) {

    var workingQry = window.location.hash;

   // workingQry = updateStrForQry(workingQry,

//    updateQryPar('active', '1');
//    updateQryPar('cname', $('#txtCName').val());
//    updateQryPar('sname', $('#txtSName').val());
//    updateQryPar('fcname', $('#txtFCName').val());
//    updateQryPar('fsname', $('#txtFSName').val());
//    updateQryPar('mcname', $('#txtMCName').val());
//    updateQryPar('msname', $('#txtMSName').val());
//    updateQryPar('locat', $('#txtLocation').val());
//    updateQryPar('count', $('#txtCounty').val());
//    updateQryPar('ldrl', $('#txtLowerDateRangeLower').val());
//    updateQryPar('ldru', $('#txtLowerDateRangeUpper').val());

//    updateQryPar('inct', $('#chkIncludeTree').prop('checked'));
//    updateQryPar('incb', $('#chkIncludeBirths').prop('checked'));
//    updateQryPar('incd', $('#chkIncludeDeaths').prop('checked'));
//    updateQryPar('parid', parishId);

    workingQry = updateStrForQry(workingQry,'active', '1');
    workingQry = updateStrForQry(workingQry,'cname', $('#txtCName').val());
    workingQry = updateStrForQry(workingQry,'sname', $('#txtSName').val());
    workingQry = updateStrForQry(workingQry,'fcname', $('#txtFCName').val());
    workingQry = updateStrForQry(workingQry,'fsname', $('#txtFSName').val());
    workingQry = updateStrForQry(workingQry,'mcname', $('#txtMCName').val());
    workingQry = updateStrForQry(workingQry,'msname', $('#txtMSName').val());
    workingQry = updateStrForQry(workingQry,'locat', $('#txtLocation').val());
    workingQry = updateStrForQry(workingQry,'count', $('#txtCounty').val());
    workingQry = updateStrForQry(workingQry,'ldrl', $('#txtLowerDateRangeLower').val());
    workingQry = updateStrForQry(workingQry,'ldru', $('#txtLowerDateRangeUpper').val());

    workingQry = updateStrForQry(workingQry,'inct', $('#chkIncludeTree').prop('checked'));
    workingQry = updateStrForQry(workingQry,'incb', $('#chkIncludeBirths').prop('checked'));
    workingQry = updateStrForQry(workingQry,'incd', $('#chkIncludeDeaths').prop('checked'));
    workingQry = updateStrForQry(workingQry,'parid', parishId);
    
   window.location.hash =workingQry;
}

//string _parentId,
//string christianName,
//string surname,
//string fatherChristianName,
//string fatherSurname,
//string motherChristianName,
//string motherSurname,
//string location,
//string lowerDate,
//string upperDate,
//string filterTreeResults,
//string filterIncludeBirths,
//string filterIncludeDeaths,
//string page_number,
//string page_size);



function getPersons(showdupes) {

    //BirthInt
    //_parentId
    var params = {};

    var page = getParameterByName('page');
    var sort_col = getParameterByName('sort_col');


    var parentId = '';


    if (!page || isNaN(page))
        page = 0;

    if (!sort_col || sort_col == '')
        sort_col = 'BirthInt';

    if (showdupes == '0') {
        updateQryPar('_parentId', parentId);
    }
    else {
        parentId = getParameterByName('_parentId');
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
    params[16] = parishId;
    params[17] = String(page);
    params[18] = '30';
    params[19] = sort_col;

  //  $.ajaxSetup({ cache: false });
  //  $.getJSON(url, params, processData);

    twaGetJSON('/GetPersons/Select', params, processData);

    createQryString(page);

    return false;
}


function processData(data) {
    //alert('received something');
    var tableBody = '';
    var visibleRecords = new Array();


    $.each(data.servicePersons, function (source, sourceInfo) {
        //<a href='' class="button" ><span>Main</span></a>
        var hidPID = '<input type="hidden" name="person_id" id="person_id" value ="' + sourceInfo.PersonId + '"/>';
        var hidParID = '<input type="hidden" name="parent_id" id="parent_id" value ="' + sourceInfo.XREF + '"/>';

        var arIdx = jQuery.inArray(sourceInfo.PersonId, selection);

        if (arIdx >= 0) {
            tableBody += '<tr class = "highLightRow">' + hidPID + hidParID;
        }
        else {
            tableBody += '<tr>' + hidPID + hidParID;
        }


        //        var _loc = window.location.hash;// +'&id=' + sourceInfo.PersonId;
        //        var idParam = getParameterByName('id');
        //        if (idParam == null) {
        //            _loc += '&id=' + sourceInfo.PersonId;

        //        }
        //        else {
        //            idParam = 'id=' + idParam;
        //            _loc = _loc.replace(idParam, 'id=' + sourceInfo.PersonId);
        //        }

        var _loc = window.location.hash;
        _loc = updateStrForQry(_loc, 'id', sourceInfo.PersonId);



        // _loc = _loc.replace('#', '');

        tableBody += '<td><a href="" onClick ="loadDupes(\'' + sourceInfo.XREF + '\');return false"><div>' + sourceInfo.Events + '</div></a></td>';
        tableBody += '<td><a href="../HtmlPages/PersonEditor.html' + _loc + '"><div> Edit </div></a></td>';
        tableBody += '<td><div class = "dates" >' + sourceInfo.BirthYear + '-' + sourceInfo.DeathYear + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.BirthLocation + '</div></td>';

        tableBody += '<td><a href="" onClick ="processSelect(\'' + sourceInfo.PersonId + '\');return false"><div>' + sourceInfo.ChristianName + '</div></a></td>';

        tableBody += '<td><div>' + sourceInfo.Surname + '</div></td>';

        if(sourceInfo.Spouse == '')
            tableBody += '<td><div class = "parent">' + sourceInfo.FatherChristianName + '</div></td>';
        else
            tableBody += '<td><div class = "spouse">' + sourceInfo.Spouse + '</div></td>';
        

        tableBody += '<td><div>' + sourceInfo.MotherChristianName + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.MotherSurname + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.DeathLocation + '</div></td>';

        tableBody += '</tr>';

        visibleRecords.push(sourceInfo.PersonId);
    });

    if (selection != undefined) {
        selection = selection.RemoveInvalid(visibleRecords);
    }

    if (tableBody != '') {

        $('#search_bdy').html(tableBody);
        //create pager based on results

        $('#reccount').html(data.Total + ' Persons');

        $('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));
    }
    else {
        $('#search_bdy').html(tableBody);
        $('#reccount').html('0 Persons');
    }
}


function loadDupes(id) {
    updateQryPar('_parentId', id);
    getPersons('1');
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

        var quantity = $this.find("#person_id").val();
        arIdx = jQuery.inArray(quantity, selection);

        if (arIdx == -1) {
            $this.removeClass('highLightRow');
        }
        else {
            $this.addClass('highLightRow');
        }
    }); //end each

}

function refreshTable() {

    $('#search_bdy tr').each(function () {
        $this = $(this)

        var quantity = $this.find("#person_id").val();
        arIdx = jQuery.inArray(quantity, selection);

        if (arIdx == -1) {
            $this.removeClass('highLightRow');
        }
        else {
            $this.addClass('highLightRow');
        }
    }); //end each
}

function getLink(toPage) {

    updateQryPar('page', toPage);
    getPersons('1');

}


function sort(sort_col) {

    sort_inner(sort_col);


    getPersons('1');
}












function RefreshPersons(form) {
  //  console.log('hello there')
    //   javascript: window.open("http://www.microsoft.com");


//    var cName = $('[id*=txtCName]').val()
//    var sName = $('[id*=txtSName]').val()

//    var fcName = $('[id*=txtFCName]').val()
//    var fsName = $('[id*=txtFSName]').val()
//    var mcName = $('[id*=txtMCName]').val()
//    var msName = $('[id*=txtMSName]').val()

//    var loc = $('[id*=txtLocation]').val()

//    var cot = $('[id*=txtCounty]').val()

//    var ld = $('[id*=txtLowerDateRangeLower]').val()
//    var ud = $('[id*=txtLowerDateRangeUpper]').val()

//    var incTree = $('[id*=chkIncludeTree]').is(':checked')

//    var incBirths = $('[id*=chkIncludeBirths]').is(':checked')
//    var incDeaths = $('[id*=chkIncludeDeaths]').is(':checked')


//    var paVis = $("#pMain").attr('class');

//    var pbVis = $("#pMore").attr('class');

//    var qry = 'FilteredDeathsAndBirths.aspx?' + 'p=0&cn=' + cName + '&sn=' + sName + '&fc=' + fcName + '&fs=' + fsName + '&mc=' + mcName
//    + '&ms=' + msName + '&lc=' + loc + '&cn=' + cot + '&ld=' + ld + '&ud='
//    + ud + '&it=' + incTree + '&incb=' + incBirths + '&incd=' + incDeaths + '&pMain=' + paVis + '&pMore=' + pbVis;


//    //    if (txtBox!=null)
//    //console.log(qry)

//    if (cName != '' || sName != '' || fcName != '' || fsName != '' || mcName != '' || msName != '' || loc != '' || cot != '' || ld != '' || ud != '') {
//        window.location.href = qry
//    }


}

//$(document).ready(function () {



////    if (window.location.indexOf("FilteredDeathsAndBirths") != -1) {
//        qry = window.location.search.substring(1);

//        var paVis = getParameterByName('pMain');
//        var pbVis = getParameterByName('pMore');

//        if (paVis == 'displayPanel')
//            personsShowPanel(1);
//        if (pbVis == 'displayPanel')
//            personsShowPanel(2);
//   // }

//});


function addPerson(path) {


    window.location.href = '../HtmlPages/PersonEditor.html#' + makeIdQryString('id', path);
}






function DeleteRecord() {

   // var localurl = getHost() + '/Person/Delete';
    var theData = {};

    theData.personId = convertToCSV(selection);

    twaPostJSON('/Person/Delete', theData, '', '', function (args) {
        refreshWithErrorHandler(getPersons, args);
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
//            getPersons('1');
//        }

//    });

}

function PrintableResults() {


}




function AssignLocations() {

 //   var localurl = getHost() + '/Person/AssignLocats';
 //   $.ajaxSetup({ cache: false });
   // $.getJSON(url, '', function () { });


    twaGetJSON('/Person/AssignLocats', '', function () { });
}

function SetDuplicates() {

//    var localurl = getHost() + '/Person/SetDuplicate';

    var theData = {};

    theData.persons = convertToCSV(selection);
   
    twaPostJSON('/Person/SetDuplicate', theData, '', '', function (args) {
        refreshWithErrorHandler(getPersons, args);
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
//            getPersons('1');
//        }

//    });

}

function UpdateEstimates() {

//    var localurl = getHost() + '/Person/UpdateDates';

//    $.ajaxSetup({ cache: false });
//    $.getJSON(url, '', function () { });

    twaGetJSON('/Person/UpdateDates', '', function () { });
}


function SetRelation(relationid) {

   // var localurl = getHost() + '/Person/SetDuplicate';

    var theData = {};

    theData.persons = convertToCSV(selection);
    theData.relationType = relationid;

    var theData = {};

    theData.persons = convertToCSV(selection);

    twaPostJSON('/Person/SetDuplicate', theData, '', '', function (args) {
        refreshWithErrorHandler(getPersons, args);
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
//            getPersons('1');
//        }

//    });

}



function SetRemoveLink() {

    //var localurl = getHost() + '/Person/RemoveLinks';
    var theData = {};

    theData.person = convertToCSV(selection);

    twaPostJSON('/Person/RemoveLinks', theData, '', '', function (args) {
        refreshWithErrorHandler(getPersons, args);
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
//            getPersons('1');
//          //  refreshTable();
//        }

//    });


}

function SetMergeSources() {

  //  var localurl = getHost() + '/Person/MergePersons';
    var theData = {};

    theData.person = convertToCSV(selection);


    twaPostJSON('/Person/MergePersons', theData, '', '', function (args) {
        refreshWithErrorHandler(getPersons, args); });

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
//            getPersons('1');
//          //  refreshTable();
//        }

//    });


}

















