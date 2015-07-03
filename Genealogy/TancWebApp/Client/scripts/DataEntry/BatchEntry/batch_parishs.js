
//var parishListURL = getHost() + "/ParishService/GetParishNames";


var BatchParishs = function () {

    this.DEFAULT_GETPARISHNAME_URL = '/ParishService/GetParishNames';

    this.ancUtils = new AncUtils();
    this.qryStrUtils = new QryStrUtils();

    this.parishparam = 'parl';
}


BatchParishs.prototype = {

    getParishLst : function () {

        var params = {};
        var scs = this.qryStrUtils.getParameterByName(this.parishparam, '');
        params[0] = scs;

        //   $.ajaxSetup({ cache: false });
        //   $.getJSON(parishListURL, params, processParishs);

        this.ancUtils.twaGetJSON(this.DEFAULT_GETPARISHNAME_URL, params, $.proxy(this.processParishs, this));
        return false;
    },


    processParishs : function (data) {
        var tableBody = '';
        var count = 0;
        tableBody += '<table class = "data_list">';

        $.each(data, function (source, sourceInfo) {
            count++;

            tableBody += '<tr><td class = "parish_data">' + sourceInfo + '</td></tr>';

        });

        tableBody += '</table>';
        $('#parishLst').html(tableBody);

    },



    getParishNamesFromForm: function () {

        var result = '';

        $('.parish_data').each(function (i) {

            result += this.outerText;

        });


        return result;
    },


    isValidParishs :function () {

        var parish_names = this.getParishNamesFromForm();

        if (parish_names == '')
            return false;
        else
            return true;

    }


}

