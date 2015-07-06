



var AncSelectorSourceTypes = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.ancSelectorBase = new AncSelectorBase();

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
  