





$(document).ready(function () {
    var jsMaster = new JSMaster();

    jsMaster.generateHeader('#1', function () {
        var bc = new BatchCore();
        bc.run();

    });

});


var BatchCore = function () {

    this.editableGrid = new EditableGrid("DemoGrid");
    this.bp = new BatchParishs();
    this.bs = new BatchSources();
    this.ancUtils = new AncUtils();
    this.qryStrUtils = new QryStrUtils();
    this.batchBirths = new BatchBirths(this.editableGrid, this);
    this.batchReferences = new BatchReferences(this.editableGrid);

    this.parishparam = 'parl';
    this.sourceparam = 'scs';

    this.includeBirthsId = '#chkIncludeBirths';
    this.includeDeathsId = '#chkIncludeDeaths';
    this.includeRefsId = '#chkIncludeRefs';
    this.rowsId = '#txtRows';
    this._isValidSources = false;
    this._isValidParishs = false;
}

BatchCore.prototype = {

    run: function () {

        $("#display").live("click", $.proxy(function () { this.Display(); return false; }, this));

        $("#save").live("click", $.proxy(function () { this.Save(); return false; }, this));
        $("#selectsource").live("click", $.proxy(function () { this.selectSource(); return false; }, this));
        $("#selectparish").live("click", $.proxy(function () { this.selectParish(); return false; }, this));
        $("#tablecontent").on("postpaste", $.proxy(this.paste, this)).pasteEvents();

        this.bs.getSourceLst();
        this.bp.getParishLst();


    },

    paste: function () {
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

            if (rowIdx < this.editableGrid.data.length) {
             
                while (cidx < cols.length) {
                    var _value = cols[cidx];
                    this.editableGrid.setValueAt(rowIdx, colIdx, _value);
                    colIdx++;
                    cidx++;
                }
            }
            rowIdx++;
            idx++;
        }
    },
    selectParish: function () {
        var _loc = window.location.hash;

        _loc = this.qryStrUtils.updateStrForQry(_loc, this.parishparam, '');
        _loc = _loc.replace('#', '');

        var url = '../HtmlPages/ParishSearch.html#' + _loc;

        window.location.href = url;
    },
    selectSource: function () {
        var _loc = window.location.hash;

        _loc = this.qryStrUtils.updateStrForQry(_loc, 'ldrl', '0');
        _loc = this.qryStrUtils.updateStrForQry(_loc, 'ldru', '0');
        _loc = this.qryStrUtils.updateStrForQry(_loc, 'udrl', '2000');
        _loc = this.qryStrUtils.updateStrForQry(_loc, 'udru', '2000');
        _loc = this.qryStrUtils.updateStrForQry(_loc, this.sourceparam, '');

        _loc = _loc.replace('#', '');


        var url = '../HtmlPages/SourceSearch.html#' + _loc;

        window.location.href = url;

    },
    validateRow: function (rowIndex, columnIndex, oldValue, newValue, row) {

        var rowIdx = 0;
        var colCount = this.editableGrid.getColumnCount();
        var chkBirths = $(this.includeBirthsId).prop('checked');
        var chkDeaths = $(this.includeDeathsId).prop('checked');
        var chkRefs = $(this.includeRefsId).prop('checked');

        var that = this;

        while (rowIdx < this.editableGrid.getRowCount()) {

            var isValidRow = false;

            var personRecord = that.batchBirths.GetBirthRecord(rowIdx);

            if (chkBirths) {
                isValidRow = that.batchBirths.ValidateBirths(rowIdx);
            }

            if (chkDeaths) {
                isValidRow = that.batchBirths.ValidateDeaths(rowIdx);
            }


            if (isValidRow && that._isValidParishs && that._isValidSources)
                this.editableGrid.setValueAt(rowIdx, 0, false);
            else
                this.editableGrid.setValueAt(rowIdx, 0, true);


            rowIdx++;
        }

    },
    Display: function () {

        var displayData = {
            metadata: [],
            data: []
        };

        // var test = test123();


        this._isValidSources = this.bs.isValidSources();
        this._isValidParishs = this.bp.isValidParishs();

        var chkBirths = $(this.includeBirthsId).prop('checked');
        var chkDeaths = $(this.includeDeathsId).prop('checked');
        var chkRefs = $(this.includeRefsId).prop('checked');
        var rowsreq = $(this.rowsId).val();

        if (this._isValidSources && this._isValidParishs && (chkBirths || chkDeaths || chkRefs)) {

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

            this.editableGrid.modelChanged = $.proxy(this.validateRow, this);

        }
        else {
            $("#footer").hide();
        }
    },
    Save: function () {

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
    },
    recordAdded: function () {
        console.log('record added');

    }

}


