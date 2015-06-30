



var AncSelectorSourceTypes = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.ancSelectorBase = new AncSelectorBase()

    this.url = "/SourceTypes/Select"; // getHost() + 
 
    this.ancSelectorBase.context_data = {
        editorUrl: '../HtmlPages/SourceTypeEditor.html',
        param_name: 'stypes',
        selectionUrl: '/SourceTypes/GetNames',
        pagerfunction: this.getSourceTypeLink,   // this.pagerfunction = 'getLink';
        search_body: 'st_search_bdy',
        search_hed: 'st_search_hed',
        pager: 's_pager',
        sourceRefId: 'txtSourceTypeRef',
        title: 'SOURCE TYPES',
        refreshMethod: this.getSourceTypes,
        parentContext: this,
        name: 'st'
    };

}

AncSelectorSourceTypes.prototype = {

    getSourceTypes: function (selectorid) {
        var params = {};

        if (selectorid != undefined) {
            this.ancSelectorBase.createOutline(selectorid)
        }
        var sourceTypeDesc = $('#' + this.ancSelectorBase.context_data.sourceRefId).val();

        if (sourceTypeDesc == '') sourceTypeDesc = '%';

        params[0] = sourceTypeDesc
        params[1] = this.qryStrUtils.getParameterByName('st_page', 0);
        params[2] = '10';
        params[3] = 'SourceTypeDesc';

        $.ajaxSetup({ cache: false });


        var success = function (data) {
            var batch_data = {};

            batch_data.Batch = data.Batch;
            batch_data.BatchLength = data.BatchLength;
            batch_data.Total = data.Total;
            batch_data.rows = new Array();

            $.each(data.serviceSources, function (source, sourceInfo) {
                var row0 = { id: sourceInfo.TypeId, ref: sourceInfo.Description };
                batch_data.rows.push(row0);
            });

            this.ancSelectorBase.createBody(batch_data);
        };


        this.ancUtils.twaGetJSON(this.url, params, $.proxy(success, this));
        return false;
    },

    getSourceTypeLink: function (toPage) {

        this.qryStrUtils.updateQryPar('st_page', toPage);
        this.getSourceTypes();

    }
}
  
//function getSourceTypes(selectorid) {

//    var rows = new Array();
//    var param_name = 'stypes';

//    var selectionUrl = '/SourceTypes/GetNames';
//    var editorUrl = '../HtmlPages/SourceTypeEditor.html';
//    var sourceRefId = 'txtSourceTypeRef';
//    var url = "/SourceTypes/Select";
//    var pagerfunction = 'getSourceTypeLink';
//    var getfunction = 'getSourceTypes';
//    var search_hed = 'st_search_hed';
//    var search_body = 'st_search_bdy';
//    var pager = 'pager';


//    var params = {};

//    var page = getParameterByName('st_page');
//    if (!page || isNaN(page))
//        page = 0;


//    if (selectorid != undefined) {

//        var body = createOutline(getfunction,search_hed, search_body, sourceRefId, "SOURCE TYPES", pager);

//        $(selectorid).html(body);
//    }

//    var sourceTypeDesc = $('#' + sourceRefId).val();

//    if (sourceTypeDesc == '') sourceTypeDesc = '%';

//    params[0] = sourceTypeDesc
//    params[1] = String(page);
//    params[2] = '10';
//    params[3] = 'SourceTypeDesc';

//    $.ajaxSetup({ cache: false });



//    //$.getJSON(url, params, function (data) {
//    
//    twaGetJSON(url, params, function (data) {

//        var batch_data = {};
//        batch_data.Batch = data.Batch;
//        batch_data.BatchLength = data.BatchLength;
//        batch_data.Total = data.Total;

//        $.each(data.serviceSources, function (source, sourceInfo) {
//            var row0 = { id: sourceInfo.TypeId, ref: sourceInfo.Description };
//            rows.push(row0);
//        });

//        createBody(rows, batch_data, editorUrl, param_name, selectionUrl, pagerfunction, search_body,search_hed, pager);
//    });

//    return false;
//}

