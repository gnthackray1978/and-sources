



$(document).ready(function () {

    createHeader('#1', imready);
});

function imready() {
    getPerson();

}



save = function () {
    var servicePerson = GetPersonRecord();

    savePerson(servicePerson);

}



saveReturn = function () {

    twaPostJSON('/Person/Add', GetPersonRecord(), '../HtmlPages/PersonSearch.html', 'id');

}



savePerson = function (servicePerson) {

    twaPostJSON('/Person/Add', GetPersonRecord(), '', 'id');

}


function getPerson() {

    var params = {};

    var id = getParameterByName('id');

    params[0] = id;

    twaGetJSON("/GetPerson/Select", params, processData);

    return false;
}


function processData(data) {

    // public string AddPerson(string personId, string birthparishId,string deathparishId, string referenceparishId, string ismale, string years, string months, string weeks, string days)

    $('#txtNam').val(data.ChristianName);
    $('#txtSur').val(data.Surname);

    $('#txtFatNam').val(data.FatherChristianName);
    $('#txtFatSur').val(data.FatherSurname);
    $('#txtMotNam').val(data.MotherChristianName);
    $('#txtMotSur').val(data.MotherSurname);

    $('#txtBirDat').val(data.Birth);
    $('#txtBapDat').val(data.Baptism);

    $('#txtBirPar').val(data.BirthLocation);
    $('#txtBirCny').val(data.BirthCounty);

    $('#txtSrc').val(data.SourceDescription);


    $('#txtRef').val(data.ReferenceLocation);

    $('#txtRefDat').val(data.ReferenceDate);

    $('#txtSNam').val(data.SpouseChristianName);
    $('#txtSSur').val(data.SpouseSurname);



    $('#txtFOcc').val(data.FatherOccupation);
    $('#txtOcc').val(data.Occupation);

    $('#txtDetDat').val(data.Death);
    $('#txtDetPar').val(data.DeathLocation);

    $('#txtDetCny').val(data.DeathCounty);
    $('#txtNotRdOnly').val(data.Notes);


    updateQryPar('source_ids', data.Sources);


    getSources('#sourceselector');
}



GetPersonRecord = function (rowIdx) {
    var data = {};
    data.personId = getParameterByName('id');
    data.sources = getParameterByName('source_ids');
    data.christianName =$('#txtNam').val();
    data.surname= $('#txtSur').val();
    data.fatherchristianname =$('#txtFatNam').val();
    data.fathersurname= $('#txtFatSur').val();
    data.motherchristianname = $('#txtMotNam').val();
    data.mothersurname  =$('#txtMotSur').val();
    data.datebirthstr = $('#txtBirDat').val();
    data.datebapstr = $('#txtBapDat').val();
    data.birthloc =  $('#txtBirPar').val();
    data.birthcounty =  $('#txtBirCny').val();
    data.source = $('#txtSrc').val();
    data.refloc = $('#txtRef').val();
    data.refdate = $('#txtRefDat').val();
    data.spousechristianname = $('#txtSNam').val();
    data.spousesurname = $('#txtSSur').val();
    data.fatheroccupation = $('#txtFOcc').val();
    data.occupation = $('#txtOcc').val();
    data.datedeath = $('#txtDetDat').val();
    data.deathloc= $('#txtDetPar').val();
    data.deathcounty= $('#txtDetCny').val();
    data.notes= $('#txtNotRdOnly').val();

    return data;


}