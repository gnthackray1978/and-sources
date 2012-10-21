


function loadParish(locats) {
    // var url = getHost() + "/Parishs/GetParishsFromLocations";

    var params = {};

    if (locats == '') {
        params[0] = '53.957700,-1.082290,0.5,0.5';
    }
    else {
        params[0] = locats;
    }

    twaGetJSON('/Parishs/GetParishsFromLocations', params, parishResults);
 
}



function parishResults(data) {

    var tableBody = '';
    var visibleRecords = new Array();


    var parishId = getParameterByName('pid', '');
    var parishName = getParameterByName('pname', '');
    var parishMarker = null;

    $.each(data, function (source, sourceInfo) {

        if (displayedMarker.indexOf(sourceInfo.ParishName) < 0) {

            var homeLatLng = new google.maps.LatLng(sourceInfo.ParishX, sourceInfo.ParishY);

            var image = '../Images/icons/32x32/church_symbol2.png';
            var marker = new google.maps.Marker({
                position: homeLatLng,
                map: map,
                icon: image,
                title: sourceInfo.ParishName
            });

            marker.set("id", sourceInfo.ParishId);

            if (sourceInfo.ParishId == parishId) {
                parishMarker = marker;
            }

            google.maps.event.addListener(marker, 'click', function () {

                createInfoWindowContent(sourceInfo.ParishId, sourceInfo.ParishName, marker);

            }); //end click

            markersArray.push(marker);
            displayedMarker.push(sourceInfo.ParishName);

        }

    });    //end each


    if (parishId != '' &&
            parishId != null &&
            parishMarker != null) {

        createInfoWindowContent(parishId, parishName, parishMarker);

    }

}






function locateParish() {

    var address = $('#txtLocation').val(); //  document.getElementById("address").value;

    address += ',' + $('#txtCounty').val();

    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            map.setZoom(14);
        } else {
            alert("Geocode was not successful for the following reason: " + status);
        }
    });

}





function getStatus() {
    var qryString = '';
    var i = 0;

    //    var byteAmount = unescape(encodeURIComponent(qryString)).length;

    //     var url = getHost() + "/Parishs/GetSearchResults";

    var params = {};
    params[0] = 'YORKSHIRE';
    params[1] = $('#txtDateLower').val();
    params[2] = $('#txtDateUpper').val();

    //        $.ajaxSetup({ cache: false });
    //        $.getJSON(url, params, processSearchResults);

    twaGetJSON('/Parishs/GetSearchResults', params, processSearchResults);
}


function processSearchResults(data) {

    var idx = 0;
    //  alert(data.length);

    $.each(data, function (source, sourceInfo) {
        // markersArray[idx]

        var markerIdx = 0;

        while (markerIdx < markersArray.length) {
            var _id = markersArray[markerIdx].get('id');

            if (_id == sourceInfo.ParishId) {
                // markersArray[markerIdx].icon = '../Images/icons/icon_church.gif';
                markersArray[markerIdx].setIcon('../Images/icons/icon_church.gif');

                break;
            }

            markerIdx++;
        }

        idx++;
    });



}


function initialize() {
    var latlng = new google.maps.LatLng(-34.397, 150.644);
    var myOptions = {
        zoom: 8,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    var map = new google.maps.Map(document.getElementById("map_canvas"),
        myOptions);
}




function loadParishTypes(data) {


    parishTypes = data;


    //parishTypes
}


function generateRegisters(serviceParishRecords) {

    var transTable = '';

    transTable += '<span class = "tab_title">Deposited at </span><span>xyz</span>';
    transTable += '<table class="tableone" summary="">';



    transTable += '<thead>';
    transTable += '<tr>';
    transTable += '<th class="th_range" scope="col">Data Range</th> ';
    transTable += '<th class="th_rec_type" scope="col">Record Type</th> ';
    transTable += '<th class="th_dat_type" scope="col">Data Type</th>';
    transTable += '</tr>';
    transTable += '</thead>';


    transTable += '<tbody>';


    transTable += '<tr><td colspan="3">';
    transTable += '<div class="innerb">';


    transTable += '<table class="tabletwo">';

    $.each(serviceParishRecords, function (source, sourceInfo) {
        transTable += '<tr>';
        transTable += '<td class="td_range" scope="row">' + sourceInfo.StartYear + sourceInfo.EndYear + '</td>';
        transTable += '<td class="td_rec_type">' + typeLookup(sourceInfo.DataType) + '</td>';
        transTable += '<td class="td_dat_type">' + sourceInfo.ParishRecordType + '</td>';
        transTable += '</tr>';
    });


    transTable += '</table>';
    transTable += '</div>';
    transTable += '</td></tr>';
    transTable += '</tbody>';
    transTable += '</table>';

    return transTable;
}


function generateTranscripts(serviceParishTranscripts) {

    var transTable = '';
    //        transTable += '<table border="1">';
    //        transTable += '<tr>';
    //        transTable += '<th>Transcripts</th>';
    //        transTable += '</tr>';

    transTable += '<table class="tableone" summary="">';

    transTable += '<thead>';
    transTable += '<tr>';
    transTable += '<th class="th1" scope="col">Transcripts</th> ';
    transTable += '</tr>';
    transTable += '</thead>';

    transTable += '<tbody>';
    transTable += '<tr><td colspan="3">';
    transTable += '<div class="innerb">';
    transTable += '<table class="tabletwo">';


    $.each(serviceParishTranscripts, function (source, sourceInfo) {
        transTable += '<tr>';
        transTable += '<th class="td1" scope="row">' + sourceInfo.ParishTranscriptRecord + '</th>';
        transTable += '</tr>';
    });


    transTable += '</table>';
    transTable += '</div>';
    transTable += '</td></tr>';
    transTable += '</tbody>';
    transTable += '</table>';


    return transTable;
}