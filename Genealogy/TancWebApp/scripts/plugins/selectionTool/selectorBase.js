
//this.ancSelectorBase.context_data = {
//    editorUrl: '../Forms/FrmEditSource.aspx',
//    param_name: 'source_ids',
//    selectionUrl: '/Sources/GetSourceNames',
//    pagerfunction: undefined,   // this.pagerfunction = 'getLink';
//    search_body: 'search_bdy',
//    search_hed: 'search_hed',
//    pager: 's_pager',
//    sourceRefId: 'txtSourceRef',
//    title: 'SOURCES',
//    refreshMethod: this.getSources

//};


var AncSelectorBase = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.selection = new Array();
    this.context_data = {};

}

AncSelectorBase.prototype = {

    createOutline: function (selectorid) {
      
                  
        var body = '';

        body += '<div class = "sf_shad sf_cont">';

        body += '<div class = "sf_hed"><div><b>' + this.ancSelectorBase.context_data.title + '</b></div></div>';

        body += '<div class = "sf_pad-cont">';
        body += '<table border="0">';
        body += '<thead>';
        body += '<tr class="tophed">';
        body += '<td><a href="../Default.aspx" onclick = "return false" ><span>Ref</span></a></td>';
        body += '<td></td>';
        body += '</tr>';
        body += '</thead>';

        body += '<tbody id = "' + this.ancSelectorBase.context_data.search_hed + '">';
        body += '</tbody>';
        body += '</table>';

        body += '<br />';

        body += '<div class = "sf_bot"><div><b>Select From</b></div></div>';

        body += '<div class="sf_mid-lab">';
        body += '<div class="sf_mlab-dlef">';
        body += '<input id="' + this.ancSelectorBase.context_data.sourceRefId + '" class = "sf_txt" type="text"/>';
        body += '</div>';


        body += '<div class="sf_mlab-drig">';
        body += '<a id= "sb_refresh" href="" class="button" ><span>R</span></a>';
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
        body += '<tbody id = "' + this.ancSelectorBase.context_data.search_body + '">';
        body += '</tbody>';
        body += '</table>';

        body += '</div>';


        body += '<div class="fltbrow">';
        body += '<div id="' + this.ancSelectorBase.context_data.pager + '" class = "pager"></div>';

        body += '</div>';

        body += '</div>';

        body += '</div>';

        $(selectorid).html(body);


        $("#sb_refresh").live("click", $.proxy(this.ancSelectorBase.context_data.refreshMethod, this));


    },


    refreshSelected: function () {

        var newSources = this.getNewSources();

        if (newSources.length > 0) {
            var params = {};
            params[0] = convertToCSV(newSources);

            var successmethod = function (data) {
                var rows = new Array();
                $.each(data, function (source, sourceInfo) {
                    var row0 = { desc: sourceInfo };
                    rows.push(row0);
                });
                this.addNewSelectedSources(data);
            };

            this.ancUtils.twaGetJSON(this.ancSelectorBase.context_data.selectionUrl, params, $.proxy(successmethod, this));
        }
    },


    addNewSelectedSources: function (data) {

        var indexs = this.getNewSources();
        var selectEvents = new Array();
        var count = 0;
        var selected_sourceId = null;

        var tableBody = $('#' + this.ancSelectorBase.context_data.search_hed).html();

        var _idx = 0;

        $.each(data, function (intIndex, objValue) {
            if ($.isArray(indexs)) {
                selected_sourceId = indexs[intIndex];
            }
            else {
                selected_sourceId = indexs;
            }

            tableBody += '<tr id = "' + selected_sourceId + '" class="selected_source" >'; //+ hidfield;
            tableBody += '<td>' + objValue + '</td>';
            tableBody += '<td><a id= "sr' + _idx + '" href=""><div> Remove </div></a></td>';
            selectEvents.push({ key: 'sr' + _idx, value: selected_sourceId});

            tableBody += '</tr>';
            _idx++;
        });


        $('#' + this.ancSelectorBase.context_data.search_hed).html(tableBody);

        this.ancUtils.addlinks(selectEvents, this.removeFromSelection, this);
    },


    removeFromSelection: function (sourceId) {

        //sourceId, param_name

        var selectedSourceIds = new Array();
        var paramSources = this.qryStrUtils.getParameterByName(this.context_data.param_name);
        if (paramSources != '' && paramSources != null) {
            selectedSourceIds = paramSources.split(',');
        }


        if (selectedSourceIds != undefined) {

            selectedSourceIds.splice(selectedSourceIds.indexOf(sourceId), 1);

            $('#' + sourceId + '.selected_source').remove();

            this.qryStrUtils.updateQryPar(this.context_data.param_name, convertToCSV(selectedSourceIds));
        }

    },

    createBody: function (batch_data) {
        var tableBody = '';
        var selectEvents = new Array();
        var _idx = 0;

        $.each(
                batch_data.rows,
                function (intIndex, objValue) {
                    var hidfield = '<input type="hidden" name="source_id" id="source_id" value ="' + objValue.id + '"/>';
                    tableBody += '<tr>' + hidfield;
                    var _loc = window.location.hash + '&id=' + objValue.id;
                    _loc = _loc.replace('#', '');

                    tableBody += '<td><a href="' + editorUrl + _loc + '"><div>' + objValue.ref + '</div></a></td>';
                    tableBody += '<td><a id= "cb' + _idx + '" href="" ><div> Add </div></a></td>';

                    selectEvents.push({ key: 'cb' + _idx, value: objValue.id });
                    
                    tableBody += '</tr>';
                    
                    _idx++;
                }


        );

        if (tableBody != '') {
            $('#' + this.context_data.searchBody).html(tableBody); //


            //create pager based on results
            var pagerparams = { ParentElement: this.context_data.pager,
                Batch: batch_data.Batch,
                BatchLength: batch_data.BatchLength,
                Total: batch_data.Total,
                Function: this.context_data.pagerfunction,
                Context: this
            };

            this.ancUtils.createpager(pagerparams);


           // $('#' + pager).html(createpager(batch_data.Batch, batch_data.BatchLength, batch_data.Total, pagerLinkFunction));


        }
        else {
            $('#' + this.context_data.searchBody).html(tableBody); //'#search_bdy'         
        }

        this.ancUtils.addlinks(selectEvents, this.addToSelection, this);

        this.refreshSelected();


    },

    addToSelection: function (sourceId) {
        //sourceId, param_name, selectionUrl, search_hed
        var selectedSourceIds = new Array();
        //var param_name = 'source_ids';

        var paramSources = this.qryStrUtils.getParameterByName(this.context_data.param_name);
        if (paramSources != '' && paramSources != null) {
            selectedSourceIds = paramSources.split(',');
        }

        if (selectedSourceIds != undefined) {

            if (selectedSourceIds.indexOf(sourceId) == -1) {
                selectedSourceIds.push(sourceId);
            }

            this.qryStrUtils.updateQryPar(this.context_data.param_name, convertToCSV(selectedSourceIds));
        }

        this.refreshSelected();
    },

    getNewSources: function () {

        var refreshRequired = false;
        var newSources = new Array();
        var currentSources = new Array();

        var paramSources = this.qryStrUtils.getParameterByName(this.context_data.param_name);

        //existing sources selected
        $('#' + this.context_data.search_hed + ' .selected_source').each(function () {
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




}

//inc
function createOutline(refresh_function,selectionbodyid, resultsbodyid, searchInputId, title, pager) {
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
    body += '<a href="" onclick = "' + refresh_function + '();return false" class="button" ><span>R</span></a>';
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


//inc
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

//inc
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




//inc
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


//inc
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

//inc
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