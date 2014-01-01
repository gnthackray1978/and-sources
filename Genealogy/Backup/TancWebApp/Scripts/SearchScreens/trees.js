var JSMaster, QryStrUtils,AncUtils,Panels;



$(document).ready(function () {
    var jsMaster = new JSMaster();


    jsMaster.generateHeader('#1', function () {
        var ancTreeSearch = new AncTreeSearch();
        ancTreeSearch.init();

    });

});


var AncTreeSearch = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    
    this.postParams = { 
        url: '',
        data: '',
        idparam: undefined,
        refreshmethod: this.getTreeSources,
        refreshArgs: ['1'],
        Context: this
    };
};


AncTreeSearch.prototype = {
    init: function () {


        var isActive = this.qryStrUtils.getParameterByName('active', '');

        //        if (active != undefined)
        //            isActive = '1';

        var panels = new Panels();

        $("#main").live("click", $.proxy(function () { panels.sourcesShowPanel('1'); return false; }, panels));
        $("#persons").live("click", $.proxy(function () { panels.sourcesShowPanel('2'); return false; }, panels));
        $("#imports").live("click", $.proxy(function () { panels.sourcesShowPanel('3'); return false; }, panels));
        $("#refresh").live("click", $.proxy(function () { this.getTreeSources('1'); return false; }, this));
        $("#deltree").live("click", $.proxy(function () { this.deleteTree(); return false; }, this));
        $("#newtree").live("click", $.proxy(function () { this.newTree(); return false; }, this));
        $("#savetree").live("click", $.proxy(function () { this.saveTree(); return false; }, this));


        if (isActive == '1') {

            $('#txtDescFilter').val(this.qryStrUtils.getParameterByName('desc', ''));

            this.getTreeSources();

        }


    },

    getTreeSources: function (active) {

        var params = {};
        params[0] = $('#txtDescFilter').val();
        params[1] = String(this.qryStrUtils.getParameterByName('page', 0));
        params[2] = '25';

        this.ancUtils.twaGetJSON('/GetTreeSources/Select', params, $.proxy(this.writeTreeSources, this));

        this.createQryString();


        return false;
    },

    createQryString: function () {
        var args = {
            "active": '1',
            "desc": $('#txtDescFilter')
        };
        this.qryStrUtils.updateQry(args);
    },

    getLink: function (toPage) {
        this.qryStrUtils.updateQryPar('page', toPage);
        this.getTreeSources();
    },
    deleteTree: function () {
        this.postParams.url = '/Trees/DeleteTree';
        this.postParams.data = { treeId: this.qryStrUtils.getParameterByName('selectedTree', '') };
        this.ancUtils.twaPostJSON(this.postParams);
    },
    writeTreeSources: function (data) {

        var tableBody = '';
        var selectEvents = [];
        var _idx = 0;
        //    var that = this;

        var selectedTree = this.qryStrUtils.getParameterByName('selectedTree', '');

        $.each(data.serviceSources, function (source, sourceInfo) {

            var hidfield = '<input type="hidden" name="source_id" id="source_id" value ="' + sourceInfo.SourceId + '"/>';

            if (selectedTree == sourceInfo.SourceId) {
                tableBody += '<tr class = "highLightRow">' + hidfield;
            }
            else {
                tableBody += '<tr>' + hidfield;
            }

            tableBody += '<td><div>' + sourceInfo.SourceYear + '</div></td>';
            tableBody += '<td><div>' + sourceInfo.SourceYearTo + '</div></td>';

            tableBody += '<td><a  id= "s' + _idx + '" href="" ><div>' + sourceInfo.SourceRef + '</div></a></td>';

            selectEvents.push({ key: 's' + _idx, value: { sid: sourceInfo.SourceId, did: sourceInfo.DefaultPerson} });

            tableBody += '<td><div>' + sourceInfo.SourceDesc + '</div></td>';

            tableBody += '<td><a href="../HtmlPages/AncestorsTree.html#?id=' + sourceInfo.DefaultPerson + '&zoom=100&xpos=0&ypos=600&isAnc=1#Dec"><div>Ancestor view</div></a></td>';

            tableBody += '<td><a href="../HtmlPages/DescendantsTree.html#?sid=' + sourceInfo.SourceId + '&id=' + sourceInfo.DefaultPerson + '&zoom=100&xpos=0&ypos=0&isAnc=1#Dec"><div>Family View</div></a></td>';

            tableBody += '</tr>';

            _idx++;
        });


        if (tableBody !== '') {
            $('#search_bdy').html(tableBody);
            $('#reccount').html(data.Total + ' Trees Found');

            //create pager based on results

            var pagerparams = { ParentElement: 'pager',
                Batch: data.Batch,
                BatchLength: data.BatchLength,
                Total: data.Total,
                Function: this.getLink,
                Context: this
            };
            this.ancUtils.createpager(pagerparams);
        }
        //re select person and tree
        this.treeSelect();

        this.ancUtils.addlinks(selectEvents, this.treeSelect, this);
    },
    treeSelect: function (args) {

        //var args = sid: sourceInfo.SourceId, did: sourceInfo.DefaultPerson 

        if (args !== undefined) {
            this.qryStrUtils.updateQryPar('selectedTree', args.sid);
            this.qryStrUtils.updateQryPar('selectedPerson', args.did);
        }


        // SELECT SOURCE (CLICK SOURCE NAME)
        // gets persons for associated tree
        // high lights selection

        var date = 0;
        var dateTo = 0;
        var sourceRef = '';
        var sourceDesc = '';



        this.getTreePersons();

        var found = false;
        $('#search_bdy .highLightRow').removeClass("highLightRow");

        var selectedTree = this.qryStrUtils.getParameterByName('selectedTree', '');



        // search all rows for the one we want
        $('#search_bdy tr').each(function () {
            var $this = $(this);

            var rowTreeId = $this.find("input").val();
            // search until we find id of the tree (the source)

            if (selectedTree === rowTreeId) {

                found = true;

                $this.toggleClass('highLightRow');

                var idx = 0;

                $(this).find('td span').each(function () {

                    var tp = $(this).text();

                    if (idx === 0)
                        date = tp;

                    if (idx === 1)
                        dateTo = tp;

                    if (idx === 2)
                        sourceRef = tp;

                    if (idx === 3)
                        sourceDesc = tp;

                    idx++;
                });
            }
        }); //endeach

        $('#txtTreeDesc').val(sourceDesc);
        $('#txtTreeRef').val(sourceRef);
        $('#txtTreeDataFrom').val(date);
        $('#txtTreeDataTo').val(dateTo);


        $('#dbutton').removeClass('hidePanel').addClass('displayPanel');
    },
    getTreePersons: function () {

        //var treePersonUrl = getHost() + "/GetTreePersons/Select";
        var treeId = this.qryStrUtils.getParameterByName('selectedTree', '');


        var tableBody = '';


        if (treeId != '00000000-0000-0000-0000-000000000000') {

            var persondata = function (data) {

                if (data != null) {

                    var selectPEvents = [];
                    var count = 0;
                    var _idx = 0;
                    var that = this;
                    $.each(data, function (source, sourceInfo) {
                        count++;
                        var hidfield = '<input type="hidden" name="person_id" id="person_id" value ="' + sourceInfo.PersonId + '"/>';

                        var selectedPerson = that.qryStrUtils.getParameterByName('selectedPerson', '');

                        if (sourceInfo.PersonId == selectedPerson) {
                            tableBody += '<tr class = "highLightRow">' + hidfield + '<td><a id= "p' + _idx + '" href="" "><span>' + sourceInfo.BirthYear + ' ' + sourceInfo.ChristianName + ' ' + sourceInfo.Surname + '</span></a></td></tr>';
                        }
                        else {
                            tableBody += '<tr>' + hidfield + '<td><a id= "p' + _idx + '" href="" ><span>' + sourceInfo.BirthYear + ' ' + sourceInfo.ChristianName + ' ' + sourceInfo.Surname + '</span></a></td></tr>';
                        }

                        selectPEvents.push({ key: 'p' + _idx, value: sourceInfo.PersonId });
                        _idx++;
                    });

                    $('#person_lookup_body').html(tableBody);
                    $('.pSelPer').removeClass('hidePanel').addClass('displayPanel');
                    this.ancUtils.addlinks(selectPEvents, this.processPersonSelect, this);
                }

            };
            var params = {};
            params[0] = treeId;
            params[1] = $('#txtFrom').val();
            params[2] = $('#txtTo').val();

            this.ancUtils.twaGetJSON('/GetTreePersons/Select', params, $.proxy(persondata, this));
        }

        $('#person_lookup_body').html(tableBody);
        $('#pSelPer').removeClass('hidePanel').addClass('displayPanel');

        return false;
    },
    //SET DEFAULT PERSON FOR SELECTED TREE
    processPersonSelect: function (evt) {
        this.qryStrUtils.updateQryPar('selectedPerson', evt);

        $('#person_lookup_body tr').each(function () {
            var $this = $(this);

            var quantity = $this.find("input").val();
            if (evt == quantity) {
                $('#person_lookup_body tr.highLightRow').toggleClass('highLightRow');

                $this.toggleClass('highLightRow');

                var theData = {};

                theData.sourceId = this.qryStrUtils.getParameterByName('selectedTree', '');
                theData.personId = this.qryStrUtils.getParameterByName('selectedPerson', '');

                this.postParams.url = '/settreepersons/Set';
                this.postParams.data = theData;
                this.ancUtils.twaPostJSON(this.postParams);
            }

        });        //endeach

    },
    newTree: function () {

        this.qryStrUtils.updateQryPar('selectedTree', '00000000-0000-0000-0000-000000000000');
        this.qryStrUtils.updateQryPar('selectedPerson', '00000000-0000-0000-0000-000000000000');

        //    date = 0;
        //    dateTo = 0;
        //    sourceRef = '';
        //    sourceDesc = '';

        $('#txtTreeDesc').val('');
        $('#txtTreeRef').val('');
        $('#txtTreeDataFrom').val(1500);
        $('#txtTreeDataTo').val(1900);

        $('#dbutton').addClass('hidePanel').removeClass('displayPanel');

        this.treeSelect();

    },
    //linked to save button on 2nd panel
    saveTree: function () {
        //if we have a valid sourceid and some data 
        //then we are editting a existing tree
        //
        //if we have a blank or empty sourceid and we have some valid data 
        //then we are adding a new one
        var treeName = $('#fileNameId').val();
        var sourceDesc = $('#txtTreeDesc').val();
        var sourceRef = $('#txtTreeRef').val();
        var date = $('#txtTreeDataFrom').val();
        var dateTo = $('#txtTreeDataTo').val();
        var selectedTree = this.qryStrUtils.getParameterByName('selectedTree', '');

        var isError = false;
        var errorMessage = '';

        if ((isNaN(date)) || (isNaN(dateTo))) {
            isError = true;
            errorMessage += 'Invalid Dates entered';
        }
        else {
            if (date < 1000 || date > 2100 || dateTo < 1000 || dateTo > 2100) {
                isError = true;
                errorMessage += 'Invalid Date Range';
            }
        }

        if (sourceDesc === '') {
            isError = true;
            errorMessage += '\r\nInvalid Description';
        }

        if (sourceRef === '') {
            isError = true;
            errorMessage += '\r\nInvalid Reference';
        }

        if (treeName === '') {
            isError = true;
            errorMessage += '\r\nInvalid Tree Name';
        }

        if (!isError) {

            var theData = {};

            theData.sourceId = String(selectedTree);
            theData.fileName = String(treeName);
            theData.sourceRef = String(sourceRef);
            theData.sourceDesc = String(sourceDesc);
            theData.sourceYear = String(date);
            theData.sourceYearTo = String(dateTo);

            this.postParams.url = '/SaveTree/Save';
            this.postParams.data = theData;
            this.ancUtils.twaPostJSON(this.postParams);

            if (selectedTree === '00000000-0000-0000-0000-000000000000' || selectedTree === '') {
                this.newTree();
            }

            $('#fileupload .fileupload-content').html('<table class="files"></table>');

        }
        else {
            alert(errorMessage);
        }


    }

};

 
 

// this can find out what page its on based on the
// returned data.








  
