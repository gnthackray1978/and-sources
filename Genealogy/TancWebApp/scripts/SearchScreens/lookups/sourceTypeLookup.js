
// this is called from parent controls
function getSourceTypes() {

    var pageName = getParameterByName('stpage');

    if (!pageName) {
        pageName = 0;         
    }

    twaGetJSON('/SourceTypes/Select', { 0: '', 1: pageName, 2: 12, 3: 'SourceTypeOrder' }, processsourcetypes);
   
}


function processsourcetypes(data) {
    var tableBody = '';
    var count = 0;

    $.each(data.serviceSources, function (source, sourceInfo) {
        count++;
        var hidfield = '<input type="hidden" name="typeId" id="typeId" value ="' + sourceInfo.TypeId + '"/>';
        tableBody += '<tr>' + hidfield + '<td><a href="" onClick ="selectTypes(\'' + sourceInfo.TypeId + '\');return false"><span>' + sourceInfo.Description + '</span></a></td></tr>';
    });


    $('#sourcetype_lookup_body').html(tableBody);

    $('#filter_pager').html(createpager(data.Batch, 12, data.Total, 'getSourceLink'));

    selectTypes();
}

function getSourceLink(toPage) {

    updateQryPar('stpage', toPage);

    getSourceTypes(toPage);

}


function selectTypes(stypeIds) {


    var stypeSelection = new Array();

 
    // get whatever is in the query string to start with
    var selectedIds = getParameterByName('stids');

    if(selectedIds != null)
        stypeSelection = selectedIds.split(',');


    if (stypeIds != undefined) {
        // does the query string contain the type we want
        var arIdx = jQuery.inArray(stypeIds, stypeSelection);

        if (arIdx == -1) {//no
            stypeSelection.push(stypeIds);
        }
        else {
            stypeSelection.splice(arIdx, 1);
        }
    }

    // at this point array should have the selection in it
    $('#sourcetype_lookup_body tr').each(function () {
        $this = $(this)

        var quantity = $this.find("input").val();
        arIdx = jQuery.inArray(quantity, stypeSelection);

        if (arIdx == -1) {
            $this.removeClass('highLightRow');
        }
        else {
            $this.addClass('highLightRow');
        }
    }); //end each


    updateQryPar('stids', convertToCSV(stypeSelection));
    $('#selected_types').val(convertToCSV(stypeSelection));
    
}


