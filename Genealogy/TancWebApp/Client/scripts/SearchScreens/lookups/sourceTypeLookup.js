

var SourceTypeLookup = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.selection = new Array();
//    this.parishId = '';
// 
//    this.postParams = {
//        url: '',
//        data: '',
//        idparam: undefined,
//        refreshmethod: this.getSources,
//        refreshArgs: ['1'],
//        Context: this
//    };

}

SourceTypeLookup.prototype = {

    init: function () {
        //var pageName = this.qryStrUtils.getParameterByName('stpage', '0');

        var params = {};
        params[0] = '';
        params[1] = this.qryStrUtils.getParameterByName('stpage', '0');
        params[2] = 12;
        params[3] = 'SourceTypeOrder';

        this.ancUtils.twaGetJSON('/SourceTypes/Select', params, $.proxy(this.processsourcetypes, this));

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

        this.ancUtils.addlinks(selectEvents, this.selectTypes, this);

        var pagerparams = { ParentElement: 'filter_pager',
            Batch: data.Batch,
            BatchLength: 12,
            Total: data.Total,
            Function: this.getSourceLink,
            Context: this
        };

        this.ancUtils.createpager(pagerparams);

        this.selectTypes();
    },

    getSourceLink: function (toPage) {

        this.qryStrUtils.updateQryPar('stpage', toPage);

        this.init();

    },

    selectTypes: function (stypeIds) {
   
        var stds = this.qryStrUtils.getParameterByName('stids', '');
   
        var stypeSelection = this.ancUtils.handleSelection(String(stypeIds), stds.split(','), '#sourcetype_lookup_body tr', "input");

        if (stypeSelection != undefined) {

            var tp = new Array();

            for (var i = 0; i < stypeSelection.length; i++) {
                if (stypeSelection[i] != undefined || stypeSelection[i] != "undefined") {
                    tp.push(stypeSelection[i]);
                   
                }
            }

            this.qryStrUtils.updateQryPar('stids', this.ancUtils.convertToCSV(tp));

            $('#selected_types').val(this.ancUtils.convertToCSV(tp));

        }

    }




}


// this is called from parent controls
//function getSourceTypes() {

//    var pageName = getParameterByName('stpage', '');

//    if (!pageName) {
//        pageName = 0;         
//    }

//    twaGetJSON('/SourceTypes/Select', { 0: '', 1: pageName, 2: 12, 3: 'SourceTypeOrder' }, processsourcetypes);
//   
//}


//function processsourcetypes(data) {
//    var tableBody = '';
//    var count = 0;

//    $.each(data.serviceSources, function (source, sourceInfo) {
//        count++;
//        var hidfield = '<input type="hidden" name="typeId" id="typeId" value ="' + sourceInfo.TypeId + '"/>';
//        tableBody += '<tr>' + hidfield + '<td><a href="" onClick ="selectTypes(\'' + sourceInfo.TypeId + '\');return false"><span>' + sourceInfo.Description + '</span></a></td></tr>';
//    });


//    $('#sourcetype_lookup_body').html(tableBody);

//    $('#filter_pager').html(createpager(data.Batch, 12, data.Total, 'getSourceLink'));

//    selectTypes();
//}

//function getSourceLink(toPage) {

//    updateQryPar('stpage', toPage);

//    getSourceTypes(toPage);

//}


//function selectTypes(stypeIds) {


//    var stypeSelection = new Array();

// 
//    // get whatever is in the query string to start with
//    var selectedIds = getParameterByName('stids', '');

//    if(selectedIds != null)
//        stypeSelection = selectedIds.split(',');


//    if (stypeIds != undefined) {
//        // does the query string contain the type we want
//        var arIdx = jQuery.inArray(stypeIds, stypeSelection);

//        if (arIdx == -1) {//no
//            stypeSelection.push(stypeIds);
//        }
//        else {
//            stypeSelection.splice(arIdx, 1);
//        }
//    }

//    // at this point array should have the selection in it
//    $('#sourcetype_lookup_body tr').each(function () {
//        $this = $(this)

//        var quantity = $this.find("input").val();
//        arIdx = jQuery.inArray(quantity, stypeSelection);

//        if (arIdx == -1) {
//            $this.removeClass('highLightRow');
//        }
//        else {
//            $this.addClass('highLightRow');
//        }
//    }); //end each


//    updateQryPar('stids', convertToCSV(stypeSelection));
//    $('#selected_types').val(convertToCSV(stypeSelection));
//    
//}


