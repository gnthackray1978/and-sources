
//var sourceTypesUrl = getHost() + "/Sources/GetSourceNames";



var BatchSources = function () {
 
    this.sourceparam = 'scs';
    this.parishparam = 'parl';
}



BatchSources.prototype = {

    isValidSources: function () {
        var source_names = this.getSourcesNamesFromForm();
        if (source_names == '')
            return false;
        else
            return true;
        return true;
    }
    ,getSourceLst: function() {
        var params = {};
        var scs = AncUtils.getParameterByName(sourceparam, '');     
        params[0] = scs;
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



}
