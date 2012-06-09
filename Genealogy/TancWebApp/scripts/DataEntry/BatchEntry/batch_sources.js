
//var sourceTypesUrl = getHost() + "/Sources/GetSourceNames";



isValidSources = function () {

    var source_names = getSourcesNamesFromForm();



    if (source_names == '')
        return false;
    else
        return true;



    return true;
}

 getSourceLst = function() {

    var params = {};
    var scs = getParameterByName('scs');     
    params[0] = scs;
 
  //  $.ajaxSetup({ cache: false });
   // $.getJSON(sourceTypesUrl, params, processsourcetypes);
    twaGetJSON('/Sources/GetSourceNames', params, processsourcetypes);

    return false;
}


 processsourcetypes = function(data) {
    var tableBody = '';
    var count = 0;
    tableBody += '<table class = "data_list">';

    $.each(data, function (source, sourceInfo) {
        count++;

        tableBody += '<tr><td class = "source_data">' + sourceInfo + '</td></tr>';

    });

    tableBody += '</table>';
    $('#sourceLst').html(tableBody);

}


 getSourcesNamesFromForm = function() {

    var result = '';
    
    $('.source_data').each(function (i) {

        result += this.outerText;

    });


    return result;
}


