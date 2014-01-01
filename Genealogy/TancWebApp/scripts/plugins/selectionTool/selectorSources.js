

var AncSelectorSources = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.ancSelectorBase = new AncSelectorBase ()
    this.url = "/Sources/Select"; // getHost() + 


    this.ancSelectorBase.context_data = {
        editorUrl : '../Forms/FrmEditSource.aspx',
        param_name :'source_ids',
        selectionUrl: '/Sources/GetSourceNames',
        pagerfunction :this.getLink,   // this.pagerfunction = 'getLink';
        search_body : 'search_bdy',
        search_hed:'search_hed',
        pager: 's_pager',
        sourceRefId: 'txtSourceRef',
        title: 'SOURCES',
        refreshMethod: this.getSources,
        parentContext: this,
        name: 'ss'

    };

}

AncSelectorSources.prototype = {

    getSources: function (selectorid) {
        var params = {};

        if (selectorid != undefined) {
            this.ancSelectorBase.createOutline(selectorid,this)
        }

        params[0] = '0';

        params[1] = $('#' + this.ancSelectorBase.context_data.sourceRefId).val();
        params[2] = ''; // desc
        params[3] = ''; //orig loc
        params[4] = 0; // $('#txtLowerDateRangeLower').val();
        params[5] = 0; //  $('#txtLowerDateRangeUpper').val();
        params[6] = 2000; // $('#txtUpperDateRangeLower').val();
        params[7] = 2000; // $('#txtUpperDateRangeUpper').val();

        params[8] = 0; // $('#txtCountNo').val();
        params[9] = false; // $('#chkIsThackrayFound').val();
        params[10] = false; //$('#chkIsCopyHeld').val();
        params[11] = false; //$('#chkIsViewed').val();
        params[12] = false; //$('#chkUseOptions').val();
        params[13] = this.qryStrUtils.getParameterByName('s_page', 0);
        params[14] = '10';

        var success = function (data) {
            var batch_data = {};
         
            batch_data.Batch = data.Batch;
            batch_data.BatchLength = data.BatchLength;
            batch_data.Total = data.Total;
            batch_data.rows = new Array();

            $.each(data.serviceSources, function (source, sourceInfo) {
                var row0 = { id: sourceInfo.SourceId, ref: sourceInfo.SourceRef };
                batch_data.rows.push(row0);
            });
          
            this.ancSelectorBase.createBody(batch_data);
        };

        this.ancUtils.twaGetJSON(this.url, params, $.proxy(success, this));
 
        return false;

    },

    getLink: function(toPage) {

        this.qryStrUtils.updateQryPar('s_page', toPage);
        this.getSources();

    }
}
 