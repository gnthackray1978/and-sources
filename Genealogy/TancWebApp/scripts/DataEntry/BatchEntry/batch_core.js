
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


var BatchCore = function (grid) {

    this.batchBirths = new BatchBirths(grid);
    this.parishparam = 'parl';
    this.sourceparam = 'scs';

}

BatchCore.prototype = {

    run: function () {

        getSourceLst();
        getParishLst();

        $("#tablecontent").on("postpaste", function () {

            var result = $("#tablecontent input:text").val();
            //  editableGrid.getCell(editableGrid.currentCellX, editableGrid.currentCellY);

            var idx = 0;
            var rowIdx = this.editableGrid.currentCellX;
            var columnIndex = this.editableGrid.currentCellY;

            $("#txtSource").focus();

            var rows = result.split('\x20');


            while (idx < rows.length) {

                var colIdx = this.editableGrid.currentCellY;

                var cols = rows[idx].split('\x09');

                var cidx = 0;

                while (cidx < cols.length) {

                    var _value = cols[cidx];

                    this.editableGrid.setValueAt(rowIdx, colIdx, _value);

                    colIdx++;
                    cidx++;
                }
                rowIdx++;
                idx++;
            }

        }).pasteEvents();
    }

    , selectParish: function () {
        var _loc = window.location.hash;

        _loc = updateStrForQry(_loc, this.parishparam, '');
        _loc = _loc.replace('#', '');

        var url = '../HtmlPages/ParishSearch.html#' + _loc;

        window.location.href = url;
    }

    , selectSource: function () {
        var _loc = window.location.hash;

        _loc = updateStrForQry(_loc, 'ldrl', '0');
        _loc = updateStrForQry(_loc, 'ldru', '0');
        _loc = updateStrForQry(_loc, 'udrl', '2000');
        _loc = updateStrForQry(_loc, 'udru', '2000');
        _loc = updateStrForQry(_loc, sourceparam, '');

        _loc = _loc.replace('#', '');


        var url = '../HtmlPages/SourceSearch.html#' + _loc;

        window.location.href = url;

    }


    , validateRow: function (rowIndex, columnIndex, oldValue, newValue, row) {

        var rowIdx = 0;
        var colCount = this.editableGrid.getColumnCount();

        while (rowIdx < this.editableGrid.getRowCount()) {

            var isValidRow = false;

            var personRecord = this.batchBirths.GetBirthRecord(rowIdx);

            if (chkBirths) {
                isValidRow = this.batchBirths.ValidateBirths(rowIdx);
            }

            if (chkDeaths) {
                isValidRow = this.batchBirths.ValidateDeaths(rowIdx);
            }


            if (isValidRow && _isValidParishs && _isValidSources)
                this.setValueAt(rowIdx, 0, false);
            else
                this.setValueAt(rowIdx, 0, true);


            rowIdx++;
        }

    }


    , Display: function () {

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
        var rowsreq = $('#txtRows').val();

        if (_isValidSources && _isValidParishs && (chkBirths || chkDeaths || chkRefs)) {

            $("#footer").show();

            if (chkBirths) {

                this.batchBirths.displayBirths(rowsreq, displayData);
            }

            if (chkDeaths) {
                this.batchBirths.displayDeaths(rowsreq, displayData);
            }

            if (chkRefs) {
                displayReferences(displayData);
            }

            this.editableGrid = new EditableGrid("DemoGrid");
            this.editableGrid.load({ "metadata": displayData.metadata, "data": displayData.data });
            this.editableGrid.renderGrid("tablecontent", "testgrid");

            var rowCount = this.editableGrid.getRowCount();

            if (rowCount > 10) {

                var workingTotal = rowCount - 10;

                workingTotal = workingTotal * 35;

                workingTotal = workingTotal + 800;

                $('.maincontent2').css("height", String(workingTotal) + "px");
            }
            else {
                $('.maincontent2').css("height", "800px");
            }

            this.editableGrid.modelChanged = validateRow;

        }
        else {
            $("#footer").hide();
        }
    }

    , Save: function () {
 
        var selectiontype = $('input[name=recType]:checked').val();

        switch (selectiontype) {
            case 'births':
            case 'deaths':
                this.batchBirths.setcommondata($('#txtSurname'), $('#txtFatherSurname'), $('#txtSource'), $('#txtBirthCounty'));                
                break;
            case 'references':

                break;
        }
        
        var rowIdx = 0;
        while (rowIdx < this.editableGrid.getRowCount()) {
            switch (selectiontype) {
                case 'births':                    
                    this.batchBirths.saveBirth(rowIdx);
                    break;
                case 'deaths':
                    this.batchBirths.saveDeath(rowIdx);
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

}


recordAdded = function () {


}