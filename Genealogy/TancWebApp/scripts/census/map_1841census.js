
 
 
 
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

    var latLng = new google.maps.LatLng(53.957700, -1.082290);
    var homeLatLng = new google.maps.LatLng(53.957700, -1.082290);




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

    var params = {};

    twaGetJSON('/Sources/Get1841CensusPlaces', params, parishResults);
};


function parishResults(data) {

    var tableBody = '';
   
    $.each(data, function (source, sourceInfo) {

        var homeLatLng = new google.maps.LatLng(sourceInfo.LocX, sourceInfo.LocY);

            var image = '../Images/icons/32x32/church_symbol2.png';
            var marker = new google.maps.Marker({
                position: homeLatLng,
                map: map,
                icon: image,
                title: sourceInfo.PlaceName
            });

            marker.set("id", sourceInfo.ParishId);
      
            google.maps.event.addListener(marker, 'click', function () {

                createInfoWindowContent(sourceInfo.ParishId,   marker);

            }); //end click

    });    //end each


}





function createInfoWindowContent(parishId, marker) {

    var params = {};
    params[0] = parishId;
 
    var infowindowloaded = 0;


    twaGetJSON('/Sources/Get1841CensusSources', params, function (result) {

 
            var headersection = '';
            headersection += '<div id="' + result[0].SourceId + '" class = "info_cont">';
 
            headersection += '<div id="panelA" class = "displayPanel">';
            headersection += '<div class = "census_inner">';
            headersection += generateDetail(result);
            headersection += '</div>';
            headersection += '</div>';

            headersection += '</div>'; //end container

            var infowindow = new google.maps.InfoWindow({
                content: headersection
            });
           
            infowindow.open(map, marker);
        });     

    }




    function generateDetail(data) {

        var transTable = '';

     //   transTable += '<span class = "tab_title">Deposited at </span><span>xyz</span>';
        transTable += '<table class="tableone" summary="">';



            transTable += '<thead>';
                transTable += '<tr>';
                transTable += '<th class="th_range" scope="col">' + data.length + ' Entries at ' + data[0].Civil_Parish + '</th> ';
                transTable += '</tr>';
            transTable += '</thead>';

            transTable += '<tbody>';


            transTable += '<tr><td colspan="1">';
            transTable += '<div class="census_innerb">';


            transTable += '<table class="tabletwo">';

            $.each(data, function (source, sourceInfo) {


                transTable += '<tr>';
                transTable += '<td class="census_entry">' + sourceInfo.Address + '</td>';
                transTable += '</tr>';


                $.each(sourceInfo.attachedPersons, function (source_i, sourceInfo_i) {
                    transTable += '<tr>';
                    transTable += '<td class="td_dat_type">' + sourceInfo_i.BirthYear + ' ' + sourceInfo_i.CName + ' ' + sourceInfo_i.SName + '</td>';
                    transTable += '</tr>';
                });

            });

            transTable += '</table>';



            transTable += '</div>';
            transTable += '</td></tr>';
            transTable += '</tbody>';
        transTable += '</table>';

        return transTable;
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




