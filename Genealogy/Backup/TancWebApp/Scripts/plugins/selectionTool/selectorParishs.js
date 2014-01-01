


var AncSelectorParishs = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.ancSelectorBase = new AncSelectorBase()

    this.url = "/ParishService/GetParishs/Select"; // getHost() + 

    this.ancSelectorBase.context_data = {
        editorUrl: '../HtmlPages/ParishEditor.html',
        param_name: 'pids',
        selectionUrl: '/ParishService/GetParishNames',
        pagerfunction: this.getParishLink,   // this.pagerfunction = 'getLink';
        search_body: 'p_search_bdy',
        search_hed: 'p_search_hed',
        pager: 'p_pager',
        sourceRefId: 'txtParishRef',
        title: 'PARISHS',
        refreshMethod: this.init,
        parentContext: this,
        name: 'sp'
    };

}


AncSelectorParishs.prototype = {

    init: function(selectorid) {

        var params = {};

        if (selectorid != undefined) {
            this.ancSelectorBase.createOutline(selectorid)
        }

        var parishName = $('#' + this.ancSelectorBase.context_data.sourceRefId).val();

        if (parishName == '') parishName = '%';

        params[0] = '%';//deposited
        params[1] = parishName;
        params[2] = '%';//county
        params[3] = this.qryStrUtils.getParameterByName('p_page', 0);
        params[4] = '10';
        params[5] = 'ParishName'; //sort column

        var success = function (data) {
            var batch_data = {};

            batch_data.Batch = data.Batch;
            batch_data.BatchLength = data.BatchLength;
            batch_data.Total = data.Total;
            batch_data.rows = new Array();

            $.each(data.serviceParishs, function (source, sourceInfo) {
                var row0 = { id: sourceInfo.ParishId, ref: sourceInfo.ParishName };
                batch_data.rows.push(row0);
            });

            this.ancSelectorBase.createBody(batch_data);
        };

        this.ancUtils.twaGetJSON(this.url, params, $.proxy(success, this));

        return false;
    },

    getParishLink: function(toPage) {

        this.qryStrUtils.updateQryPar('p_page', toPage);
        this.init();

    }



}




//var rows = new Array();
// 

//function getParishLink(toPage) {

//    updateQryPar('p_page', toPage);
//    getParishs();

//}


//function getParishs(selectorid) {

//    var rows = new Array();
//    var param_name = 'pids';
//    var selectionUrl = '/ParishService/GetParishNames';
//    var editorUrl = '../HtmlPages/ParishEditor.html';
//    var sourceRefId = 'txtParishRef';
//    var url = "/ParishService/GetParishs/Select";
//    
//    var pagerfunction = 'getParishLink';
//    var getfunction = 'getParishs';

//    var search_hed = 'p_search_hed';
//    var search_body = 'p_search_bdy';

//    var pager = 'p_pager';



//    var params = {};

//    var page = getParameterByName('p_page');
//    if (!page || isNaN(page))
//        page = 0;


//    if (selectorid != undefined) {

//        var body = createOutline(getfunction,search_hed, search_body, sourceRefId, "PARISHS", pager);

//        $(selectorid).html(body);
//    }

//    var parishName = $('#' + sourceRefId).val();

//    if (parishName == '') parishName = '%';

//    params[0] = '%';//deposited
//    params[1] = parishName;
//    params[2] = '%';//county
//    params[3] = String(page);
//    params[4] = '10';
//    params[5] = 'ParishName'; //sort column


//   // $.ajaxSetup({ cache: false });



//   // $.getJSON(url, params, function (data) {
//    twaGetJSON(url, params, function (data) {

//        var batch_data = {};
//        batch_data.Batch = data.Batch;
//        batch_data.BatchLength = data.BatchLength;
//        batch_data.Total = data.Total;

//        $.each(data.serviceParishs, function (source, sourceInfo) {
//            var row0 = { id: sourceInfo.ParishId, ref: sourceInfo.ParishName };
//            rows.push(row0);
//        });

//        createBody(rows, batch_data, editorUrl, param_name, selectionUrl, pagerfunction, search_body, search_hed,pager);
//    });

//    return false;
//}

