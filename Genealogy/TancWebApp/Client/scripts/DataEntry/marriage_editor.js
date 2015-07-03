
var JSMaster, QryStrUtils, AncUtils,AncSelectorSources;

 

$(document).ready(function () {
    var jsMaster = new JSMaster();


    console.log('marriage editor ready');

    jsMaster.generateHeader('#1', function () {
        
        var ancMarriageEditor = new AncMarriageEditor();
        ancMarriageEditor.init();

    });

});


var AncMarriageEditor = function () {
    this.qryStrUtils = new QryStrUtils();
    this.ancUtils = new AncUtils();
    this.DEFAULT_GET_URL = '/MarriageService/Marriage';
    this.DEFAULT_ADD_URL = '/MarriageService/Add';
    this.DEFAULT_SEARCHPAGE_URL = '../HtmlPages/MarriageSearch.html';

    this.postParams = {

        url: '',
        data: '',
        idparam: 'id',
        refreshmethod: this.load,
        refreshArgs: undefined,
        Context: this
    };
};

AncMarriageEditor.prototype = {

    init: function () {

        console.log('marriage editor init');
        $('body').on("click","#save", $.proxy(function () { this.save(); return false; }, this));

        $('body').on("click","#return", $.proxy(function () { this.saveReturn(); return false; }, this));


        this.load();

        return false;

    },

    load: function () {
        var params = {};
        params[0] = this.qryStrUtils.getParameterByName('id', '');

        this.ancUtils.twaGetJSON(this.DEFAULT_GET_URL, params, $.proxy(this.processData, this));
    },

    processData: function (data) {

        console.log('marriage editor recieved data');

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
        $('#txtWitDesc1').val(data.Witness1Description);

        $('#txtWit2c').val(data.Witness2ChristianName);
        $('#txtWit2').val(data.Witness2Surname);
        $('#txtWitDesc2').val(data.Witness2Description);

        $('#txtWit3c').val(data.Witness3ChristianName);
        $('#txtWit3').val(data.Witness3Surname);
        $('#txtWitDesc3').val(data.Witness3Description);

        $('#txtWit4c').val(data.Witness4ChristianName);
        $('#txtWit4').val(data.Witness4Surname);
        $('#txtWitDesc4').val(data.Witness4Description);



        $('#txtWit5c').val(data.Witness5ChristianName);
        $('#txtWit5').val(data.Witness5Surname);
        $('#txtWitDesc5').val(data.Witness5Description);

        $('#txtWit6c').val(data.Witness6ChristianName);
        $('#txtWit6').val(data.Witness6Surname);
        $('#txtWitDesc6').val(data.Witness6Description);

        $('#txtWit7c').val(data.Witness7ChristianName);
        $('#txtWit7').val(data.Witness7Surname);
        $('#txtWitDesc7').val(data.Witness7Description);

        $('#txtWit8c').val(data.Witness8ChristianName);
        $('#txtWit8').val(data.Witness8Surname);
        $('#txtWitDesc8').val(data.Witness8Description);
        




        $('#txtMaleLoc').val(data.MaleLocation);
        $('#txtFemaleLoc').val(data.FemaleLocation);

        $('#txtMBYr').val(data.MaleBirthYear);
        $('#txtFBYr').val(data.FemaleBirthYear);

        $('#txtMOcc').val(data.MaleOccupation);
        $('#txtFOcc').val(data.FemaleOccupation);

        $('#txtMNot').val(data.MaleNotes);

        $('#txtFNot').val(data.FemaleNotes);

        if (data.IsWidow === false) {
            $('#chkIsWid').prop('checked', false);
        }
        else {
            $('#chkIsWid').prop('checked', true);
        }

        if (data.IsWidower === false) {
            $('#chkIsWiw').prop('checked', false);
        }
        else {
            $('#chkIsWiw').prop('checked', true);
        }


        if (data.IsBanns === false) {
            $('#chkIsBann').prop('checked', false);
        }
        else {
            $('#chkIsBann').prop('checked', true);
        }

        if (data.IsLicense === false) {
            $('#chkIsLic').prop('checked', false);
        }
        else {
            $('#chkIsLic').prop('checked', true);
        }


        this.qryStrUtils.updateQryPar('source_ids', data.Sources);


        //getSources('#sourceselector');

        var ancSelectorSources = new AncSelectorSources();

        ancSelectorSources.getSources('#sourceselector');

        console.log('marriage editor requested source data');
    },

    GetMarriageRecord: function (rowIdx) {
        // theData.SourceDescription = $('#txtSource').val();
        var record = {};
        record.FemaleLocationId = '';
        record.LocationId = '';
        record.MaleLocationId = '';
        record.SourceDescription = $('#txtSource').val();
        record.Sources = this.qryStrUtils.getParameterByName('source_ids', '');
        record.MarriageId = this.qryStrUtils.getParameterByName('id', '');
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




        //        var _obj = [
        //                { name: 'john', surname: 'smith', description: 'witness' },
        //                { name: 'chris', surname: 'jones', description: 'witness' },
        //                { name: 'allen', surname: 'bond', description: 'vicar' }        
        //                ];


        var _obj = [];

        var idx = 1;

        while (idx < 9) {
            var cname = '#txtWit' + idx + 'c';
            var sname = '#txtWit' + idx;
            var description = '#txtWitDesc' + idx;

            if ($(cname).val() != '' || $(sname).val() != '' || $(description).val() != '') {
                _obj.push({ name: $(cname).val(), surname: $(sname).val(), description: $(description).val() });
            }
            idx++;
        }

        record.MarriageWitnesses = JSON.stringify(_obj);
        //        record.Witness1ChristianName = $('#txtWit1c').val();
        //        record.Witness1Surname = $('#txtWit1').val();
        //        record.Witness2ChristianName = $('#txtWit2c').val();
        //        record.Witness2Surname = $('#txtWit2').val();
        //        record.Witness3ChristianName = $('#txtWit3c').val();
        //        record.Witness3Surname = $('#txtWit3').val();
        //        record.Witness4ChristianName = $('#txtWit4c').val();
        //        record.Witness4Surname = $('#txtWit4').val();

        return record;
    },
    save: function () {
        this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetMarriageRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    },

    saveReturn: function () {
        this.postParams.refreshmethod = function () { window.location = this.DEFAULT_SEARCHPAGE_URL + window.location.hash; };
        this.postParams.url = this.DEFAULT_ADD_URL;
        this.postParams.data = this.GetMarriageRecord();
        this.ancUtils.twaPostJSON(this.postParams);
    }

};
