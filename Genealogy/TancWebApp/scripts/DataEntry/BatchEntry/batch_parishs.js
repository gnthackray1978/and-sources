﻿
//var parishListURL = getHost() + "/Parishs/GetParishNames";



getParishLst = function () {

    var params = {};
    var scs = getParameterByName('parl');
    params[0] = scs;

    //   $.ajaxSetup({ cache: false });
    //   $.getJSON(parishListURL, params, processParishs);

    twaGetJSON("/Parishs/GetParishNames", params, processParishs);
    return false;
}


processParishs = function (data) {
    var tableBody = '';
    var count = 0;
    tableBody += '<table class = "data_list">';

    $.each(data, function (source, sourceInfo) {
        count++;

        tableBody += '<tr><td class = "parish_data">' + sourceInfo + '</td></tr>';

    });

    tableBody += '</table>';
    $('#parishLst').html(tableBody);

}



getParishNamesFromForm = function () {

    var result = '';

    $('.parish_data').each(function (i) {

        result += this.outerText;

    });


    return result;
}


isValidParishs = function () {

    var parish_names = getParishNamesFromForm();

    if (parish_names == '')
        return false;
    else
        return true;

}