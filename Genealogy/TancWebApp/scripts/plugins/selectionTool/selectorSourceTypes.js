
function getSourceTypeLink(toPage) {

    updateQryPar('st_page', toPage);
    getSourceTypes();

}


function getSourceTypes(selectorid) {

    var rows = new Array();
    var param_name = 'stypes';

    var selectionUrl = '/SourceTypes/GetNames';
    var editorUrl = '../HtmlPages/SourceTypeEditor.html';
    var sourceRefId = 'txtSourceTypeRef';
    var url = "/SourceTypes/Select";
    var pagerfunction = 'getSourceTypeLink';
    var search_hed = 'st_search_hed';
    var search_body = 'st_search_bdy';
    var pager = 'pager';


    var params = {};

    var page = getParameterByName('st_page');
    if (!page || isNaN(page))
        page = 0;


    if (selectorid != undefined) {

        var body = createOutline(search_hed, search_body, sourceRefId, "SOURCE TYPES",pager);

        $(selectorid).html(body);
    }

    var sourceTypeDesc = $('#' + sourceRefId).val();

    if (sourceTypeDesc == '') sourceTypeDesc = '%';

    params[0] = sourceTypeDesc
    params[1] = String(page);
    params[2] = '10';
    params[3] = 'SourceTypeDesc';

    $.ajaxSetup({ cache: false });



    //$.getJSON(url, params, function (data) {
    
    twaGetJSON(url, params, function (data) {

        var batch_data = {};
        batch_data.Batch = data.Batch;
        batch_data.BatchLength = data.BatchLength;
        batch_data.Total = data.Total;

        $.each(data.serviceSources, function (source, sourceInfo) {
            var row0 = { id: sourceInfo.TypeId, ref: sourceInfo.Description };
            rows.push(row0);
        });

        createBody(rows, batch_data, editorUrl, param_name, selectionUrl, pagerfunction, search_body,search_hed, pager);
    });

    return false;
}

