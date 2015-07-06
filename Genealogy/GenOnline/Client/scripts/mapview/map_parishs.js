



var MapParishs = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.parishTypes = null;
    this.DEFAULT_PARISHDETAILS_URL = '/ParishService/GetParishDetails';
}



MapParishs.prototype = {
    typeLookup: function (typeid) {

        var retVal = '';

        $.each(this.parishTypes, function (source, sourceInfo) {
            if (typeid == sourceInfo.DataTypeId) {
                retVal = sourceInfo.Description;
            }
        });


        return retVal;
    },
    loadParish: function (locats, resultFunc) {
        // var url = getHost() + "/ParishService/GetParishsFromLocations";

        var params = {};

        if (locats == '') {
            params[0] = '53.957700,-1.082290,0.5,0.5';
        }
        else {
            params[0] = locats;
        }

        this.ancUtils.twaGetJSON(this.DEFAULT_PARISHDETAILS_URL, params, resultFunc);
    },

    loadParishTypes: function (data) {
        this.parishTypes = data;
    },
    generateRegisters: function (serviceParishRecords) {

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

        var that = this;
        $.each(serviceParishRecords, function (source, sourceInfo) {
            transTable += '<tr>';
            transTable += '<td class="td_range" scope="row">' + sourceInfo.StartYear + sourceInfo.EndYear + '</td>';
            transTable += '<td class="td_rec_type">' + that.typeLookup(sourceInfo.DataType) + '</td>';
            transTable += '<td class="td_dat_type">' + sourceInfo.ParishRecordType + '</td>';
            transTable += '</tr>';
        });


        transTable += '</table>';
        transTable += '</div>';
        transTable += '</td></tr>';
        transTable += '</tbody>';
        transTable += '</table>';

        return transTable;
    },
    generateTranscripts: function (serviceParishTranscripts) {

        var transTable = '';

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
    },
    initialize: function () {
        var latlng = new google.maps.LatLng(-34.397, 150.644);
        var myOptions = {
            zoom: 8,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("map_canvas"),
            myOptions);
    }
}
