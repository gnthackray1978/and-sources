


$(document).ready(function () {
    createHeader('#1');
    refreshData();
});

// called from refresh button that gives it page 0 as default.
function getTreeSources(page) {

    if (isNaN(page))
        page = 0;

    updateQryPar('active', '1');
    updateQryPar('desc', $('#txtDescFilter').val());
    updateQryPar('page', page);

    refreshData();
}

// called from pager
function getLink(toPage) {
    // replace the previous page in the query string with the new one we want
    window.location.hash = window.location.hash.replace('page=' + getParameterByName('page'), 'page=' + toPage);
    refreshData();
}


function deleteTree() {
    //var deleteSourceUrl = getHost() + "/Trees/DeleteTree";
    var selectedTree = getParameterByName('selectedTree');
    var theData = {};

    if (selectedTree.length > 5) {

        theData.treeId = selectedTree;

        twaPostJSON('/Trees/DeleteTree', theData, '', '', function (args) {
            refreshWithErrorHandler(refreshData, args);
        });

//        var stringy = JSON.stringify(theData);

//        $.ajax({
//            cache: false,
//            type: "POST",
//            async: false,
//            url: deleteSourceUrl,
//            data: stringy,
//            contentType: "application/json",
//            dataType: "json",
//            success: function (department) {
//                refreshData();
//            }
//        });

    }
    else {
        showError('No Tree Selected : ' + selectedTree);
    }
}

// refresh tree sources with data from the query string
function refreshData(param) {
    //var url = getHost() + "/GetTreeSources/Select";
    var isActive = getParameterByName('active');

    if (isActive == '1') {

        var params = {};

        params[0] = getParameterByName('desc');
        params[1] = getParameterByName('page');
        params[2] = '25';
      //  $.getJSON(url, params, writeTreeSources);

        twaGetJSON('/GetTreeSources/Select', params, writeTreeSources);
    }

}

// this can find out what page its on based on the
// returned data.
function writeTreeSources(data) {
    
    var tableBody = '';

    var selectedTree = getParameterByName('selectedTree');

    $.each(data.serviceSources, function (source, sourceInfo) {
        //<a href='' class="button" ><span>Main</span></a>
        var hidfield = '<input type="hidden" name="source_id" id="source_id" value ="' + sourceInfo.SourceId + '"/>';

        if (selectedTree == sourceInfo.SourceId) {
            tableBody += '<tr class = "highLightRow">' + hidfield;
         //   selectedTreeRow = false;
        }
        else {
            tableBody += '<tr>' + hidfield;
        }

        tableBody += '<td><div>' + sourceInfo.SourceYear + '</div></td>';
        tableBody += '<td><div>' + sourceInfo.SourceYearTo + '</div></td>';
        tableBody += '<td><a href="" onClick ="treeSelect(\'' + sourceInfo.SourceId + '\',\'' + sourceInfo.DefaultPerson + '\');return false"><div>' + sourceInfo.SourceRef + '</div></a></td>';
        tableBody += '<td><div>' + sourceInfo.SourceDesc + '</div></td>';

        tableBody += '<td><a href="../HtmlPages/AncestorsTree.html#?id=' + sourceInfo.DefaultPerson + '&zoom=100&xpos=0&ypos=600&isAnc=1#Dec"><div>Ancestor view</div></a></td>';

        tableBody += '<td><a href="../HtmlPages/DescendantsTree.html#?sid=' + sourceInfo.SourceId + '&id=' + sourceInfo.DefaultPerson + '&zoom=100&xpos=0&ypos=0&isAnc=1#Dec"><div>Family View</div></a></td>';

        tableBody += '</tr>';
    });


    if (tableBody != '') {
        $('#search_bdy').html(tableBody);


        $('#reccount').html(data.Total + ' Trees Found');

        //create pager based on results

        $('#pager').html(createpager(data.Batch, data.BatchLength, data.Total, 'getLink'));
    }


    //re select person and tree
    treeSelect();
}


// SELECT SOURCE (CLICK SOURCE NAME)
// gets persons for associated tree
// high lights selection
function treeSelect(arg_selectedTree, arg_defaultPerson) {

    var date = 0;
    var dateTo = 0;
    var sourceRef = '';
    var sourceDesc = '';

    if (arg_defaultPerson != undefined && arg_selectedTree != undefined) {
        updateQryPar('selectedTree', arg_selectedTree);
        updateQryPar('selectedPerson', arg_defaultPerson);
    }

    getTreePersons();

    var found = false;
    $('#search_bdy .highLightRow').removeClass("highLightRow");

    var selectedTree = getParameterByName('selectedTree');



    // search all rows for the one we want
    $('#search_bdy tr').each(function () {
        $this = $(this);
        var rowTreeId = $this.find("input").val();
        // search until we find id of the tree (the source)

        if (selectedTree == rowTreeId) {

            found = true;

            $this.toggleClass('highLightRow');

            var idx = 0;

            $(this).find('td span').each(function () {

                var tp = $(this).text();

                if (idx == 0)
                    date = tp;

                if (idx == 1)
                    dateTo = tp;

                if (idx == 2)
                    sourceRef = tp;

                if (idx == 3)
                    sourceDesc = tp;

                idx++;
            })
        }
    }); //endeach

    $('#txtTreeDesc').val(sourceDesc);
    $('#txtTreeRef').val(sourceRef);
    $('#txtTreeDataFrom').val(date);
    $('#txtTreeDataTo').val(dateTo);


    $('#dbutton').removeClass('hidePanel').addClass('displayPanel');
}


function newTree() {

    updateQryPar('selectedTree', '00000000-0000-0000-0000-000000000000');
    updateQryPar('selectedPerson', '00000000-0000-0000-0000-000000000000');

//    date = 0;
//    dateTo = 0;
//    sourceRef = '';
//    sourceDesc = '';

    $('#txtTreeDesc').val('');
    $('#txtTreeRef').val('');
    $('#txtTreeDataFrom').val(1500);
    $('#txtTreeDataTo').val(1900);

    $('#dbutton').addClass('hidePanel').removeClass('displayPanel');

    treeSelect();

}


//linked to save button on 2nd panel
function saveTree() {
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

    if (sourceDesc == '') {
        isError = true;
        errorMessage += '\r\nInvalid Description';
    }

    if (sourceRef == '') {
        isError = true;
        errorMessage += '\r\nInvalid Reference';
    }

    if (treeName == '') {
        isError = true;
        errorMessage += '\r\nInvalid Tree Name';
    }




    if (!isError) {
        // $.post(setDefaultPersonUrl, JSON.stringify(theData));
        //sourceId, string fileName, string sourceRef, string sourceDesc, string sourceYear, string sourceYearTo

       // var saveSourceUrl = getHost() + "/SaveTree/Save";
        var theData = {};

        theData.sourceId = String(selectedTree);
        theData.fileName = String(treeName);
        theData.sourceRef = String(sourceRef);
        theData.sourceDesc = String(sourceDesc);
        theData.sourceYear = String(date);
        theData.sourceYearTo = String(dateTo);


//        var stringy = JSON.stringify(theData);

//        $.ajax({
//            cache: false,
//            type: "POST",
//            async: false,
//            url: saveSourceUrl,
//            data: stringy,
//            contentType: "application/json",
//            dataType: "json",
//            success: function (department) {
//                //  alert('finished');
//            }
//        });

        twaPostJSON('/SaveTree/Save', theData, '', '', function (args) {
            refreshWithErrorHandler(refreshData, args);
        });

        //refresh sources
        //refreshData();       
        

        //<div class="fileupload-content">
        //<table class="files"></table>
        //</div>

        if (selectedTree == '00000000-0000-0000-0000-000000000000' || selectedTree == '') {
            newTree();
        }

        $('#fileupload .fileupload-content').html('<table class="files"></table>');

    }
    else {
        alert(errorMessage);
    }


}


function getTreePersons() {

    //var treePersonUrl = getHost() + "/GetTreePersons/Select";
    var treeId = getParameterByName('selectedTree');
    

    var tableBody = '';
    

    if (treeId != '00000000-0000-0000-0000-000000000000') {
        var theData = {};
        var count = 0;
        theData[0] = treeId;
        theData[1] = $('#txtFrom').val();
        theData[2] = $('#txtTo').val();

        twaGetJSON('/GetTreePersons/Select', theData, function (data) {
        //$.getJSON(treePersonUrl, theData, function (data) {

            $.each(data, function (source, sourceInfo) {
                count++;
                var hidfield = '<input type="hidden" name="person_id" id="person_id" value ="' + sourceInfo.PersonId + '"/>';
                
                var selectedPerson = getParameterByName('selectedPerson');

                if (sourceInfo.PersonId == selectedPerson) {
                    tableBody += '<tr class = "highLightRow">' + hidfield + '<td><a href="" onClick ="processPersonSelect(\'' + sourceInfo.PersonId + '\');return false"><span>'
                    + sourceInfo.BirthYear + ' ' + sourceInfo.ChristianName + ' ' + sourceInfo.Surname + '</span></a></td></tr>';
                }
                else {
                    tableBody += '<tr>' + hidfield + '<td><a href="" onClick ="processPersonSelect(\'' + sourceInfo.PersonId + '\');return false"><span>'
                    + sourceInfo.BirthYear + ' ' + sourceInfo.ChristianName + ' ' + sourceInfo.Surname + '</span></a></td></tr>';

                }
            });

            $('#person_lookup_body').html(tableBody);

            $('.pSelPer').removeClass('hidePanel').addClass('displayPanel');

        });
    }

    $('#person_lookup_body').html(tableBody);

    $('#pSelPer').removeClass('hidePanel').addClass('displayPanel');

    return false;
}



//SET DEFAULT PERSON FOR SELECTED TREE
function processPersonSelect(evt) {

    var setDefaultPersonUrl = getHost() + "/settreepersons/Set";

    updateQryPar('selectedPerson', evt);

    $('#person_lookup_body tr').each(function () {
        $this = $(this)

        var quantity = $this.find("input").val();
        if (evt == quantity) {
            $('#person_lookup_body tr.highLightRow').toggleClass('highLightRow');

            $this.toggleClass('highLightRow');
                    
            var theData = {};

            theData.sourceId = getParameterByName('selectedTree');
            theData.personId = getParameterByName('selectedPerson');

            // $.post(setDefaultPersonUrl, JSON.stringify(theData));

//            var stringy = JSON.stringify(theData);

//            $.ajax({
//                cache: false,
//                type: "POST",
//                async: false,
//                url: setDefaultPersonUrl,
//                data: stringy,
//                contentType: "application/json",
//                dataType: "json",
//                success: function (department) {
//                    //  alert('finished');
//                }
//            });


            twaPostJSON('/settreepersons/Set', theData, '', '');
        }

    });        //endeach

    


}

           


        
