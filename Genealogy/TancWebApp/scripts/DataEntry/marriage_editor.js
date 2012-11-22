



//$(document).ready(function () {

//    //http://localhost:666/Forms/FrmEditorMarriages.aspx?_parentId=&active=1&mcname=test&msname=&fcname=&fsname=&locat=&ldrl=1700&ldru=1799&parid=&id=c2c2c673-b10b-42a3-8922-0c0bc1b29144


//    createHeader('#1',getMarriage);
//   // ();
//});



$(document).ready(function () {
    var jsMaster = new JSMaster();
    var ancMarriageEditor = new AncMarriageEditor();

    jsMaster.generateHeader('#1', function () {
        ancMarriageEditor.init();

    });

});





var AncMarriageEditor = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
 
}

AncMarriageEditor.prototype = {

    init: function () {
        var params = {};

        $("#save").live("click", $.proxy(function () { this.save(); return false; }, this));

        $("#return").live("click", $.proxy(function () { this.saveReturn(); return false; }, this));


        var id = this.qryStrUtils.getParameterByName('id', '');

        params[0] = id;

        this.ancUtils.twaGetJSON("/Marriages/GetMarriage/Select", params, $.proxy(this.processData, this));

        return false;

    },

    processData: function (data) {

        $('#txtMDate').val(data.MarriageDate);
        $('#txtSource').val(data.SourceDescription);

        $('#txtMaleName').val(data.MaleCName);
        $('#txtMaleSurname').val(data.MaleSName);
        $('#txtFemaleName').val(data.FemaleCName);
        $('#txtFemaleSurname').val(data.FemaleSName);

        $('#txtParish').val(data.MarriageLocation);
        $('#txtCounty').val(data.LocationCounty);

        $('#txtWit1c').val(data.Witness1ChristianName);
        $('#txtWit1').val(data.Witness1Surname);

        $('#txtWit2c').val(data.Witness2ChristianName);
        $('#txtWit2').val(data.Witness2Surname);

        $('#txtWit3c').val(data.Witness3ChristianName);
        $('#txtWit3').val(data.Witness3Surname);

        $('#txtWit4c').val(data.Witness4ChristianName);
        $('#txtWit4').val(data.Witness4Surname);

        $('#txtMaleLoc').val(data.MaleLocation);
        $('#txtFemaleLoc').val(data.FemaleLocation);

        $('#txtMBYr').val(data.MaleBirthYear);
        $('#txtFBYr').val(data.FemaleBirthYear);

        $('#txtMOcc').val(data.MaleOccupation);
        $('#txtFOcc').val(data.FemaleOccupation);

        $('#txtMNot').val(data.MaleNotes);

        $('#txtFNot').val(data.FemaleNotes);

        if (data.IsWidow == false) {
            $('#chkIsWid').prop('checked', false);
        }
        else {
            $('#chkIsWid').prop('checked', true);
        }

        if (data.IsWidower == false) {
            $('#chkIsWiw').prop('checked', false);
        }
        else {
            $('#chkIsWiw').prop('checked', true);
        }


        if (data.IsBanns == false) {
            $('#chkIsBann').prop('checked', false);
        }
        else {
            $('#chkIsBann').prop('checked', true);
        }

        if (data.IsLicense == false) {
            $('#chkIsLic').prop('checked', false);
        }
        else {
            $('#chkIsLic').prop('checked', true);
        }


        this.qryStrUtils.updateQryPar('source_ids', data.Sources);


        getSources('#sourceselector');
    },

    GetMarriageRecord: function (rowIdx) {
        // theData.SourceDescription = $('#txtSource').val();
        var record = {};
        record.FemaleLocationId = '';
        record.LocationId = '';
        record.MaleLocationId = '';
        record.SourceDescription = $('#txtSource').val();
        record.Sources = getParameterByName('source_ids', '');
        record.MarriageId = getParameterByName('id', '');
        record.IsBanns = $('#chkIsBann').prop('checked');
        record.IsLicense = $('#chkIsLic').prop('checked');
        record.IsWidow = $('#chkIsWid').prop('checked');
        record.IsWidower = $('#chkIsWiw').prop('checked');
        record.FemaleBirthYear = $('#txtFBYr').val();
        record.FemaleCName = $('#txtFemaleName').val();
        record.FemaleLocation = $('#txtFemaleLoc').val();
        record.FemaleNotes = $('#txtFNot').val();
        record.FemaleOccupation = $('#txtFOcc').val();
        record.FemaleSName = $('#txtFemaleSurname').val();
        record.LocationCounty = $('#txtCounty').val();
        record.MaleBirthYear = $('#txtMBYr').val();
        record.MaleCName = $('#txtMaleName').val();
        record.MaleLocation = $('#txtMaleLoc').val();
        record.MaleNotes = $('#txtMNot').val();
        record.MaleOccupation = $('#txtMOcc').val();
        record.MaleSName = $('#txtMaleSurname').val();
        record.MarriageDate = $('#txtMDate').val();
        record.MarriageLocation = $('#txtParish').val();

        record.Witness1ChristianName = $('#txtWit1c').val();
        record.Witness1Surname = $('#txtWit1').val();
        record.Witness2ChristianName = $('#txtWit2c').val();
        record.Witness2Surname = $('#txtWit2').val();
        record.Witness3ChristianName = $('#txtWit3c').val();
        record.Witness3Surname = $('#txtWit3').val();
        record.Witness4ChristianName = $('#txtWit4c').val();
        record.Witness4Surname = $('#txtWit4').val();

        return record;
    },

    save: function () {
        var serviceMarriage = this.GetMarriageRecord();
        this.saveMarriage(serviceMarriage);
    },

    saveReturn: function () {
        var serviceMarriage = this.GetMarriageRecord();
        twaPostJSON('/Marriages/Add', serviceMarriage, '../HtmlPages/MarriageSearch.html', 'id');
    },

    saveMarriage: function (serviceMarriage) {
        twaPostJSON('/Marriages/Add', serviceMarriage, '', 'id');
    }


}






//save = function () {
//    var serviceMarriage = GetMarriageRecord();

//    saveMarriage(serviceMarriage);

//}



//saveReturn = function () {

//    var serviceMarriage = GetMarriageRecord();
//    twaPostJSON('/Marriages/Add', serviceMarriage, '../HtmlPages/MarriageSearch.html','id');
//}



//saveMarriage = function (serviceMarriage) {

//    twaPostJSON('/Marriages/Add', serviceMarriage, '', 'id');
//}


//function getMarriage() {

//    var params = {};

//    var id = getParameterByName('id', '');

//    params[0] = id;

//    twaGetJSON("/Marriages/GetMarriage/Select", params, processData);

//    return false;
//}


//function processData(data) {



//    $('#txtMDate').val(data.MarriageDate);
//    $('#txtSource').val(data.SourceDescription);

//    $('#txtMaleName').val(data.MaleCName);
//    $('#txtMaleSurname').val(data.MaleSName);
//    $('#txtFemaleName').val(data.FemaleCName);
//    $('#txtFemaleSurname').val(data.FemaleSName);

//    $('#txtParish').val(data.MarriageLocation);
//    $('#txtCounty').val(data.LocationCounty);

//    $('#txtWit1c').val(data.Witness1ChristianName);
//    $('#txtWit1').val(data.Witness1Surname);

//    $('#txtWit2c').val(data.Witness2ChristianName);
//    $('#txtWit2').val(data.Witness2Surname);

//    $('#txtWit3c').val(data.Witness3ChristianName);
//    $('#txtWit3').val(data.Witness3Surname);

//    $('#txtWit4c').val(data.Witness4ChristianName);
//    $('#txtWit4').val(data.Witness4Surname);

//    $('#txtMaleLoc').val(data.MaleLocation);
//    $('#txtFemaleLoc').val(data.FemaleLocation);

//    $('#txtMBYr').val(data.MaleBirthYear);
//    $('#txtFBYr').val(data.FemaleBirthYear);

//    $('#txtMOcc').val(data.MaleOccupation);
//    $('#txtFOcc').val(data.FemaleOccupation);

//    $('#txtMNot').val(data.MaleNotes);

//    $('#txtFNot').val(data.FemaleNotes);

//    if (data.IsWidow == false) {
//        $('#chkIsWid').prop('checked', false);
//    }
//    else {
//        $('#chkIsWid').prop('checked', true);
//    }

//    if (data.IsWidower == false) {
//        $('#chkIsWiw').prop('checked', false);
//    }
//    else {
//        $('#chkIsWiw').prop('checked', true);
//    }


//    if (data.IsBanns == false) {
//        $('#chkIsBann').prop('checked', false);
//    }
//    else {
//        $('#chkIsBann').prop('checked', true);
//    }

//    if (data.IsLicense == false) {
//        $('#chkIsLic').prop('checked', false);
//    }
//    else {
//        $('#chkIsLic').prop('checked', true);
//    }


//    updateQryPar('source_ids', data.Sources);


//    getSources('#sourceselector');
//}



//GetMarriageRecord = function (rowIdx) {
//    // theData.SourceDescription = $('#txtSource').val();
//    var record = {};



//    record.FemaleLocationId = '';
//    record.LocationId = '';
//    record.MaleLocationId = '';
//    record.SourceDescription = $('#txtSource').val();
//    record.Sources = getParameterByName('source_ids', '');
//    record.MarriageId = getParameterByName('id', '');
//    record.IsBanns = $('#chkIsBann').prop('checked');
//    record.IsLicense = $('#chkIsLic').prop('checked');
//    record.IsWidow = $('#chkIsWid').prop('checked');
//    record.IsWidower = $('#chkIsWiw').prop('checked');
//    record.FemaleBirthYear = $('#txtFBYr').val();
//    record.FemaleCName = $('#txtFemaleName').val();
//    record.FemaleLocation = $('#txtFemaleLoc').val();
//    record.FemaleNotes = $('#txtFNot').val();
//    record.FemaleOccupation = $('#txtFOcc').val();
//    record.FemaleSName = $('#txtFemaleSurname').val();
//    record.LocationCounty = $('#txtCounty').val();
//    record.MaleBirthYear = $('#txtMBYr').val();
//    record.MaleCName = $('#txtMaleName').val();
//    record.MaleLocation = $('#txtMaleLoc').val();
//    record.MaleNotes = $('#txtMNot').val();
//    record.MaleOccupation = $('#txtMOcc').val();
//    record.MaleSName = $('#txtMaleSurname').val();
//    record.MarriageDate = $('#txtMDate').val();
//    record.MarriageLocation = $('#txtParish').val();

//    record.Witness1ChristianName = $('#txtWit1c').val();
//    record.Witness1Surname = $('#txtWit1').val();
//    record.Witness2ChristianName = $('#txtWit2c').val();
//    record.Witness2Surname = $('#txtWit2').val();
//    record.Witness3ChristianName = $('#txtWit3c').val();
//    record.Witness3Surname = $('#txtWit3').val();
//    record.Witness4ChristianName = $('#txtWit4c').val();
//    record.Witness4Surname = $('#txtWit4').val();

//    return record;


//}