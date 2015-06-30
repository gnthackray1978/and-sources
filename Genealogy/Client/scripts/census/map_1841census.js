
var JSMaster, QryStrUtils, AncUtils, google;
 

$(document).ready(function () {
    
    var jsMaster = new JSMaster();
    var censusMap = new CensusMap();

    jsMaster.connectfacebook(function () {        

        censusMap.init();

    });

});

var CensusMap = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.infoWindows = [];
    this.zoomLevel = 0;
    this.map = null;

    var headersection = '';

    headersection += '<div id="fb-root">';
    headersection += '<fb:login-button autologoutlink="true" &nbsp;perms="email,user_birthday,status_update,publish_stream"></fb:login-button>';
    headersection += '</div>';

    $('#1').html(headersection);


    this.postParams = {

        url: '',
        data: '',
        idparam: 'id',
        refreshmethod: this.load,
        refreshArgs: undefined,
        Context: this
    };
};


CensusMap.prototype = {

    init: function (selectorid, readyfunction) {

        var latLng = new google.maps.LatLng(53.957700, -1.082290);
        //var homeLatLng = new google.maps.LatLng(53.957700, -1.082290);


        var mapDiv = document.getElementById('map_canvas');

        this.map = new google.maps.Map(mapDiv, {
            center: latLng,
            zoom: 12,
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

        this.setMapDetail();

        var params = {};



        this.ancUtils.twaGetJSON('/parishService/Get1841CensusPlaces', params, $.proxy(this.parishResults, this));

    },

    parishResults: function (data) {

       
        var that = this;

        $.each(data, function (source, sourceInfo) {
            var homeLatLng = new google.maps.LatLng(sourceInfo.LocX, sourceInfo.LocY);
            var image = '../Images/icons/32x32/church_symbol2.png';
            var marker = new google.maps.Marker({
                position: homeLatLng,
                map: that.map,
                icon: image,
                title: sourceInfo.PlaceName
            });

            marker.set("id", sourceInfo.ParishId);

            var showinfo = function () {
                that.createInfoWindowContent(sourceInfo.ParishId, marker);
            };

            google.maps.event.addListener(marker, 'click', $.proxy(showinfo, that)); //end click
        });    //end each
    },

    createInfoWindowContent: function (parishId, marker) {

        var params = {};
        params[0] = parishId;

        var infowindowloaded = 0;

        var processSource = function (result) {

            var headersection = '';
            headersection += '<div id="' + result[0].SourceId + '" class = "info_cont">';

            headersection += '<div id="panelA" class = "displayPanel">';
            headersection += '<div class = "census_inner">';
            headersection += this.generateDetail(result);
            headersection += '</div>';
            headersection += '</div>';

            headersection += '</div>'; //end container

            var infowindow = new google.maps.InfoWindow({
                content: headersection
            });

            infowindow.open(this.map, marker);
        };

        this.ancUtils.twaGetJSON('/Sources/Get1841CensusSources', params, $.proxy(processSource, this));

    },

    generateDetail: function (data) {

        var transTable = '';
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
    },

    setMapDetail: function () {
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

        var customMapType = new google.maps.StyledMapType(stylez, { name: 'Border View' });

        this.map.mapTypes.set('Border View', customMapType);

    }
}




