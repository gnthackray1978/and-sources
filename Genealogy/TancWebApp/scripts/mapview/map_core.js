
var markersArray = [];

var parishTypes = null;
var downloadedArea = new Array();
var displayedMarker = new Array();
var infoWindows = new Array();


var zoomLevel = 0;
var map = null;

$(document).ready(function () {

    localCreate('#1', initMap);
});

 

function localCreate(selectorid, readyfunction) {

    facebookReady = readyfunction;

    var headersection = '';

    headersection += '<div id="fb-root">';
    headersection += '<fb:login-button autologoutlink="true" &nbsp;perms="email,user_birthday,status_update,publish_stream"></fb:login-button>';
    headersection += '</div>';

    $(selectorid).html(headersection);
}

function donothing() {


}





function initMap() {

    //        var latlng = new google.maps.LatLng(-34.397, 150.644);
    //        var myOptions = {
    //            zoom: 8,
    //            center: latlng,
    //            mapTypeId: google.maps.MapTypeId.ROADMAP
    //        };
    //        var map = new google.maps.Map(document.getElementById("map_canvas"),
    //        myOptions);



    var latLng = new google.maps.LatLng(53.957700, -1.082290);
    var homeLatLng = new google.maps.LatLng(53.957700, -1.082290);

    var parishId = getParameterByName('pid');
    var cx = getParameterByName('cx');
    var cy = getParameterByName('cy');

    if (cx != null && cy != null) {
        latLng = new google.maps.LatLng(cx, cy);
    }


    geocoder = new google.maps.Geocoder();
    var mapDiv = document.getElementById('map_canvas');

    map = new google.maps.Map(mapDiv, {
        center: latLng,
        zoom: 12
            ,
        mapTypeId: 'Border View',
        draggableCursor: 'pointer',
        draggingCursor: 'pointer',

        mapTypeControlOptions: { mapTypeIds: ['Border View'] },

        zoomControl: true,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.LARGE,
            position: google.maps.ControlPosition.TOP_RIGHT
        },

        panControl: true,
        panControlOptions: {
            position: google.maps.ControlPosition.TOP_RIGHT
        }
    });

    setMapDetail();

    //  var url = getHost() + "/Parishs/GetParishsFromLocations";

    var params = {};


    //        $.ajaxSetup({ cache: false });
    //        $.getJSON(getHost() + "/Parishs/GetParishsTypes", params, loadParishTypes);

    twaGetJSON('/Parishs/GetParishsTypes', params, loadParishTypes);

    google.maps.event.addListener(map, 'dragend', function () {
        DrawMap();
    });

    google.maps.event.addListener(map, 'zoom_changed', function () {
        DrawMap();
    });




    DrawMap();


};

function DrawMap() {

    // closer you get the higher the zoomlevel is


    zoomLevel = map.getZoom();

    if (zoomLevel < 12) {
        deleteOverlays();
    }
    else {
        var bounds = map.getCenter();

        var newLocation = MakeLocation(bounds.lat(), bounds.lng());

        if (newLocation != "") {
            loadParish(newLocation);
        }
    }

    //  trimMarkers();

}


function createInfoWindowContent(parishId, parishName, marker) {

    var params = {};
    params[0] = parishId;
    // $.ajaxSetup({ cache: false });

    //$.getJSON(getHost() + "/Parishs/GetParishDetails", params, function (result) {

    var infowindowloaded = 0;

    //basically cache what we've already downloaded
    // we have a array of downloaded info window content


    $.each(infoWindows, function (key, value) {

        // we have downloaded this one before
        if (value.pid == parishId) {
            if (value.isopen == 0) {
                infoWindows[key].isopen = 1;
                var infowindow = new google.maps.InfoWindow({
                    content: value.infowindow
                });

                infowindow.open(map, marker);
                
                var idx = key;

                google.maps.event.addListener(infowindow, 'closeclick', function () {
                    updateQryPar('pid', '');
                    infoWindows[idx].isopen = 0;

                });
            }
            infowindowloaded = 1;
        }

    });

    

    if (infowindowloaded ==0) {

        twaGetJSON('/Parishs/GetParishDetails', params, function (result) {

            //window.location.hash = sourceInfo.ParishId;

            //updateQryPar('pid', parishId);

            var bounds = map.getBounds();

            var centre = bounds.getCenter();
            //lat(), bounds.lng()


            updateQryPar('cx', centre.lat());
            updateQryPar('cy', centre.lng());
            updateQryPar('pid', parishId);

            var headersection = '';

            headersection += '<div id="' + parishId + '" class = "info_cont">';

            headersection += '<div class = "title">' + parishName + ' </div>';
            headersection += '<div class = "tabhed">';
            headersection += '<a href=\'\' onclick="masterShowTab(\'1\');return false" ><span>Transcripts</span></a>';
            headersection += '<a href=\'\' onclick="masterShowTab(\'2\');return false" ><span>Registers</span></a>';
            headersection += '<a href=\'\' onclick="masterShowTab(\'3\');return false" ><span>Sources</span></a>';
            headersection += '</div>';

            headersection += '<div id="panelA" class = "displayPanel">';
            headersection += '<div class = "mtrlnk">';
            headersection += generateTranscripts(result.serviceParishTranscripts);
            headersection += '</div>';
            headersection += '</div>';

            headersection += '<div id="panelB" class = "hidePanel">';
            headersection += '<div class = "mtrlnk">';
            headersection += generateRegisters(result.serviceParishRecords);
            headersection += '</div>';
            headersection += '</div>';

            headersection += '<div id="panelC" class = "hidePanel">';
            headersection += '<div class = "mtrlnk">';
            headersection += generateSources(result.serviceServiceMapDisplaySource, parishId, parishName, result.MarriageCount, result.PersonCount);
            headersection += '</div>';
            headersection += '</div>';
            headersection += '</div>'; //end container





            var infowindow = new google.maps.InfoWindow({
                content: headersection
            });

            //infowindow.
            infowindow.open(map, marker);

            var infowindowentry = {};

            infowindowentry.pid = parishId;
            infowindowentry.isopen = 1;
            infowindowentry.infowindow = headersection;


            var idx = infoWindows.push(infowindowentry);

            google.maps.event.addListener(infowindow, 'closeclick', function () {
                updateQryPar('pid', '');
                infoWindows[idx - 1].isopen = 0;
            });





            masterShowTab(1);


        });      //end     $.getJSON(url, params, function () {

}



}



Array.prototype.ContainsRec = function (_rec) {

    for (var i = 0; i < this.length; i++) {

        if (this[i].latx == _rec.latx &&
                 this[i].laty == _rec.laty &&
                 this[i].boxlen == _rec.boxlen) {
            return true;
        }
    }
    return false;

}


function typeLookup(typeid) {

    var retVal = '';

    $.each(parishTypes, function (source, sourceInfo) {
        if (typeid == sourceInfo.DataTypeId) {
            retVal = sourceInfo.Description;
        }
    });


    return retVal;
}

function MakeSquareCSV(_rec) {

    var locationString = "";

    for (var i = 0; i < _rec.length; i++) {
        locationString += "," + _rec[i].latx + "," + _rec[i].laty + "," + _rec[i].boxlen;
    }

    if (locationString != "")
        locationString = locationString.slice(1);

    return locationString;

}


function MakeLocation(latx, laty) {


    //List<RectangleD> newArea = new List<RectangleD>();
    var newArea = new Array();

    //double latx = 53.957700;
    // double laty = -1.082290;
    var XIdx = 0;
    var YIdx = 0;
    var xcount = 10;
    var ycount = 10;

    var boxlen = 0.1;



    latx = (Math.round(latx * 10) / 10) - boxlen * (xcount / 2);
    laty = (Math.round(laty * 10) / 10) - boxlen * (ycount / 2);


    var startx = latx;
    while (YIdx < ycount) {
        XIdx = 0;

        latx = startx;

        while (XIdx < xcount) {

            latx = Math.round(latx * 100) / 100
            laty = Math.round(laty * 100) / 100



            // RectangleD _rec = new RectangleD(latx, laty, boxlen, boxlen);

            var _rec = {
                latx: latx,
                laty: laty,
                boxlen: boxlen
            }


            if (!downloadedArea.ContainsRec(_rec)) {
                downloadedArea.push(_rec);
                newArea.push(_rec);
            }
            else {
                // console.log('downloaded');
            }
            latx += 0.1;
            XIdx++;
        }


        laty += 0.1;

        YIdx++;
    }

    //   Debug.WriteLine("areas count: " + downloadedArea.Count);
    return MakeSquareCSV(newArea);
}






function trimMarkers() {

    var bounds = map.getBounds();

    // this doesnt work!!!!
    // not called at the moment because it 
    // isnt necessary.


    if (bounds != undefined) {
        var nebounds = bounds.getNorthEast();
        var swbounds = bounds.getSouthWest();

        var bufferSize = 0.3;


        var leftbound = swbounds.Ra - bufferSize;
        var rightbound = nebounds.Ra + bufferSize;

        var topbound = nebounds.Qa + bufferSize;
        var bottombound = swbounds.Qa - bufferSize;





        if (markersArray) {

            var idx = 0;
            while (idx < markersArray.length) {

                var latlong = markersArray[idx].getPosition();

                if (markersArray[idx].labelContent == 'Rufforth') {
                    var test = 0;
                }

                if (latlong.Ra < leftbound || latlong.Ra > rightbound ||
                        latlong.Qa > topbound || latlong.Qa < bottombound) {
                    markersArray[idx].setMap(null);

                    var label = markersArray[idx].labelContent;
                    //remove relavent entry in displayedmarkers array
                    displayedMarker.splice(displayedMarker.indexOf(label), 1);
                    //remove marker entry in markers array
                    markersArray.splice(idx, 1);
                }
                else {
                    idx++;
                }
            }

        }



        if (downloadedArea) {
            // cycle through the downloaded areas
            // and remove any that we've got rid of

            //laty
            //50 top

            //40 bottom



            var idx = 0;
            while (idx < downloadedArea.length) {

                var blat = (downloadedArea[idx].laty - downloadedArea[idx].boxlen);
                var blng = (downloadedArea[idx].latx + downloadedArea[idx].boxlen);

                if (downloadedArea[idx].latx < leftbound || blng > rightbound || downloadedArea[idx].laty > topbound || blat < bottombound) {

                    downloadedArea.splice(idx, 1);
                }
                else {
                    idx++;
                }
            }

        }
    }
}

// Deletes all markers in the array by removing references to them
function deleteOverlays() {

    if (markersArray) {

        var idx = 0;
        while (idx < markersArray.length) {
            markersArray[idx].setMap(null);
            idx++;
        }

        markersArray.length = 0;
    }

    displayedMarker = new Array();
    downloadedArea = new Array();

}


function setMapDetail() {

    var styleOff = [{ visibility: 'off'}];
    var styleOn = [{ visibility: 'on'}];
    var stylez = [
          { featureType: 'administrative',
              elementType: 'labels',
              stylers: styleOn
          },
            { featureType: 'transit.line',
                elementType: 'geometry',
                stylers: styleOff
            },
            { featureType: 'transit.station.airport',
                elementType: 'geometry',
                stylers: styleOff
            },
          { featureType: 'administrative.province',
              stylers: styleOff
          },
          { featureType: 'administrative.locality',
              stylers: styleOn
          },
          { featureType: 'administrative.neighborhood',
              stylers: styleOn
          },

          { featureType: 'administrative.land_parcel',
              stylers: styleOn
          },

          { featureType: 'poi',
              stylers: styleOff
          },

          { featureType: 'landscape',
              stylers: styleOn
          },

          { featureType: 'landscape.natural',
              stylers: styleOn
          },

          { featureType: 'road',
              stylers: styleOn
          }
          ];


    var customMapType = new google.maps.StyledMapType(stylez,
            { name: 'Border View' });

    map.mapTypes.set('Border View', customMapType);





}




