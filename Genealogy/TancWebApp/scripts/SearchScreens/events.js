var selection = new Array();
//var url = getHost() + "/Events/GetEvents/Select";



$(document).ready(function () {

    createHeader('#1', loaded);
});


function loaded() {

    var isActive = getParameterByName('active');

    if (isActive == '1') {
        $('#txtCName').val(getParameterByName('cname'));
        $('#txtSName').val(getParameterByName('sname'));
        $('#txtLocation').val(getParameterByName('locat'));
        $('#txtLowerDateRange').val(getParameterByName('lower_dat'));
        $('#txtUpperDateRange').val(getParameterByName('upper_dat'));

        if (getParameterByName('inc_par') == 'false') {
            $('#chkIncludeParents').prop('checked', false);
        }
        else {
            $('#chkIncludeParents').prop('checked', true);
        }

        if (getParameterByName('incb') == 'false') {
            $('#chkIncludeBirths').prop('checked', false);
        }
        else {
            $('#chkIncludeBirths').prop('checked', true);
        }

        if (getParameterByName('incd') == 'false') {
            $('#chkIncludeDeaths').prop('checked', false);
        }
        else {
            $('#chkIncludeDeaths').prop('checked', true);
        }

        if (getParameterByName('incm') == 'false') {
            $('#chkIncludeMarriages').prop('checked', false);
        }
        else {
            $('#chkIncludeMarriages').prop('checked', true);
        }

        if (getParameterByName('incw') == 'false') {
            $('#chkIncludeWitnesses').prop('checked', false);
        }
        else {
            $('#chkIncludeWitnesses').prop('checked', true);
        }

        if (getParameterByName('incs') == 'false') {
            $('#chkIncludeSpouses').prop('checked', false);
        }
        else {
            $('#chkIncludeSpouses').prop('checked', true);
        }

        if (getParameterByName('inc_ps') == 'false') {
            $('#chkIncludePersonWithSpouses').prop('checked', false);
        }
        else {
            $('#chkIncludePersonWithSpouses').prop('checked', true);
        }


        getEvents('1');


    }


}

function createQryString(page) {


    updateQryPar('active', '1');
    updateQryPar('cname', $('#txtCName').val());
    updateQryPar('sname', $('#txtSName').val());
    updateQryPar('locat', $('#txtLocation').val());
    updateQryPar('lower_dat', $('#txtLowerDateRange').val());
    updateQryPar('upper_dat', $('#txtUpperDateRange').val());


    updateQryPar('inc_par', $('#chkIncludeParents').prop('checked'));
    updateQryPar('incb', $('#chkIncludeBirths').prop('checked'));
    updateQryPar('incd', $('#chkIncludeDeaths').prop('checked'));
    updateQryPar('incm', $('#chkIncludeMarriages').prop('checked'));
    updateQryPar('incw', $('#chkIncludeWitnesses').prop('checked'));
    updateQryPar('incs', $('#chkIncludeSpouses').prop('checked'));
    updateQryPar('inc_ps', $('#chkIncludePersonWithSpouses').prop('checked'));


}
 


function getEvents(showdupes) {

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


    params[0] = String($('#chkIncludeBirths').prop('checked'));
    params[1] = String($('#chkIncludeDeaths').prop('checked'));
    params[2] = String($('#chkIncludeWitnesses').prop('checked'));
    params[3] = String($('#chkIncludeParents').prop('checked'));
    params[4] = String($('#chkIncludeMarriages').prop('checked'));
    params[5] = String($('#chkIncludeSpouses').prop('checked'));
    params[6] = String($('#chkIncludePersonWithSpouses').prop('checked'));  
    params[7] = String($('#txtCName').val());
    params[8] = String($('#txtSName').val());
    params[9] = String($('#txtLowerDateRange').val());
    params[10] = String($('#txtUpperDateRange').val());
    params[11] = String($('#txtLocation').val());


    params[12] = String(page);
    params[13] = '30';
    params[14] = sort_col;

    // $.ajaxSetup({ cache: false });
    // $.getJSON(url, params, processData);

    twaGetJSON('/Events/GetEvents/Select', params, processData);

    createQryString(page);

    return false;
}




function processData(data) {
    //alert('received something');
    var tableBody = '';

    $.each(data.serviceEvents, function (source, sourceInfo) {
        //<a href='' class="button" ><span>Main</span></a>
        var hidEvtId = '<input type="hidden" name="event_id" id="event_id" value ="' + sourceInfo.EventId + '"/>';
        var hidLnkId = '<input type="hidden" name="link_id" id="link_id" value ="' + sourceInfo.LinkId + '"/>';

        tableBody += '<tr>' + hidEvtId + hidLnkId;


        var _loc = window.location.hash + '&id=' + sourceInfo.LinkId;

       // _loc = _loc.replace('#', '');

        tableBody += '<td><div>' + sourceInfo.EventDate + '</div></td>';


        //person
        if (sourceInfo.LinkTypeId == 1) {
            tableBody += '<td><a href="../HtmlPages/PersonEditor.html' + _loc + '"><div>'+ sourceInfo.EventDescription+'</div></a></td>';
        }

        //marriage
        if (sourceInfo.LinkTypeId == 2) {
            tableBody += '<td><a href="../HtmlPages/MarriageEditor.html' + _loc + '"><div>' + sourceInfo.EventDescription + '</div></a></td>';
        }

        //source
        if (sourceInfo.LinkTypeId == 4) {
            tableBody += '<td><a href="../HtmlPages/SourceEditor.html' + _loc + '"><div>' + sourceInfo.EventDescription + '</div></a></td>';
        }

        tableBody += '<td><div>' + sourceInfo.EventChristianName + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.EventSurname + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.EventLocation + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.EventText + '</div></td>';


        tableBody += '</tr>';
    });

    if (tableBody != '') {

        $('#search_bdy').html(tableBody);
        //create pager based on results

        $('#reccount').html(data.Total + ' Events');

        $('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));
    }
    else {
        $('#search_bdy').html(tableBody);
        $('#reccount').html('0 Persons');
    }
}

function getLink(toPage) {

    updateQryPar('page', toPage);
    getEvents('1');

}

function sort(sort_col) {

    sort_inner(sort_col);


    getEvents('1');
}


