



//this.ancSelectorBase.context_data = {
//    name: ''
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

    createOutline: function (selectorid, base) {


        var body = '';

        body += '<div class = "sf_shad sf_cont">';

        body += '<div class = "sf_hed"><div><b>' + this.context_data.title + '</b></div></div>';

        body += '<div class = "sf_pad-cont">';
        body += '<table border="0">';
        body += '<thead>';
        body += '<tr class="tophed">';
        body += '<td><a href="../Default.aspx" onclick = "return false" ><span>Ref</span></a></td>';
        body += '<td></td>';
        body += '</tr>';
        body += '</thead>';

        body += '<tbody id = "' + this.context_data.search_hed + '">';
        body += '</tbody>';
        body += '</table>';

        body += '<br />';

        body += '<div class = "sf_bot"><div><b>Select From</b></div></div>';

        body += '<div class="sf_mid-lab">';
        body += '<div class="sf_mlab-dlef">';
        body += '<input id="' + this.context_data.sourceRefId + '" class = "sf_txt" type="text"/>';
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
        body += '<tbody id = "' + this.context_data.search_body + '">';
        body += '</tbody>';
        body += '</table>';

        body += '</div>';


        body += '<div class="fltbrow">';
        body += '<div id="' + this.context_data.pager + '" class = "pager"></div>';

        body += '</div>';

        body += '</div>';

        body += '</div>';

        $(selectorid).html(body);

        var that = this;

      
       // var tp = base;
        //var lrefreshMethod = that.context_data.refreshMethod;


        $('body').on("click", "#sb_refresh",function () {
            that.context_data.refreshMethod.call(that.context_data.parentContext, 'nothing');          
            return false;
        });

   


    },


    refreshSelected: function () {

        var newSources = this.getNewSources();

        if (newSources.length > 0) {
            var params = {};
            params[0] = this.ancUtils.convertToCSV(newSources);

            var successmethod = function (data) {
                var rows = new Array();
                $.each(data, function (source, sourceInfo) {
                    var row0 = { desc: sourceInfo };
                    rows.push(row0);
                });
                this.addNewSelectedSources(data);
            };

            this.ancUtils.twaGetJSON(this.context_data.selectionUrl, params, $.proxy(successmethod, this));
        }
    },


    addNewSelectedSources: function (data) {

        var indexs = this.getNewSources();
        var selectEvents = new Array();

        var selected_sourceId = null;

        var tableBody = $('#' + this.context_data.search_hed).html();

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
            selectEvents.push({ key: 'sr' + _idx, value: selected_sourceId });

            tableBody += '</tr>';
            _idx++;
        });


        $('#' + this.context_data.search_hed).html(tableBody);

        this.ancUtils.addlinks(selectEvents, this.removeFromSelection, this);
    },


    removeFromSelection: function (sourceId) {

        //sourceId, param_name

        var selectedSourceIds = new Array();
        var paramSources = this.qryStrUtils.getParameterByName(this.context_data.param_name);
        if (paramSources !== '' && paramSources !== null) {
            selectedSourceIds = paramSources.split(',');
        }


        if (selectedSourceIds !== undefined) {

            selectedSourceIds.splice(selectedSourceIds.indexOf(sourceId), 1);

            $('#' + sourceId + '.selected_source').remove();

            this.qryStrUtils.updateQryPar(this.context_data.param_name, this.ancUtils.convertToCSV(selectedSourceIds));
        }

    },

    createBody: function (batch_data) {
        var tableBody = '';
        var selectEvents = new Array();
        var _idx = 0;

        var that = this;
        $.each(
                batch_data.rows,
                function (intIndex, objValue) {
                    var hidfield = '<input type="hidden" name="source_id" id="source_id" value ="' + objValue.id + '"/>';
                    tableBody += '<tr>' + hidfield;
                    var _loc = window.location.hash + '&id=' + objValue.id;
                    _loc = _loc.replace('#', '');

                    tableBody += '<td><a href="' + that.context_data.editorUrl + _loc + '"><div>' + objValue.ref + '</div></a></td>';
                    tableBody += '<td><a id= "cb' + that.context_data.name + _idx + '" href="" ><div> Add </div></a></td>';

                    selectEvents.push({ key: 'cb' + that.context_data.name + _idx, value: objValue.id });

                    tableBody += '</tr>';

                    _idx++;
                }


        );

        if (tableBody != '') {
            $('#' + this.context_data.search_body).html(tableBody); //


            //create pager based on results
            var pagerparams = { ParentElement: this.context_data.pager,
                Batch: batch_data.Batch,
                BatchLength: batch_data.BatchLength,
                Total: batch_data.Total,
                Function: this.context_data.pagerfunction,
                Context: this.context_data.parentContext
            };

            this.ancUtils.createpager(pagerparams);


            // $('#' + pager).html(createpager(batch_data.Batch, batch_data.BatchLength, batch_data.Total, pagerLinkFunction));


        }
        else {
            $('#' + this.context_data.search_body).html(tableBody); //'#search_bdy'         
        }

        this.ancUtils.addlinks(selectEvents, this.addToSelection, this);

        this.refreshSelected();


    },

    addToSelection: function (sourceId) {
        //sourceId, param_name, selectionUrl, search_hed
        var selectedSourceIds = new Array();
        //var param_name = 'source_ids';

        var paramSources = this.qryStrUtils.getParameterByName(this.context_data.param_name);

        // some hacks to get rid of nulls
        paramSources = paramSources.replace("null", "").replace(",,", ",");
        
        if (paramSources != '' && paramSources != null) {

            selectedSourceIds = paramSources.split(',');
        }

        if (selectedSourceIds != undefined) {

            if (selectedSourceIds.indexOf(sourceId) == -1) {
                selectedSourceIds.push(sourceId);
            }

            this.qryStrUtils.updateQryPar(this.context_data.param_name, this.ancUtils.convertToCSV(selectedSourceIds));
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
