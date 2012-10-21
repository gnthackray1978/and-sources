

//ok

$(document).ready(function () {



    createHeader('#1',getParish);
   
});

//ok
save = function () {
    // var sourceType = getParish();

    saveParish();

}


//ok
saveReturn = function () {

//    var localurl = getHost() + '/Parishs/Add';
//    var stringy = JSON.stringify(getParishRecord());

//    $.ajax({
//        cache: false,
//        type: "POST",
//        async: false,
//        url: localurl,
//        data: stringy,
//        contentType: "application/json",
//        dataType: "json",
//        success: function (message) {
//            handleReturnCodeWithReturn(message, '../HtmlPages/ParishSearch.html', 'id');
//        }
//    });


    twaPostJSON('/Parishs/Add', getParishRecord(), '../HtmlPages/ParishSearch.html', 'id');
}



saveParish = function () {

//    var localurl = getHost() + '/Parishs/Add';



//    var stringy = JSON.stringify(getParishRecord());

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

    twaPostJSON('/Parishs/Add', getParishRecord(), '', 'id');
}

//ok
function getParish() {

    var params = {};

  //  var url = getHost() + "/Parishs/GetParish";

    var id = getParameterByName('id', '');

    params[0] = id;

//    $.ajaxSetup({ cache: false });
//    $.getJSON(url, params, processData);


    twaGetJSON("/Parishs/GetParish", params, processData);

    return false;
}



function processData(data) {

//    // public string AddPerson(string personId, string birthparishId,string deathparishId, string referenceparishId, string ismale, string years, string months, string weeks, string days)
//            public Guid ParishId { get; set; }
  
//        public string ParishParent { get; set; }


        updateQryPar('id', data.ParishId);

        $('#txtParishName').val(data.ParishName);
        $('#txtParishCounty').val(data.ParishCounty);
        $('#txtParishDeposited').val(data.ParishDeposited);
        $('#txtParentParish').val(data.ParishParent);      
        $('#txtParStartYear').val(data.ParishStartYear);
        $('#txtParEndYear').val(data.ParishEndYear);
        $('#txtXLocat').val(data.ParishLat);
        $('#txtYLocat').val(data.ParishLong);
        $('#txtNotes').val(data.ParishNote);

}



getParishRecord = function (rowIdx) {
    var data = {};


    data.ParishId = getParameterByName('id', '');
    data.ParishName = $('#txtParishName').val();
    data.ParishCounty = $('#txtParishCounty').val();
    data.ParishDeposited = $('#txtParishDeposited').val();
    data.ParishParent = $('#txtParentParish').val();
    data.ParishStartYear = $('#txtParStartYear').val();
    data.ParishEndYear = $('#txtParEndYear').val();
    data.ParishLat = $('#txtXLocat').val();
    data.ParishLong = $('#txtYLocat').val();
    data.ParishNote = $('#txtNotes').val();

    return data;
}