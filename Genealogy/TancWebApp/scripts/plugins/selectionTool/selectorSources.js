
function getLink(toPage) {

    updateQryPar('s_page', toPage);
    getSources();

}


function getSources(selectorid) {

    var rows = new Array();
    var param_name = 'source_ids';
    var selectionUrl = '/Sources/GetSourceNames';
    var editorUrl = '../Forms/FrmEditSource.aspx';
    var sourceRefId = 'txtSourceRef';
    var url ="/GetSources/Select";// getHost() + 
    var pagerfunction = 'getLink';
    var getfunction = 'getSources';
    var search_hed = 'search_hed';
    var search_body = 'search_bdy';
    var pager = 's_pager';



    var params = {};

    var page = getParameterByName('s_page');
    if (!page || isNaN(page))
        page = 0;


    if (selectorid != undefined) {

        var body = createOutline(getfunction,search_hed, search_body, sourceRefId, "SOURCES", pager);

        $(selectorid).html(body);
    }

    params[0] = '0';

    params[1] = $('#' + sourceRefId).val();
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
    params[13] = page;
    params[14] = '10';




   // $.ajaxSetup({ cache: false });


    twaGetJSON(url, params, function (data) {
   // $.getJSON(url, params, function (data) {

        var batch_data = {};
        batch_data.Batch = data.Batch;
        batch_data.BatchLength = data.BatchLength;
        batch_data.Total = data.Total;

        $.each(data.serviceSources, function (source, sourceInfo) {
            var row0 = { id: sourceInfo.SourceId, ref: sourceInfo.SourceRef };
            rows.push(row0);

        });

        createBody(rows, batch_data, editorUrl, param_name, selectionUrl, pagerfunction, search_body,search_hed, pager);
    });

    return false;
}

