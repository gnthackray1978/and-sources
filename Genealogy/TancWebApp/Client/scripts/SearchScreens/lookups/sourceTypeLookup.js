

var SourceTypeLookup = function () {

    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.selectorTools = new SelectorTools();
    this.pager = new Pager();
    this.selection = new Array();
    this.DEFAULT_SELECT_URL = '/SourceTypes/Select';
}

SourceTypeLookup.prototype = {

    init: function () {
        
        var params = {};
        params[0] = '';
        params[1] = this.qryStrUtils.getParameterByName('stpage', '0');
        params[2] = 12;
        params[3] = 'SourceTypeOrder';

        this.ancUtils.twaGetJSON(this.DEFAULT_SELECT_URL, params, $.proxy(this.processsourcetypes, this));

    },
    processsourcetypes: function (data) {
        var tableBody = '';

        var selectEvents = new Array();
        var _idx = 0;


        $.each(data.serviceSources, function (source, sourceInfo) {

            var hidfield = '<input type="hidden" name="typeId" id="typeId" value ="' + sourceInfo.TypeId + '"/>';
            tableBody += '<tr>' + hidfield + '<td><a  id= "st' + _idx + '" href="" ><span>' + sourceInfo.Description + '</span></a></td></tr>';

            selectEvents.push({ key: 'st' + _idx, value: sourceInfo.TypeId });

            _idx++;
        });

        $('#sourcetype_lookup_body').html(tableBody);

        this.selectorTools.addlinks(selectEvents, this.selectTypes, this);

        var pagerparams = { ParentElement: 'filter_pager',
            Batch: data.Batch,
            BatchLength: 12,
            Total: data.Total,
            Function: this.getSourceLink,
            Context: this
        };

        this.pager.createpager(pagerparams);

        this.selectTypes();
    },

    getSourceLink: function (toPage) {

        this.qryStrUtils.updateQryPar('stpage', toPage);

        this.init();

    },

    selectTypes: function (stypeIds) {
   
        var stds = this.qryStrUtils.getParameterByName('stids', '');
   
        var stypeSelection = this.selectorTools.handleSelection(String(stypeIds), stds.split(','), '#sourcetype_lookup_body tr', "input");

        if (stypeSelection != undefined) {

            var tp = new Array();

            for (var i = 0; i < stypeSelection.length; i++) {
                if (stypeSelection[i] != undefined || stypeSelection[i] != "undefined") {
                    tp.push(stypeSelection[i]);
                   
                }
            }

            this.qryStrUtils.updateQryPar('stids', this.qryStrUtils.convertToCSV(tp));

            $('#selected_types').val(this.qryStrUtils.convertToCSV(tp));

        }

    }

}



