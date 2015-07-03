
//var sourceTypesUrl = getHost() + "/Sources/GetSourceNames";



var BatchSources = function () {

    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();

    this.sourceparam = 'scs';
    this.parishparam = 'parl';

    this.DEFAULT_SOURCEGETNAME_URL = '/Sources/GetSourceNames';
}



BatchSources.prototype = {

    isValidSources: function () {
    
        if (this.getSourcesNamesFromForm() == '')
            return false;
        else
            return true;
    
    },

    getSourceLst: function () {
        var params = {};
        var scs = this.qryStrUtils.getParameterByName(this.sourceparam, '');     
        params[0] = scs;
        this.ancUtils.twaGetJSON(this.DEFAULT_SOURCEGETNAME_URL, params, $.proxy(this.processsourcetypes, this));//processsourcetypes);
        return false;
    },

    processsourcetypes: function (data) {
        var tableBody = '';
        var count = 0;
        tableBody += '<table class = "data_list">';

        $.each(data, function (source, sourceInfo) {
            count++;

            tableBody += '<tr><td class = "source_data">' + sourceInfo + '</td></tr>';

        });

        tableBody += '</table>';
        $('#sourceLst').html(tableBody);

     },

     getSourcesNamesFromForm: function () {

        var result = '';
    
        $('.source_data').each(function (i) {

            result += this.outerText;

        });
        return result;
    }



}
