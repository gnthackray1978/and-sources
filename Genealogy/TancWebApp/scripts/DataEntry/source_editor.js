

$(document).ready(function () {



    createHeader('#1',getSource);
   
});


save = function () {
    var serviceSource = GetSourceRecord();

    saveSource(serviceSource);

}



saveReturn = function () {

//    var serviceSource = GetSourceRecord();
//    var localurl = getHost() + '/Sources/Add';

//    var stringy = JSON.stringify(serviceSource);

//    $.ajax({
//        cache: false,
//        type: "POST",
//        async: false,
//        url: localurl,
//        data: stringy,
//        contentType: "application/json",
//        dataType: "json",
//        success: function (message) {
//            handleReturnCodeWithReturn(message, '../HtmlPages/SourceSearch.html', 'id')            
//        }

//    });


    twaPostJSON('/Sources/Add', GetSourceRecord(), '../HtmlPages/SourceSearch.html', 'id');

}



saveSource = function (serviceSource) {

//    var localurl = getHost() + '/Sources/Add';

//    var stringy = JSON.stringify(serviceSource);

//    $.ajax({
//        cache: false,
//        type: "POST",
//        async: false,
//        url: localurl,
//        data: stringy,
//        contentType: "application/json",
//        dataType: "json",
//        success: function (message) {           
//            handleReturnCode(message, 'id');  
//        }

    //    });

    twaPostJSON('/Sources/Add', serviceSource, '', 'id');
}


function getSource() {

    var params = {};

   // var url = getHost() + "/Sources/GetSource";

    var id = getParameterByName('id', '');

    params[0] = id;

//    $.ajaxSetup({ cache: false });
//    $.getJSON(url, params, processData);

    twaGetJSON("/Sources/GetSource", params, processData);

    return false;
}


function processData(data) {

    // public string AddPerson(string personId, string birthparishId,string deathparishId, string referenceparishId, string ismale, string years, string months, string weeks, string days)

    $('#txtSourceRef').val(data.SourceRef);
    $('#txtSourceDescription').val(data.SourceDesc);

    $('#txtLowerDate').val(data.SourceDateStr);
    $('#txtUpperDate').val(data.SourceDateStrTo);
    $('#txtOriginalLoc').val(data.OriginalLocation);


    $('#txtNotRdOnly').val(data.SourceNotes);

    if (data.IsCopyHeld == false) {
        $('#chkIsCopyHeld').prop('checked', false);
    }
    else {
        $('#chkIsCopyHeld').prop('checked', true);
    }


    if (data.IsViewed == false) {
        $('#chkIsViewed').prop('checked', false);
    }
    else {
        $('#chkIsViewed').prop('checked', true);
    }


    if (data.IsThackrayFound == false) {
        $('#chkIsThackrayFound').prop('checked', false);
    }
    else {
        $('#chkIsThackrayFound').prop('checked', true);
    }

    updateQryPar('stypes', data.SourceTypes);

    updateQryPar('pids', data.Parishs);

    updateQryPar('fids', data.FileIds);

    getParishs('#parishselector');

    getSourceTypes('#sourcetypeselector');
}



GetSourceRecord = function (rowIdx) {
    var data = {};
    //data.personId = getParameterByName('id');

    //data.sources = getParameterByName('source_ids');

    data.sourceRef = $('#txtSourceRef').val();
    data.sourceDesc = $('#txtSourceDescription').val();
    data.sourceDateStr = $('#txtLowerDate').val();
    data.sourceDateStrTo = $('#txtUpperDate').val();
    data.sourceNotes =$('#txtNotes').val();
    data.originalLocation = $('#txtOriginalLoc').val();


    data.isCopyHeld = $('#chkIsCopyHeld').prop('checked');
    data.isViewed = $('#chkIsViewed').prop('checked');
    data.isThackrayFound = $('#chkIsThackrayFound').prop('checked');



    data.parishs = getParameterByName('pids', '');
    data.sourceTypes = getParameterByName('stypes', '');
    data.fileIds = getParameterByName('fids', '');

    return data;


}