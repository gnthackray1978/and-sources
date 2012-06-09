
function getParishLink(toPage) {

    updateQryPar('p_page', toPage);
    getParishs();

}


function getParishs(selectorid) {

    var rows = new Array();
    var param_name = 'pids';
    var selectionUrl = '/Parishs/GetParishNames';
    var editorUrl = '../HtmlPages/ParishEditor.html';
    var sourceRefId = 'txtParishRef';
    var url = "/Parishs/GetParishs/Select";
    var pagerfunction = 'getParishLink';

    var search_hed = 'p_search_hed';
    var search_body = 'p_search_bdy';
    var pager = 'p_pager';


    var params = {};

    var page = getParameterByName('p_page');
    if (!page || isNaN(page))
        page = 0;


    if (selectorid != undefined) {

        var body = createOutline(search_hed, search_body, sourceRefId, "PARISHS", pager);

        $(selectorid).html(body);
    }

    var parishName = $('#' + sourceRefId).val();

    if (parishName == '') parishName = '%';

    params[0] = '%';//deposited
    params[1] = parishName;
    params[2] = '%';//county
    params[3] = String(page);
    params[4] = '10';
    params[5] = 'ParishName'; //sort column


   // $.ajaxSetup({ cache: false });



   // $.getJSON(url, params, function (data) {
    twaGetJSON(url, params, function (data) {

        var batch_data = {};
        batch_data.Batch = data.Batch;
        batch_data.BatchLength = data.BatchLength;
        batch_data.Total = data.Total;

        $.each(data.serviceParishs, function (source, sourceInfo) {
            var row0 = { id: sourceInfo.ParishId, ref: sourceInfo.ParishName };
            rows.push(row0);
        });

        createBody(rows, batch_data, editorUrl, param_name, selectionUrl, pagerfunction, search_body, search_hed,pager);
    });

    return false;
}

