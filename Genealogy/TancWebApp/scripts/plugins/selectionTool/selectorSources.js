

var AncSelectorSources = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.ancSelectorBase = new AncSelectorBase ()
    this.url = "/GetSources/Select"; // getHost() + 


    this.ancSelectorBase.context_data = {
        editorUrl : '../Forms/FrmEditSource.aspx',
        param_name :'source_ids',
        selectionUrl: '/Sources/GetSourceNames',
        pagerfunction :undefined,   // this.pagerfunction = 'getLink';
        search_body : 'search_bdy',
        search_hed:'search_hed',
        pager: 's_pager',
        sourceRefId: 'txtSourceRef',
        title: 'SOURCES',
        refreshMethod: this.getSources
    };

}

AncSelectorSources.prototype = {

    getSources: function (selectorid) {
        var params = {};

        if (selectorid != undefined) {
            this.ancSelectorBase.createOutline(selectorid)
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




//function getLink(toPage) {

//    updateQryPar('s_page', toPage);
//    getSources();

//}


//function getSources(selectorid) {

//    var rows = new Array();
//    var param_name = 'source_ids';
//    var selectionUrl = '/Sources/GetSourceNames';
//    var editorUrl = '../Forms/FrmEditSource.aspx';
//    var sourceRefId = 'txtSourceRef';
//    var url ="/GetSources/Select";// getHost() + 
//    var pagerfunction = 'getLink';
//    var getfunction = 'getSources';
//    var search_hed = 'search_hed';
//    var search_body = 'search_bdy';
//    var pager = 's_pager';



//    var params = {};

//    var page = getParameterByName('s_page');
//    if (!page || isNaN(page))
//        page = 0;


//    if (selectorid != undefined) {

//        var body = createOutline(getfunction,search_hed, search_body, sourceRefId, "SOURCES", pager);

//        $(selectorid).html(body);
//    }

//    params[0] = '0';

//    params[1] = $('#' + sourceRefId).val();
//    params[2] = ''; // desc
//    params[3] = ''; //orig loc
//    params[4] = 0; // $('#txtLowerDateRangeLower').val();
//    params[5] = 0; //  $('#txtLowerDateRangeUpper').val();
//    params[6] = 2000; // $('#txtUpperDateRangeLower').val();
//    params[7] = 2000; // $('#txtUpperDateRangeUpper').val();

//    params[8] = 0; // $('#txtCountNo').val();
//    params[9] = false; // $('#chkIsThackrayFound').val();
//    params[10] = false; //$('#chkIsCopyHeld').val();
//    params[11] = false; //$('#chkIsViewed').val();
//    params[12] = false; //$('#chkUseOptions').val();
//    params[13] = page;
//    params[14] = '10';




//   // $.ajaxSetup({ cache: false });


//    twaGetJSON(url, params, function (data) {
//   // $.getJSON(url, params, function (data) {

//        var batch_data = {};
//        batch_data.Batch = data.Batch;
//        batch_data.BatchLength = data.BatchLength;
//        batch_data.Total = data.Total;

//        $.each(data.serviceSources, function (source, sourceInfo) {
//            var row0 = { id: sourceInfo.SourceId, ref: sourceInfo.SourceRef };
//            rows.push(row0);

//        });

//        createBody(rows, batch_data, editorUrl, param_name, selectionUrl, pagerfunction, search_body,search_hed, pager);
//    });

//    return false;
//}

