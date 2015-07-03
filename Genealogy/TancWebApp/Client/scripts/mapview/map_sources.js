


var MapSources = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();

    this.DEFAULT_SOURCEEDITOR_URL = '../HtmlPages/SourceEditor.html';
    this.DEFAULT_PERSONSEARCH_URL = '../HtmlPages/PersonSearch.html';
    this.DEFAULT_MARRIAGESEARCH_URL = '../HtmlPages/MarriageSearch.html';

}


MapSources.prototype = {

    loadSource: function(parishId, parishName, sourceId) {
        this.qryStrUtils.updateQryPar('pid', parishId);
        this.qryStrUtils.updateQryPar('pname', parishName);

    //        var _loc = window.location.hash + '&id=' + sourceInfo.SourceId;
    //        _loc = _loc.replace('#', '');

    //        tableBody += '<td><a href="../Forms/FrmEditSource.aspx' + _loc + '"><div> Edit </div></a></td>';

        window.location = this.DEFAULT_SOURCEEDITOR_URL + '?id=' + sourceId;

    },

    generateSources: function(serviceServiceMapDisplaySource, parishId, parishName, marriageCount, personCount) {

        var transTable = '';

        var qry = this.DEFAULT_PERSONSEARCH_URL +  '#?active=1&cname=&sname=thack&fcname=&fsname=&mcname=&msname=&locat=&count=&ldrl=1500&ldru=1900&inct=true&incb=false&incd=false&parid=' + parishId;
        var mqry = this.DEFAULT_MARRIAGESEARCH_URL + '#?active=1&mcname=&msname=&fcname=&fsname=&locat=&ldrl=1400&ldru=1900&parid=' + parishId;


        transTable += '<span class = "tab_title"><a href = "' + qry + '">' + personCount + ' Persons</a><span>  </span><a href = "' + mqry + '">' + marriageCount + ' Marriages</a>  </span>';

        transTable += '<table class="tableone" summary="">';

        transTable += '<thead>';
        transTable += '<tr>';
        transTable += '<th class="th_source_dr" scope="col">Data Range</th> ';
        transTable += '<th class="th_source_ref" scope="col">Source Ref.</th> ';
        transTable += '</tr>';
        transTable += '</thead>';

        transTable += '<tbody>';
        transTable += '<tr><td colspan="3">';
        transTable += '<div class="innerb">';
        transTable += '<table class="tabletwo">';


        $.each(serviceServiceMapDisplaySource, function (source, sourceInfo) {
            transTable += '<tr>';
            var clickfunc = "\"loadSource('" + parishId + "','" + parishName + "','" + sourceInfo.SourceId + "');return false\"";

            transTable += '<td class="td_source_dr" scope="row">' + sourceInfo.YearStart + sourceInfo.YearEnd + '</td>';
            transTable += '<td class="td_source_ref" scope="row"><a href = "" onClick = ' + clickfunc + '>' + sourceInfo.SourceRef + '</a></td>';
            transTable += '</tr>';
        });


        transTable += '</table>';
        transTable += '</div>';
        transTable += '</td></tr>';
        transTable += '</tbody>';
        transTable += '</table>';




        return transTable;
    }

}