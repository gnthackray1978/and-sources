

//ok

$(document).ready(function () {



    createHeader('#1',getSourceType);
  
});

//ok
save = function () {
   // var sourceType = getSourceType();

    saveSourceType();

}


//ok
saveReturn = function () {

    twaPostJSON('/SourceTypes/Add', GetSourceTypeRecord(), '../HtmlPages/SourceTypesSearch.html', 'id');

    //    var localurl = getHost() + '/SourceTypes/Add';
    //    var stringy = JSON.stringify(GetSourceTypeRecord());

    //    $.ajax({
    //        cache: false,
    //        type: "POST",
    //        async: false,
    //        url: localurl,
    //        data: stringy,
    //        contentType: "application/json",
    //        dataType: "json",
    //        success: function (message) {

    //            var result = getValueFromKey(message, 'Id');

    //            updateQryPar('id', result);

    //            var error = getValueFromKey(message, 'error');

    //            if (error != '' && error != null) {
    //                showError(error);
    //            }
    //            else {
    //                var _hash = window.location.hash;
    //                window.location = '../HtmlPages/SourceTypesSearch.html' + _hash;
    //            }
    //        }
    //    });
}



saveSourceType = function () {

    twaPostJSON('/SourceTypes/Add', GetSourceTypeRecord(), '', 'id');

//    var localurl = getHost() + '/SourceTypes/Add';



//    var stringy = JSON.stringify(GetSourceTypeRecord());

//    $.ajax({
//        cache: false,
//        type: "POST",
//        async: false,
//        url: localurl,
//        data: stringy,
//        contentType: "application/json",
//        dataType: "json",
//        success: function (message) {

//            var result = getValueFromKey(message, 'Id');

//            updateQryPar('id', result);

//            var error = getValueFromKey(message, 'error');

//            if (error != '' && error != null) {
//                showError(error);
//            }


//        }

//    });

}

//ok
function getSourceType() {

    var params = {};

 //   var url = getHost() + "/SourceTypes/Id";

    var id = getParameterByName('id', '');

    params[0] = id;

//    $.ajaxSetup({ cache: false });
//    $.getJSON(url, params, processData);

    twaGetJSON("/SourceTypes/Id", params, processData);

    return false;
}



function processData(data) {

    // public string AddPerson(string personId, string birthparishId,string deathparishId, string referenceparishId, string ismale, string years, string months, string weeks, string days)

    $('#txtOrder').val(data.Order);
    $('#txtDescription').val(data.Description);
  
}



GetSourceTypeRecord = function (rowIdx) {
    var data = {};

    data.TypeId = getParameterByName('id', '');
    data.Description = $('#txtDescription').val();
    data.Order = $('#txtOrder').val();



    return data;
}