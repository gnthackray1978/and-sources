
var JSMaster, QryStrUtils, AncUtils,Panels;

 


$(document).ready(function () {
    var jsMaster = new JSMaster();
    var ancEvents = new AncEvents();

    jsMaster.generateHeader('#1', function () {
        ancEvents.init();

    });

});


var AncEvents = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.pager = new Pager();

    this.DEFAULT_SELECT_URL = '/Events/GetEvents/Select';
    this.DEFAULT_MARRIAGEEDITOR_URL = '../HtmlPages/MarriageEditor.html';
    this.DEFAULT_PERSONEDITOR_URL = '../HtmlPages/PersonEditor.html';
    this.DEFAULT_SOURCEEDITOR_URL = '../HtmlPages/SourceEditor.html';

    this.selection = [];
 
    this.postParams = { 
        url: '',
        data: '',
        idparam: undefined,
        refreshmethod: this.getEvents,
        refreshArgs: ['1'],
        Context: this
    };
 
};

AncEvents.prototype = {
    
    init: function() {

        var isActive = this.qryStrUtils.getParameterByName('active', '');
        
        var panels = new Panels();
     
        $('body').on("click", "#main", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $('body').on("click", "#more", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));

        $('body').on("click", "#refresh", $.proxy(function () { this.getEvents("0"); return false; }, this));

        $('body').on("click", "#year_hed", $.proxy(function () { this.sort('EventDate'); return false; }, this));
        $('body').on("click", "#surname_hed", $.proxy(function () { this.sort('EventSurname'); return false; }, this));
        $('body').on("click", "#locat_hed", $.proxy(function () { this.sort('EventLocation'); return false; }, this));
        
        
    
        if (isActive == '1') {
            $('#txtCName').val(this.qryStrUtils.getParameterByName('cname', ''));
            $('#txtSName').val(this.qryStrUtils.getParameterByName('sname', ''));
            $('#txtLocation').val(this.qryStrUtils.getParameterByName('locat', ''));
            $('#txtLowerDateRange').val(this.qryStrUtils.getParameterByName('lower_dat', ''));
            $('#txtUpperDateRange').val(this.qryStrUtils.getParameterByName('upper_dat', ''));
    
            if (this.qryStrUtils.getParameterByName('inc_par', '') == 'false') {
                $('#chkIncludeParents').prop('checked', false);
            }
            else {
                $('#chkIncludeParents').prop('checked', true);
            }
    
            if (this.qryStrUtils.getParameterByName('incb', '') == 'false') {
                $('#chkIncludeBirths').prop('checked', false);
            }
            else {
                $('#chkIncludeBirths').prop('checked', true);
            }
    
            if (this.qryStrUtils.getParameterByName('incd', '') == 'false') {
                $('#chkIncludeDeaths').prop('checked', false);
            }
            else {
                $('#chkIncludeDeaths').prop('checked', true);
            }
    
            if (this.qryStrUtils.getParameterByName('incm', '') == 'false') {
                $('#chkIncludeMarriages').prop('checked', false);
            }
            else {
                $('#chkIncludeMarriages').prop('checked', true);
            }
    
            if (this.qryStrUtils.getParameterByName('incw', '') == 'false') {
                $('#chkIncludeWitnesses').prop('checked', false);
            }
            else {
                $('#chkIncludeWitnesses').prop('checked', true);
            }
    
            if (this.qryStrUtils.getParameterByName('incs', '') == 'false') {
                $('#chkIncludeSpouses').prop('checked', false);
            }
            else {
                $('#chkIncludeSpouses').prop('checked', true);
            }
    
            if (this.qryStrUtils.getParameterByName('inc_ps', '') == 'false') {
                $('#chkIncludePersonWithSpouses').prop('checked', false);
            }
            else {
                $('#chkIncludePersonWithSpouses').prop('checked', true);
            }
    
    
            this.getEvents('1');
        }

    },
    createQryString: function() {
    
        var args = {
                "active": '1',
                "cname": $('#txtCName'),
                "sname": $('#txtSName'),
                "locat": $('#txtLocation'),
                "lower_dat": $('#txtLowerDateRange'),
                "upper_dat": $('#txtUpperDateRange'),
                "inc_par": $('#chkIncludeParents').prop('checked'),
                "incb": $('#chkIncludeBirths').prop('checked'),
                "incd": $('#chkIncludeDeaths').prop('checked'),
                "incm": $('#chkIncludeMarriages').prop('checked'),
                "incw": $('#chkIncludeWitnesses').prop('checked'),
                "incs": $('#chkIncludeSpouses').prop('checked'),
                "inc_ps": $('#chkIncludePersonWithSpouses').prop('checked')
                
            };
    
        this.qryStrUtils.updateQry(args);
    
    },
    
    getEvents: function(showdupes) {
    
        //BirthInt
        //_parentId
        var params = {};
     
        params[0] = String($('#chkIncludeBirths').prop('checked'));
        params[1] = String($('#chkIncludeDeaths').prop('checked'));
        params[2] = String($('#chkIncludeWitnesses').prop('checked'));
        params[3] = String($('#chkIncludeParents').prop('checked'));
        params[4] = String($('#chkIncludeMarriages').prop('checked'));
        params[5] = String($('#chkIncludeSpouses').prop('checked'));
        params[6] = String($('#chkIncludePersonWithSpouses').prop('checked'));  
        params[7] = String($('#txtCName').val());
        params[8] = String($('#txtSName').val());
        params[9] = String($('#txtLowerDateRange').val());
        params[10] = String($('#txtUpperDateRange').val());
        params[11] = String($('#txtLocation').val());
        params[12] = String(this.qryStrUtils.getParameterByName('page', '0'));
        params[13] = '30';
        params[14] = this.qryStrUtils.getParameterByName('sort_col', 'BirthInt');
    
        this.ancUtils.twaGetJSON(this.DEFAULT_SELECT_URL, params, $.proxy(this.processData, this));
    
        this.createQryString();
    
        return false;
    },
    
    processData: function(data) {
        //alert('received something');
        var tableBody = '';
    
        $.each(data.serviceEvents, function (source, sourceInfo) {
            //<a href='' class="button" ><span>Main</span></a>
            var hidEvtId = '<input type="hidden" name="event_id" id="event_id" value ="' + sourceInfo.EventId + '"/>';
            var hidLnkId = '<input type="hidden" name="link_id" id="link_id" value ="' + sourceInfo.LinkId + '"/>';
    
            tableBody += '<tr>' + hidEvtId + hidLnkId;
    
    
            var _loc = window.location.hash + '&id=' + sourceInfo.LinkId;
    
           // _loc = _loc.replace('#', '');
    
            tableBody += '<td><div>' + sourceInfo.EventDate + '</div></td>';
    
    
            //person
            if (sourceInfo.LinkTypeId == 1) {
                tableBody += '<td><a href="' +this.DEFAULT_PERSONEDITOR_URL + _loc + '"><div>'+ sourceInfo.EventDescription+'</div></a></td>';
            }
    
            //marriage
            if (sourceInfo.LinkTypeId == 2) {
                tableBody += '<td><a href="' + this.DEFAULT_MARRIAGEEDITOR_URL + _loc + '"><div>' + sourceInfo.EventDescription + '</div></a></td>';
            }
    
            //source
            if (sourceInfo.LinkTypeId == 4) {
                tableBody += '<td><a href="' + this.DEFAULT_PERSONEDITOR_URL + _loc + '"><div>' + sourceInfo.EventDescription + '</div></a></td>';
            }
    
            tableBody += '<td><div>' + sourceInfo.EventChristianName + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.EventSurname + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.EventLocation + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.EventText + '</div></td>';
    
    
            tableBody += '</tr>';
        });
    
        if (tableBody !== '') {
    
            $('#search_bdy').html(tableBody);
            //create pager based on results
    
            $('#reccount').html(data.Total + ' Events');
    
            var pagerparams = { ParentElement: 'pager',
                    Batch: data.Batch,
                    BatchLength: data.BatchLength,
                    Total: data.Total,
                    Function: this.getLink,
                    Context: this
                };
    
            this.pager.createpager(pagerparams);
     
        }
        else {
            $('#search_bdy').html(tableBody);
            $('#reccount').html('0 Persons');
        }
    },
    getLink: function(toPage) {
    
        this.qryStrUtils.updateQryPar('page', toPage);
        this.getEvents();
    
    },
    sort: function(sort_col) {
    
        this.qryStrUtils.sort_inner(sort_col);
        this.getEvents();
    }

    
    
};







 











