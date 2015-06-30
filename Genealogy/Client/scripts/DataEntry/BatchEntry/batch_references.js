
var BatchReferences = function (grid, batchcore) {
    this.editableGrid = grid;

    this.rowcount = 0;

    this.sourceparam = 'scs';
    

    this.ancUtils = new AncUtils();
    this.qryStrUtils = new QryStrUtils();
    this.batchCore = batchcore;
}


BatchReferences.prototype.displayReferences = function (rowsrequired, displayData) {

    // this approach is interesting if you need to dynamically create data in Javascript 
    //var total = Number($('#txtRows').val());

    //  var total = Number($('#txtRows').val());




    var params = {};

    params[0] = '';
    params[1] = '0';
    params[2] = '100';
    params[3] = 'SourceTypeDesc';

    this.ancUtils.twaGetJSON('/SourceTypes/Select', params, function (data) {
        var content = '';

        $.each(data.serviceSources, function (source, sourceInfo) {
            content += '<option>' + sourceInfo.TypeId + ' ' + sourceInfo.Description + '</option>';
        });

        content = '<b>Source Type Lookup</b><br/> <select>' + content + '</select>';

        $('#message').html(content);
    });



    this.rowcount = rowsrequired;

    displayData.metadata.push({ name: "InValid", label: "Inv.", datatype: "boolean", editable: true, class: 'colBoolWidth' });
    displayData.metadata.push({ name: "sourceId", label: "Id", datatype: "string", editable: false, class: 'colIdWidth' });
    displayData.metadata.push({ name: "sourceRef", label: "Ref.", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "sourceDesc", label: "Desc.", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "sourceDateStr", label: "Date", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "sourceDateStrTo", label: "Date To", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "originalLocation", label: "Original Location", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "sourceNotes", label: "Notes", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "parishs", label: "Parish Id", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "sourceTypes", label: "Source Type", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "isCopyHeld", label: "Copy Held", datatype: "boolean", editable: true, class: 'default' });
    displayData.metadata.push({ name: "isViewed", label: "Viewed", datatype: "boolean", editable: true, class: 'default' });
    displayData.metadata.push({ name: "isThackrayFound", label: "Thackray Found", datatype: "boolean", editable: true, class: 'default' });

    var idx = 1;

    while (idx < this.rowcount) {

        displayData.data.push({ id: idx, values: {
            "InValid": true,
            "sourceRef": "",
            "sourceDesc": "",
            "sourceDateStr": "",
            "sourceDateStrTo": "",
            "originalLocation": "",
            "sourceNotes": "",
            "parishs": "",
            "sourceTypes": "",
            "isCopyHeld": false,
            "isViewed": false,
            "isThackrayFound": false
        }
        });
        idx++;
    }


}

BatchReferences.prototype.GetRefenceRecord = function (rowIdx) {

    var theData = {};
    theData.sourceId = this.editableGrid.getValueAt(rowIdx, 1);
    theData.isCopyHeld = this.editableGrid.getValueAt(rowIdx, 10);
    theData.isThackrayFound = this.editableGrid.getValueAt(rowIdx, 11);
    theData.isViewed = this.editableGrid.getValueAt(rowIdx, 12);

    theData.originalLocation = this.editableGrid.getValueAt(rowIdx, 6);
    theData.sourceDesc = this.editableGrid.getValueAt(rowIdx, 3);
    theData.sourceRef = this.editableGrid.getValueAt(rowIdx, 2);
    theData.sourceNotes = this.editableGrid.getValueAt(rowIdx, 7); //mother name
    theData.sourceDateStr = this.editableGrid.getValueAt(rowIdx, 4); //mother surname
    theData.sourceDateStrTo = this.editableGrid.getValueAt(rowIdx, 5);
    theData.sourceFileCount = 0;
    theData.parishs = this.editableGrid.getValueAt(rowIdx, 8);
    theData.sourceTypes = this.editableGrid.getValueAt(rowIdx, 9);
    theData.fileIds = '';


    return theData;
}

BatchReferences.prototype.ValidateReferences = function (rowIdx) {

    var isValidRow = true;

    var personRecord = this.GetRefenceRecord(rowIdx);

    if (personRecord.sourceDesc == '')
        isValidRow = false;

    if (personRecord.sourceRef == '')
        isValidRow = false;

    if (personRecord.sourceDateStr == '')
        isValidRow = false;

    if (personRecord.sourceDateStrTo == '')
        isValidRow = false;

    if (personRecord.parishs == '')
        isValidRow = false;

    return isValidRow;
}

BatchReferences.prototype.saveReference = function (rowIdx) {
    var args = { rowid: rowIdx, data: '' };

    var postParams = {
        url: '/Sources/Add',
        data: this.GetRefenceRecord(rowIdx),
        idparam: 'id',
        refreshmethod: $.proxy(this.batchCore.recordAdded, this.batchCore),
        refreshArgs: args,
        Context: this
    };

    this.ancUtils.twaPostJSON(postParams);
}


