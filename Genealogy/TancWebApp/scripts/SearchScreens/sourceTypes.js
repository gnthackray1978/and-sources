﻿$(document).ready(function () {
    var jsMaster = new JSMaster();
    var ancSourceTypes = new AncSourceTypes();
    jsMaster.generateHeader('#1', function () {
        ancSourceTypes.init();
    });
});


var AncSourceTypes = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.selection = new Array();
  
    this.postParams = {
        url: '',
        data: '',
        idparam: undefined,
        refreshmethod: this.getSourceTypes,
        refreshArgs: ['1'],
        Context: this
    };
}



AncSourceTypes.prototype = {

    init: function () {
        var isActive = this.qryStrUtils.getParameterByName('active', '');
        
        var panels = new Panels();

        $("#main").live("click", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $("#more").live("click", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
        $("#refresh").live("click", $.proxy(function () { this.getSourceTypes(); return false; }, this));

        $("#add").live("click", $.proxy(function () { this.AddSourceType(); return false; }, this));
        $("#delete").live("click", $.proxy(function () { this.DeleteRecord(); return false; }, this));

        $("#stdesc").live("click", $.proxy(function () { this.sort('SourceTypeDesc'); return false; }, this));

        if (isActive == '1') {
            $('#txtDescription').val(this.qryStrUtils.getParameterByName('stdesc', ''));


            this.getSourceTypes('1');
        }
    },
    createQryString: function(page) {
         
        var args = {
            "active": '1',
            "stdesc": $('#txtDescription') 
        };

        this.qryStrUtils.updateQry(args);
    },
    getSourceTypes: function(showdupes) {

        var params = {};

        params[0] = String($('#txtDescription').val());
        params[1] = String(this.qryStrUtils.getParameterByName('page', 0));
        params[2] = '30';
        params[3] = String(this.qryStrUtils.getParameterByName('sort_col', 'SourceTypeDesc'));

        this.ancUtils.twaGetJSON('/SourceTypes/Select', params, $.proxy(this.processData, this));
        
        this.createQryString(page);

        return false;
    },

    processData: function(data) {
        //alert('received something');
        var tableBody = '';
        var tableBody = '';
        var selectEvents = new Array();
        var _idx = 0;
        var that = this;



        $.each(data.serviceSources, function (source, sourceInfo) {
            //<a href='' class="button" ><span>Main</span></a>
            var hidPID = '<input type="hidden" name="SourceTypeId" id="SourceTypeId" value ="' + sourceInfo.TypeId + '"/>';


            var arIdx = jQuery.inArray(sourceInfo.TypeId, selection);

            if (arIdx >= 0) {
                tableBody += '<tr class = "highLightRow">' + hidPID + hidParID;
            }
            else {
                tableBody += '<tr>' + hidPID;
            }

            var _loc = window.location.hash;
            _loc = that.qryStrUtils.updateStrForQry(_loc, 'id', sourceInfo.TypeId);

            tableBody += '<td><a id= "s' + _idx + '" href="" ><div>' + sourceInfo.Description + '</div></a></td>';
            selectEvents.push({ key: 's' + _idx, value: sourceInfo.TypeId });

            tableBody += '<td><div>' + sourceInfo.Order + '</div></td>';
            tableBody += '<td><a href="../HtmlPages/SourceTypeEditor.html' + _loc + '"><div> Edit </div></a></td>';

            tableBody += '</tr>';
        });

        if (tableBody != '') {

            $('#search_bdy').html(tableBody);
            //create pager based on results


            var pagerparams = { ParentElement: 'pager',
                Batch: data.Batch,
                BatchLength: data.BatchLength,
                Total: data.Total,
                Function: this.getLink,
                Context: this
            };

            this.ancUtils.createpager(pagerparams);

            //$('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));

            $('#reccount').html(data.Total + ' Source Types');
        }
        else {

            $('#search_bdy').html(tableBody);
            $('#reccount').html('0 Source Types');
        }

        this.ancUtils.addlinks(selectEvents, this.processSelect, this);
    },
    processSelect: function (evt) {
        this.ancUtils.handleSelection(evt, this.selection, '#search_bdy tr', "#SourceTypeId");
    },
    sort: function (sort_col) {
        this.ancUtils.sort_inner(sort_col);
        this.getSourceTypes();
    },
    getLink: function(toPage) {
        this.qryStrUtils.updateQryPar('page', toPage);
        this.getSourceTypes();
    },
    DeleteRecord: function () {
        this.postParams.url = '/SourceTypes/Delete';
        this.postParams.data = { sourceIds: this.ancUtils.convertToCSV(this.selection) };
        this.ancUtils.twaPostJSON(this.postParams);
    },
    AddParish: function () {
        window.location.href = '../HtmlPages/SourceTypeEditor.html#' + this.qryStrUtils.makeIdQryString('id', '0');
    }

}


