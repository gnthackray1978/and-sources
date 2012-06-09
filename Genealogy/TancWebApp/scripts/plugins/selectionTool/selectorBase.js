


function createOutline(selectionbodyid, resultsbodyid, searchInputId, title, pager) {
    var body = '';

    body += '<div class = "sf_shad sf_cont">';

    body += '<div class = "sf_hed"><div><b>' + title + '</b></div></div>';

    body += '<div class = "sf_pad-cont">';
    body += '<table border="0">';
    body += '<thead>';
    body += '<tr class="tophed">';
    body += '<td><a href="../Default.aspx" onclick = "return false" ><span>Ref</span></a></td>';
    body += '<td></td>';
    body += '</tr>';
    body += '</thead>';

    body += '<tbody id = "' + selectionbodyid + '">';
    body += '</tbody>';
    body += '</table>';

    body += '<br />';

    body += '<div class = "sf_bot"><div><b>Select From</b></div></div>';

    body += '<div class="sf_mid-lab">';
    body += '<div class="sf_mlab-dlef">';
    body += '<input id="' + searchInputId + '" class = "sf_txt" type="text"/>';
    body += '</div>';


    body += '<div class="sf_mlab-drig">';
    body += '<a href="" onclick = "getSources();return false" class="button" ><span>R</span></a>';
    body += '</div>';
    body += '</div>';

    body += '<br /><br />';

    body += '<div style="overflow:auto;height:300px;">';

    body += '<table >'; //class="pink"
    body += '<thead>';
    body += '<tr class="bothed" >'; //class="hed"
    body += '<td><a href="../Default.aspx" onclick = "return false" ><span>Ref</span></a></td>';
    body += '<td></td>';
    body += '</tr>';
    body += '</thead>';
    body += '<tbody id = "' + resultsbodyid + '">';
    body += '</tbody>';
    body += '</table>';

    body += '</div>';


    body += '<div class="fltbrow">';
    body += '<div id="' + pager + '" class = "pager"></div>';

    body += '</div>';

    body += '</div>';

    body += '</div>';


    return body;

}



function refreshSelected(param_name, selectionUrl, search_hed) {

    var newSources = getNewSources(param_name, search_hed);

    if (newSources.length > 0) {
        var params = {};
        params[0] = convertToCSV(newSources);
        //  var sourceTypesUrl = getHost() + "/Sources/GetSourceNames";
        //var sourceTypesUrl = getHost() + selectionUrl;
        //$.ajaxSetup({ cache: false });

        twaGetJSON(selectionUrl, params, function (data) {
        //$.getJSON(sourceTypesUrl, params, function (data) {

            var rows = new Array();
            $.each(data, function (source, sourceInfo) {
                var row0 = { desc: sourceInfo };
                rows.push(row0);
            });

            addNewSelectedSources(data, param_name,search_hed);


        });
    }
}

// sourceid= selection to add, param_name = name of param in qry string 
// and selection url is json service of selected items
function addToSelection(sourceId, param_name, selectionUrl, search_hed) {
    var selectedSourceIds = new Array();
    //var param_name = 'source_ids';

    var paramSources = getParameterByName(param_name);
    if (paramSources != '' && paramSources != null) {
        selectedSourceIds = paramSources.split(',');
    }

    if (selectedSourceIds != undefined) {

        if (selectedSourceIds.indexOf(sourceId) == -1) {
            selectedSourceIds.push(sourceId);
        }

        updateQryPar(param_name, convertToCSV(selectedSourceIds));
    }

    refreshSelected(param_name, selectionUrl, search_hed);
}





addNewSelectedSources = function (data, param_name, search_hed) {


    var indexs = getNewSources(param_name,search_hed);

    var count = 0;
    var selected_sourceId = null;

    var tableBody = $('#' + search_hed).html();


    $.each(data, function (intIndex, objValue) {
        if ($.isArray(indexs)) {
            selected_sourceId = indexs[intIndex];
        }
        else {
            selected_sourceId = indexs;
        }

        tableBody += '<tr id = "' + selected_sourceId + '" class="selected_source" >'; //+ hidfield;
        tableBody += '<td>' + objValue + '</td>';
        tableBody += '<td><a href="" onClick ="removeFromSelection(\'' + selected_sourceId + '\',\'' + param_name + '\');return false"><div> Remove </div></a></td>';

        tableBody += '</tr>';
    });


    $('#' + search_hed).html(tableBody);
}



function removeFromSelection(sourceId, param_name) {

    var selectedSourceIds = new Array();
    var paramSources = getParameterByName(param_name);
    if (paramSources != '' && paramSources != null) {
        selectedSourceIds = paramSources.split(',');
    }


    if (selectedSourceIds != undefined) {

        selectedSourceIds.splice(selectedSourceIds.indexOf(sourceId), 1);

        $('#' + sourceId + '.selected_source').remove();

        updateQryPar(param_name, convertToCSV(selectedSourceIds));
    }

}


function createBody(data,batch_data, editorUrl, param_name, selectionUrl, pagerLinkFunction, searchBody,searchHead, pager) {

    var tableBody = '';

    $.each(
            data,
            function (intIndex, objValue) {
                var hidfield = '<input type="hidden" name="source_id" id="source_id" value ="' + objValue.id + '"/>';
                tableBody += '<tr>' + hidfield;
                var _loc = window.location.hash + '&id=' + objValue.id;
                _loc = _loc.replace('#', '');

                tableBody += '<td><a href="' + editorUrl + _loc + '"><div>' + objValue.ref + '</div></a></td>';
                tableBody += '<td><a href="" onClick ="addToSelection(\'' + objValue.id + '\',\'' + param_name + '\',\'' + selectionUrl + '\',\'' +searchHead + '\');return false"><div> Add </div></a></td>';
                tableBody += '</tr>';
            }
    );

    if (tableBody != '') {
        $('#' + searchBody).html(tableBody); //
        //$('#reccount').html(data.Total + ' Sources');
        $('#' + pager).html(createpager(batch_data.Batch, batch_data.BatchLength, batch_data.Total, pagerLinkFunction));
    }
    else {
        $('#' + searchBody).html(tableBody); //'#search_bdy'
        // $('#reccount').html('0 Sources');
    }

    refreshSelected(param_name, selectionUrl,searchHead);
}




getNewSources = function (paramName, search_hed) {

    var refreshRequired = false;
    var newSources = new Array();
    var currentSources = new Array();

    var paramSources = getParameterByName(paramName);

    //existing sources selected
    $('#'+ search_hed + ' .selected_source').each(function () {
        currentSources.push(this.id);
    });

    if (paramSources != '' && paramSources != null) {
        //if we just have 1 source in the query string
        if (paramSources.indexOf(',') == -1) {
            // if the current sources on the form dont contain that source 
            // add it to the new sources list
            if (currentSources.indexOf(paramSources) == -1) {
                newSources.push(paramSources);
            }
        } else {
            //loop through each source in the query string
            $.each(paramSources.split(','), function (key, val) {

                if (currentSources.indexOf(val) == -1) {

                    newSources.push(val);
                }

            });
        }
    }



    return newSources;
}