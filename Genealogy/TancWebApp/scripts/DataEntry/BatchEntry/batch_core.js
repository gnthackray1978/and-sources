
$.fn.pasteEvents = function (delay) {
    if (delay == undefined) delay = 20;
    return $(this).each(function () {
        var $el = $(this);
        $el.on("paste", function () {
            $el.trigger("prepaste");
            setTimeout(function () { $el.trigger("postpaste"); }, delay);
        });
    });
};



//window.onload = function () {

//    createHeader('#1',run);

//}

$(document).ready(function () {

    createHeader('#1', run);
});




run = function () {

    getSourceLst();
    getParishLst();



    $("#tablecontent").on("postpaste", function () {

        var result = $("#tablecontent input:text").val();
        //  editableGrid.getCell(editableGrid.currentCellX, editableGrid.currentCellY);

        var idx = 0;
        var rowIdx = editableGrid.currentCellX;
        var columnIndex = editableGrid.currentCellY;

        $("#txtSource").focus();

        var rows = result.split('\x20');


        while (idx < rows.length) {

            var colIdx = editableGrid.currentCellY;

            var cols = rows[idx].split('\x09');

            var cidx = 0;

            while (cidx < cols.length) {

                var _value = cols[cidx];

                editableGrid.setValueAt(rowIdx, colIdx, _value);

                colIdx++;
                cidx++;
            }
            rowIdx++;
            idx++;
        }

    }).pasteEvents();
}


selectParish = function () {

    var _loc = window.location.hash;


    _loc = updateStrForQry(_loc, 'parl', '');
    _loc = _loc.replace('#', '');

    var url = '../HtmlPages/ParishSearch.html#' + _loc;

    window.location.href = url;
}





selectSource = function () {


    var _loc = window.location.hash;

    _loc = updateStrForQry(_loc, 'ldrl', '0');
    _loc = updateStrForQry(_loc, 'ldru', '0');
    _loc = updateStrForQry(_loc, 'udrl', '2000');
    _loc = updateStrForQry(_loc, 'udru', '2000');
    _loc = updateStrForQry(_loc, 'scs', '');

    _loc = _loc.replace('#', '');


    var url = '../HtmlPages/SourceSearch.html#' + _loc;

    window.location.href = url;

}


Display = function () {

    var displayData = {
        metadata: [],
        data: []
    };

    // var test = test123();

    var _isValidSources = isValidSources();
    var _isValidParishs = isValidParishs();


    var chkBirths = $('#chkIncludeBirths').prop('checked');

    var chkDeaths = $('#chkIncludeDeaths').prop('checked');

    var chkRefs = $('#chkIncludeRefs').prop('checked');


    if (_isValidSources && _isValidParishs && (chkBirths || chkDeaths || chkRefs)) {

        $("#footer").show();

        if (chkBirths) {
            displayBirths(displayData);
        }

        if (chkDeaths) {
            displayDeaths(displayData);
        }

        if (chkRefs) {
            displayReferences(displayData);
        }

        editableGrid = new EditableGrid("DemoGrid");
        editableGrid.load({ "metadata": displayData.metadata, "data": displayData.data });
        editableGrid.renderGrid("tablecontent", "testgrid");

        var rowCount = editableGrid.getRowCount();

        if (rowCount > 10) {

            var workingTotal = rowCount - 10;

            workingTotal = workingTotal * 35;

            workingTotal = workingTotal + 800;

            $('.maincontent2').css("height", String(workingTotal) + "px");
        }
        else {
            $('.maincontent2').css("height", "800px");
        }

        editableGrid.modelChanged = function (rowIndex, columnIndex, oldValue, newValue, row) {

            var rowIdx = 0;
            var colCount = editableGrid.getColumnCount();

            while (rowIdx < editableGrid.getRowCount()) {

                var isValidRow = false;

                var personRecord = GetBirthRecord(rowIdx);

                if (chkBirths) {
                    isValidRow = ValidateBirths(rowIdx);
                }

                if (chkDeaths) {
                    isValidRow = ValidateDeaths(rowIdx);
                }


                if (isValidRow && _isValidParishs && _isValidSources)
                    this.setValueAt(rowIdx, 0, false);
                else
                    this.setValueAt(rowIdx, 0, true);


                rowIdx++;
            }

        };

    }
    else {
        $("#footer").hide();
    }





}



Save = function () {

    //getRowCount
    //getColumnCount

    //editableGrid.getValueAt( 
    var colCount = editableGrid.getColumnCount();

    var rowIdx = 0;
    while (rowIdx < editableGrid.getRowCount()) {
        switch ($('input[name=recType]:checked').val()) {
            case 'births':
                var _birth = GetBirthRecord(rowIdx);
                savePerson(_birth);
                break;
            case 'deaths':
                var _death = GetDeathRecord(rowIdx);
                savePerson(_death);
                break;
            case 'references':
                saveReference();
                break;
        }
        rowIdx++;
    }

    var display = 'xx';
    $('#templabel').html(display);
}


recordAdded = function () {


}