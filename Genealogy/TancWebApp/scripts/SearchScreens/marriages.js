﻿
//var url = getHost() + "/Marriages/GetMarriages/Select";


//var selection = new Array();
//var parishId = '';




$(document).ready(function () {
    var jsMaster = new JSMaster();
    var ancMarriages = new AncMarriages();

    jsMaster.generateHeader('#1', function () {
        ancMarriages.init();

    });

});


var AncMarriages = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.selection = new Array();
    this.parishId = '';
}

AncMarriages.prototype = {

    init: function () {
        var isActive = this.qryStrUtils.getParameterByName('active', '');

        var clickEvents = new Array();
        var panels = new panels();

        //clickEvents.push({ key: '#', value: 1 });
        //this.ancUtils.addlinks(clickEvents, this.loadDupes, this);

        $("#refresh").live("click", $.proxy(function () { this.getMarriages("0"); return false; }, this));
        $("#main").live("click", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $("#more").live("click", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));





        if (isActive == '1') {
            $('#txtMaleCName').val(this.qryStrUtils.getParameterByName('mcname', ''));
            $('#txtMaleSName').val(this.qryStrUtils.getParameterByName('msname', ''));
            $('#txtFemaleCName').val(this.qryStrUtils.getParameterByName('fcname', ''));
            $('#txtFemaleSName').val(this.qryStrUtils.getParameterByName('fsname', ''));
            $('#txtLocation').val(this.qryStrUtils.getParameterByName('locat', ''));

            $('#txtLowerDateRangeLower').val(this.qryStrUtils.getParameterByName('ldrl', ''));
            $('#txtLowerDateRangeUpper').val(this.qryStrUtils.getParameterByName('ldru', ''));

            this.parishId = this.qryStrUtils.getParameterByName('parid', '');

            this.getMarriages('1');
        }


    },

    createQryString: function () {
        this.qryStrUtils.updateQryPar({
            "active": '1',
            "mcname": $('#txtMaleCName'),
            "msname": $('#txtMaleSName'),
            "fcname": $('#txtFemaleCName'),
            "fsname": $('#txtFemaleSName'),
            "locat": $('#txtLocation'),
            "ldrl": $('#txtLowerDateRangeLower'),
            "ldru": $('#txtLowerDateRangeUpper'),
            "parid": parishId
        });

    },

    getMarriages: function (showdupes) {

        var parentId = '';
        if (showdupes == '0') {
            this.qryStrUtils.updateQryPar('_parentId', parentId);
        }
        else {
            parentId = this.qryStrUtils.getParameterByName('_parentId', '');
        }

        var params = {};
        params[0] = parentId;
        params[1] = String($('#txtMaleCName').val());
        params[2] = String($('#txtMaleSName').val());
        params[3] = String($('#txtFemaleCName').val());
        params[4] = String($('#txtFemaleSName').val());
        params[5] = String($('#txtLocation').val());
        params[6] = String($('#txtLowerDateRangeLower').val());
        params[7] = String($('#txtLowerDateRangeUpper').val());
        params[8] = '';
        params[9] = this.qryStrUtils.getParameterByName('parid', '');
        params[10] = String(this.qryStrUtils.getParameterByName('page', 0));
        params[11] = '30';
        params[12] = this.qryStrUtils.getParameterByName('sort_col', 'MarriageDate');

        this.ancUtils.twaGetJSON('/Marriages/GetMarriages/Select', params, this.marriageResult);

        this.createQryString(page);

        return false;
    },


    marriageResult: function (data) {
        //alert('received something');
        var tableBody = '';
        var visibleRecords = new Array();

        var dupeEvents = new Array();
        var selectEvents = new Array();

        var _idx = 0;

        $.each(data.serviceMarriages, function (source, sourceInfo) {

            var hidPID = '<input type="hidden" name="MarriageId" id="MarriageId" value ="' + sourceInfo.MarriageId + '"/>';
            var hidParID = '<input type="hidden" name="parent_id" id="parent_id" value ="' + sourceInfo.XREF + '"/>';

            var arIdx = jQuery.inArray(sourceInfo.MarriageId, this.selection);

            if (arIdx >= 0) {
                tableBody += '<tr class = "highLightRow">' + hidPID + hidParID;
            }
            else {
                tableBody += '<tr>' + hidPID + hidParID;
            }

            var _loc = window.location.hash;
            _loc = this.ancUtils.updateStrForQry(_loc, 'id', sourceInfo.MarriageId);

            tableBody += '<td><a id= "d' + _idx + '" href=""><div>' + sourceInfo.Events + '</div></a></td>';
            dupeEvents.push({ key: '#d' + _idx, value: sourceInfo.XREF });

            tableBody += '<td><a id= "s' + _idx + '" href=""><div>' + sourceInfo.MarriageDate + '</div></a></td>';
            selectEvents.push({ key: '#s' + _idx, value: sourceInfo.MarriageId });

            tableBody += '<td><a href="../HtmlPages/MarriageEditor.html' + _loc + '"><div> Edit </div></a></td>';

            tableBody += '<td><div>' + sourceInfo.MaleCName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.MaleSName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.FemaleCName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.FemaleSName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.MarriageLocation + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.Witnesses + '</div></td>';
            tableBody += '</tr>';

            visibleRecords.push(sourceInfo.MarriageId);

            _idx++;
        });

        if (this.selection != undefined) {
            this.selection = this.selection.RemoveInvalid(visibleRecords);
        }

        if (tableBody != '') {

            $('#search_bdy').html(tableBody);
            //create pager based on results

            $('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));

            $('#reccount').html(data.Total + ' Marriages');
        }
        else {

            $('#search_bdy').html(tableBody);
            $('#reccount').html('0 Marriages');
        }


        this.ancUtils.addlinks(dupeEvents, this.loadDupes, this);

        this.ancUtils.addlinks(selectEvents, this.processSelect, this);

    },

    loadDupes: function (id) {
        this.qryStrUtils.updateQryPar('_parentId', id);
        this.getMarriages('1');
    },

    processSelect: function (evt) {
        this.ancUtils.handleSelection(evt, selection, '#search_bdy tr', "#MarriageId");
    },

    getLink: function (toPage) {
        this.qryStrUtils.updateQryPar('page', toPage);
        this.getMarriages('1');
    },

    sort: function (sort_col) {
        this.ancUtils.sort_inner(sort_col);
        this.getMarriages('1');
    },

    DeleteRecord: function () {
        var theData = {};

        theData.marriageIds = this.ancUtils.convertToCSV(this.selection);

        this.ancUtils.twaPostJSON('/Marriages/Delete', theData, '', '', function (args) {
            this.getMarriages('1');
        });
    },

    SetDuplicates: function () {
        var theData = {};

        theData.marriages = this.ancUtils.convertToCSV(this.selection);

        this.ancUtils.twaPostJSON('/Marriages/SetDuplicate', theData, '', '', function (args) {
            this.getMarriages('1');
        });

    },

    SetRemoveLink: function () {

        var theData = {};

        theData.marriage = this.ancUtils.convertToCSV(this.selection);

        this.ancUtils.twaPostJSON('/Marriages/RemoveLinks', theData, '', '', function (args) {
            this.getMarriages('1');
        });

    },

    SetMergeMarriages: function () {
        var theData = {};
        theData.marriage = this.ancUtils.convertToCSV(this.selection);
        this.ancUtils.twaPostJSON('/Marriages/MergeMarriages', theData, '', '', function (args) {
            this.getMarriages('1');
        });
    },

    addMarriage: function (path) {
        window.location.href = '../HtmlPages/MarriageEditor.html#' + this.qryStrUtils.makeIdQryString('id', path);

    }
}



 
//function imready() {

//    var isActive = getParameterByName('active', '');

//    if (isActive == '1') {
//        $('#txtMaleCName').val(getParameterByName('mcname', ''));
//        $('#txtMaleSName').val(getParameterByName('msname', ''));
//        $('#txtFemaleCName').val(getParameterByName('fcname', ''));
//        $('#txtFemaleSName').val(getParameterByName('fsname', ''));
//        $('#txtLocation').val(getParameterByName('locat', ''));

//        $('#txtLowerDateRangeLower').val(getParameterByName('ldrl', ''));
//        $('#txtLowerDateRangeUpper').val(getParameterByName('ldru', ''));

//        parishId = getParameterByName('parid', '');

//        getMarriages('1');
//    }
//}


//function createQryString(page) {



//    updateQryPar('active', '1');

//    var workingQry = window.location.hash;

//    workingQry = updateStrForQry(workingQry, 'mcname', $('#txtMaleCName').val());
//    workingQry = updateStrForQry(workingQry, 'msname', $('#txtMaleSName').val());
//    workingQry = updateStrForQry(workingQry, 'fcname', $('#txtFemaleCName').val());
//    workingQry = updateStrForQry(workingQry, 'fsname', $('#txtFemaleSName').val());
//    workingQry = updateStrForQry(workingQry, 'locat', $('#txtLocation').val());
//    workingQry = updateStrForQry(workingQry, 'ldrl', $('#txtLowerDateRangeLower').val());
//    workingQry = updateStrForQry(workingQry, 'ldru', $('#txtLowerDateRangeUpper').val());

//    workingQry = updateStrForQry(workingQry, 'parid', parishId);

//    //window.location.hash = workingQry;

//    window.location.replace(workingQry);
//}

//string _parentId,
//string christianName,
//string surname,
//string fatherChristianName,
//string fatherSurname,
//string motherChristianName,
//string motherSurname,
//string location,
//string lowerDate,
//string upperDate,
//string filterTreeResults,
//string filterIncludeBirths,
//string filterIncludeDeaths,
//string page_number,
//string page_size);



//function getMarriages(showdupes) {

//    //BirthInt
//    //_parentId
//    var params = {};

//    var page = getParameterByName('page');
//    var sort_col = getParameterByName('sort_col');
//    var parentId = '';

//   

//    if (!page || isNaN(page))
//        page = 0;

//    if (!sort_col || sort_col == '')
//        sort_col = 'MarriageDate';

//    if (showdupes == '0') {
//        updateQryPar('_parentId', parentId);
//    }
//    else {
//        parentId = getParameterByName('_parentId', '');
//    }

//    var parishId = getParameterByName('parid', '');

//    params[0] = parentId;
//    params[1] = String($('#txtMaleCName').val());
//    params[2] = String($('#txtMaleSName').val());
//    params[3] = String($('#txtFemaleCName').val());
//    params[4] = String($('#txtFemaleSName').val());
//    params[5] = String($('#txtLocation').val());
//    params[6] = String($('#txtLowerDateRangeLower').val());
//    params[7] = String($('#txtLowerDateRangeUpper').val());
//    params[8] = '';
//    params[9] = parishId;
//    params[10] = String(page);
//    params[11] = '30';
//    params[12] = sort_col;


//    twaGetJSON('/Marriages/GetMarriages/Select', params, marriageResult);


//    createQryString(page);

//    return false;
//}






//function marriageResult(data) {
//    //alert('received something');
//    var tableBody = '';
//    var visibleRecords = new Array();

//    


//    $.each(data.serviceMarriages, function (source, sourceInfo) {
//        //<a href='' class="button" ><span>Main</span></a>
//        var hidPID = '<input type="hidden" name="MarriageId" id="MarriageId" value ="' + sourceInfo.MarriageId + '"/>';
//        var hidParID = '<input type="hidden" name="parent_id" id="parent_id" value ="' + sourceInfo.XREF + '"/>';

//        var arIdx = jQuery.inArray(sourceInfo.MarriageId, selection);

//        if (arIdx >= 0) {
//            tableBody += '<tr class = "highLightRow">' + hidPID + hidParID;
//        }
//        else {
//            tableBody += '<tr>' + hidPID + hidParID;
//        }

//        var _loc = window.location.hash;
//        _loc = updateStrForQry(_loc, 'id', sourceInfo.MarriageId);

//        // _loc = _loc.replace('#', '');

//        tableBody += '<td><a href="" onClick ="loadDupes(\'' + sourceInfo.XREF + '\');return false"><div>' + sourceInfo.Events + '</div></a></td>';



//        tableBody += '<td><a href="" onClick ="processSelect(\'' + sourceInfo.MarriageId + '\');return false"><div>' + sourceInfo.MarriageDate + '</div></a></td>';

//        //~/HtmlPages/MarriageEditor.html#?_parentId=&active=1&mcname=test&msname=&fcname=&fsname=&locat=&ldrl=1700&ldru=1799&parid=&id=dafb85fe-cbfb-4d77-adb2-1caae78a9066">Marriage Editor</asp:HyperLink>

//        tableBody += '<td><a href="../HtmlPages/MarriageEditor.html' + _loc + '"><div> Edit </div></a></td>';

//        //tableBody += '<td><a href="../Forms/FrmEditorMarriages.aspx' + _loc + '"><div> Edit </div></a></td>';

//        tableBody += '<td><div>' + sourceInfo.MaleCName + '</div></td>';
//        tableBody += '<td><div>' + sourceInfo.MaleSName + '</div></td>';
//        tableBody += '<td><div>' + sourceInfo.FemaleCName + '</div></td>';
//        tableBody += '<td><div>' + sourceInfo.FemaleSName + '</div></td>';
//        tableBody += '<td><div>' + sourceInfo.MarriageLocation + '</div></td>';
//        tableBody += '<td><div>' + sourceInfo.Witnesses + '</div></td>';
//        tableBody += '</tr>';

//        visibleRecords.push(sourceInfo.MarriageId);
//    });





//    if (selection != undefined) {
//       selection = selection.RemoveInvalid(visibleRecords);
//    }

//    if (tableBody != '') {

//        $('#search_bdy').html(tableBody);
//        //create pager based on results

//        $('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));

//        $('#reccount').html(data.Total + ' Marriages');
//    }
//    else {

//        $('#search_bdy').html(tableBody);
//        $('#reccount').html('0 Marriages');
//    }





//}


//function loadDupes(id) {
//    updateQryPar('_parentId', id);
//    getMarriages('1');
//}

//function processSelect(evt) {

//    var arIdx = jQuery.inArray(evt, selection);

//    if (arIdx == -1) {
//        selection.push(evt);
//    }
//    else {
//        selection.splice(arIdx, 1);
//    }

//    $('#search_bdy tr').each(function () {
//        $this = $(this)

//        var quantity = $this.find("#MarriageId").val();
//        arIdx = jQuery.inArray(quantity, selection);

//        if (arIdx == -1) {
//            $this.removeClass('highLightRow');
//        }
//        else {
//            $this.addClass('highLightRow');
//        }
//    }); //end each

//}



//function getLink(toPage) {

//    updateQryPar('page', toPage);
//    getMarriages('1');

//}


//function sort(sort_col) {


//    sort_inner(sort_col);
//    getMarriages('1');
//}





//function DeleteRecord() {

//    var theData = {};

//    theData.marriageIds = convertToCSV(selection);

//    twaPostJSON('/Marriages/Delete', theData, '', '', function (args) {
//        getMarriages('1');
//    });
//}


//function SetDuplicates() {


//    var theData = {};

//    theData.marriages = convertToCSV(selection);

//    twaPostJSON('/Marriages/SetDuplicate', theData, '', '', function (args) {
//        getMarriages('1');
//    });

//}


//function SetRemoveLink() {
//    //var localurl = getHost() + '/Marriages/RemoveLinks';
//    var theData = {};

//    theData.marriage = convertToCSV(selection);

//    twaPostJSON('/Marriages/RemoveLinks', theData, '', '', function (args) {
//        getMarriages('1');
//    });

//}

//function SetMergeMarriages() {

//   // var localurl = getHost() + '/Marriages/MergeMarriages';
//    var theData = {};

//    theData.marriage = convertToCSV(selection);

//    twaPostJSON('/Marriages/MergeMarriages', theData, '', '', function (args) {
//        getMarriages('1');
//    });

//}

//function addMarriage(path) {
// 
//    window.location.href = '../HtmlPages/MarriageEditor.html#' + makeIdQryString('id', path);
// 
//}
 