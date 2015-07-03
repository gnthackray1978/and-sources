
    //    this.elementsurname = $('#txtSurname');
    //    this.elementfathersurname = $('#txtFatherSurname');
    //    this.elementsource = $('#txtSource');
    //    this.elementbirthcounty = $('#txtBirthCounty');
    //Number($('#txtRows').val());


var BatchBirths = function (grid, batchcore) {
    this.DEFAULT_ADD_URL = '/PersonService/Add';

    this.editableGrid = grid;
    this.rowcount = 0;
    this.surname = '';
    this.fathersurname = '';
    this.source = '';
    this.birthcounty = '';
    this.sourceparam = 'scs';
    this.parishparam = 'parl';
    this.ancUtils = new AncUtils();
    this.qryStrUtils = new QryStrUtils();
    this.batchCore = batchcore;
}




BatchBirths.prototype.setcommondata = function (surname, fathersurname, source, birthcounty) {

    this.surname = surname;
    this.fathersurname = fathersurname;
    this.source = source;
    this.birthcounty = birthcounty;
}



BatchBirths.prototype.displayBirths = function (rowsrequired, displayData) {

    // this approach is interesting if you need to dynamically create data in Javascript 
    //var total = Number($('#txtRows').val());

    //  var total = Number($('#txtRows').val());

    this.rowcount = rowsrequired;

    displayData.metadata.push({ name: "InValid", label: "Inv.", datatype: "boolean", editable: true, class: 'colBoolWidth' });
    displayData.metadata.push({ name: "Id", label: "Id", datatype: "string", editable: false, class: 'colIdWidth' });

    displayData.metadata.push({ name: "IsMale", label: "Sex", datatype: "boolean", editable: true, class: 'colBoolWidth' });
    displayData.metadata.push({ name: "ChristianName", label: "Name", datatype: "string", editable: true, class: 'default' });
    // metadata.push({ name: "Surname", label: "Surname", datatype: "string", editable: true , class: 'default' });
    displayData.metadata.push({ name: "BirthLocation", label: "Birth Location", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "FatherChristianName", label: "Father Name", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "MotherChristianName", label: "Mother Name", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "MotherSurname", label: "Mother Surname", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "Notes", label: "Notes", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "Occupation", label: "Occupation", datatype: "string", editable: true, class: 'default' });

    // metadata.push({ name: "SpouseName", label: "Spouse Name", datatype: "string", editable: true, class: 'default' });
    // metadata.push({ name: "SpouseSurname", label: "Spouse Surname", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "FatherOccupation", label: "Father Occupation", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "BirthDateStr", label: "Birth Date", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "BaptismDateStr", label: "Baptism Date", datatype: "string", editable: true, class: 'default' });

    var idx = 1;

    while (idx < this.rowcount) {

        displayData.data.push({ id: idx, values: {
            "InValid": true,
            "Id":"",
            "IsMale": false,
            "ChristianName": "",
            "BirthLocation": "",
            "FatherChristianName": "",
            "MotherChristianName": "",
            "MotherSurname": "",
            "Notes": "",
            "Occupation": "",
            "FatherOccupation": "",
            "BirthDateStr": "",
            "BaptismDateStr": ""
            //    "SpouseName": "",
            //    "SpouseSurname": "",
        }
        });
        idx++;
    }

     
}


BatchBirths.prototype.displayDeaths = function (rowsrequired, displayData) {

    this.rowcount = rowsrequired;
    // this approach is interesting if you need to dynamically create data in Javascript 

   // var total = Number($('#txtRows').val());

    displayData.metadata.push({ name: "InValid", label: "Inv.", datatype: "boolean", editable: false, class: 'colBoolWidth' });
    displayData.metadata.push({ name: "Id", label: "Id", datatype: "string", editable: false, class: 'colIdWidth' });
    displayData.metadata.push({ name: "IsMale", label: "Sex", datatype: "boolean", editable: true, class: 'colBoolWidth' });
    displayData.metadata.push({ name: "ChristianName", label: "Name", datatype: "string", editable: true, class: 'default' });


    displayData.metadata.push({ name: "DeathLocation", label: "Death Loc.", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "FatherChristianName", label: "Father Name", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "MotherChristianName", label: "Mother Name", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "MotherSurname", label: "Mother Surname", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "Notes", label: "Notes", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "Occupation", label: "Occu.", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "FatherOccupation", label: "Father Occ.", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "DeathDateStr", label: "Death Date", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "SpouseName", label: "Spouse Name", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "SpouseSurname", label: "Spouse Surname", datatype: "string", editable: true, class: 'default' });


    displayData.metadata.push({ name: "AgeYear", label: "Age Year", datatype: "int", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeMonth", label: "Age Month", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeDay", label: "Age Days", datatype: "int", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeWeek", label: "Age Weeks", datatype: "int", editable: true, class: 'default' });
   // displayData.metadata.push({ name: "InValid", label: "Is Valid", datatype: "boolean", editable: true, class: 'colBoolWidth' });

    var idx = 1;


    while (idx < this.rowcount) {

        displayData.data.push({ id: idx, values: {
            "InValid": true,
            "Id":"",
            "IsMale": false,
            "ChristianName": "",

            "DeathLocation": "",
            "FatherChristianName": "",
            "MotherChristianName": "",
            "MotherSurname": "",
            "Notes": "",
            "Occupation": "",
            "FatherOccupation": "",

            "DeathDateStr": "",
            "SpouseName": "",
            "SpouseSurname": "",

            "AgeYear": "",
            "AgeMonth": "",
            "AgeDay": "",
            "AgeWeek": ""

        }

        });
        idx++;
    }

    return displayData;
}


BatchBirths.prototype.GetBirthRecord = function (rowIdx) {

    var theData = {};
    theData.personId = this.editableGrid.getValueAt(rowIdx, 1); 
    theData.birthparishId = this.qryStrUtils.getParameterByName('parl', '');


    theData.sources = this.qryStrUtils.getParameterByName('scs', '');
    theData.christianName = this.editableGrid.getValueAt(rowIdx, 3); //name
    theData.surname = $('#txtSurname').val();
    theData.fatherchristianname = this.editableGrid.getValueAt(rowIdx, 5); //father name

    theData.fathersurname = $('#txtFatherSurname').val();
    
    
    theData.motherchristianname = this.editableGrid.getValueAt(rowIdx, 6); //mother name
    theData.mothersurname = this.editableGrid.getValueAt(rowIdx, 7); //mother surname
    theData.source = $('#txtSource').val();
    theData.ismale = this.editableGrid.getValueAt(rowIdx, 2); ;
    theData.occupation = this.editableGrid.getValueAt(rowIdx, 9);
    theData.fatheroccupation = this.editableGrid.getValueAt(rowIdx, 10);
    theData.datebirthstr = this.editableGrid.getValueAt(rowIdx, 11);
    theData.datebapstr = this.editableGrid.getValueAt(rowIdx, 12);

    theData.birthloc = this.editableGrid.getValueAt(rowIdx, 4); //location
    theData.birthcounty = $('#txtBirthCounty').val();
    theData.notes = this.editableGrid.getValueAt(rowIdx, 8); //notes

    theData.datedeath = '';
    theData.deathloc = '';
    theData.deathcounty = '';

    theData.refdate = '';
    theData.refloc = '';

    theData.years = '';
    theData.months = '';
    theData.days = '';
    theData.weeks = '';

    theData.spousesurname = '';
    theData.spousechristianname = '';
    


    return theData;
}


BatchBirths.prototype.GetDeathRecord = function (rowIdx) {
    var theData = {};

    theData.personId = this.editableGrid.getValueAt(rowIdx, 1);

    theData.birthparishId = this.qryStrUtils.getParameterByName('parl', '');
    theData.sources = this.qryStrUtils.getParameterByName('scs', '');



    theData.ismale = this.editableGrid.getValueAt(rowIdx, 2); //sex 
    theData.christianName = this.editableGrid.getValueAt(rowIdx, 3); //name
    theData.surname = $('#txtSurname').val();
    theData.deathloc = this.editableGrid.getValueAt(rowIdx, 4); //location
    theData.birthloc = '';
    theData.birthcounty = $('#txtBirthCounty').val();
    theData.deathcounty = $('#txtDeathCounty').val();

    theData.fatherchristianname = this.editableGrid.getValueAt(rowIdx, 5); //father name

    if (theData.fatherchristianname != '')
        theData.fathersurname = $('#txtFatherSurname').val();
    else
        theData.fathersurname = '';

    theData.motherchristianname = this.editableGrid.getValueAt(rowIdx, 6); //mother name
    theData.mothersurname = this.editableGrid.getValueAt(rowIdx, 7); //mother surname
    theData.notes = this.editableGrid.getValueAt(rowIdx, 8); //notes
    theData.occupation = this.editableGrid.getValueAt(rowIdx, 9); //occupation
    theData.fatheroccupation = this.editableGrid.getValueAt(rowIdx, 10); //father occupation

    theData.datedeath = this.editableGrid.getValueAt(rowIdx, 11); //birth or death date

    theData.datebirthstr = '';
    theData.datebapstr = '';

    theData.refdate = '';
    theData.refloc = '';

    theData.spousechristianname = this.editableGrid.getValueAt(rowIdx, 12); //baptism date or spousename
    theData.spousesurname = this.editableGrid.getValueAt(rowIdx, 13); //baptism date or spousename
    theData.source = $('#txtSource').val();
    theData.years = this.editableGrid.getValueAt(rowIdx, 14); // - age year
    theData.months = this.editableGrid.getValueAt(rowIdx, 15); // - age month
    theData.weeks = this.editableGrid.getValueAt(rowIdx, 16); //- age days
    theData.days = this.editableGrid.getValueAt(rowIdx, 17); //- age weeks

    return theData;
}


BatchBirths.prototype.ValidateBirths = function (rowIdx) {

    var isValidRow = true;

    var personRecord = this.GetBirthRecord(rowIdx);

    if (personRecord.baptismDate == '' && personRecord.birthDate == '')
        isValidRow = false;

    if (personRecord.birthloc == '')
        isValidRow = false;

    if (personRecord.birthcounty == '')
        isValidRow = false;

    if (personRecord.christianName == '')
        isValidRow = false;

    if (personRecord.surname == '')
        isValidRow = false;

    return isValidRow;
}


BatchBirths.prototype.ValidateDeaths = function (rowIdx) {

   // var rowIdx = 0;
    var isValidRow = true;

    //var colCount = this.editableGrid.getColumnCount();
    //  alert(colCount);

    var personRecord = this.GetDeathRecord(rowIdx);

    //while (rowIdx < this.editableGrid.getRowCount()) {

        if (personRecord.christianName == '')
            isValidRow = false;

        if (personRecord.surname == '')
            isValidRow = false;

      //  rowIdx++;
   // }

    return isValidRow;
}


BatchBirths.prototype.savePerson = function (theData) {

    this.ancUtils.twaPostJSON(theData);
  
}


BatchBirths.prototype.saveBirth = function (rowIdx) {
    var args = { rowid: rowIdx, data: '' };

   var postParams = {
       url: this.DEFAULT_ADD_URL,
        data: this.GetBirthRecord(rowIdx),
        idparam: 'id',
        refreshmethod: $.proxy(this.batchCore.recordAdded, this.batchCore),
        refreshArgs: args,
        Context: this
    }; 
    this.savePerson(postParams); 
}


BatchBirths.prototype.saveDeath = function (rowIdx) {
    //  var _death = this.GetDeathRecord(rowIdx);
    // this.savePerson(_death);
    var args = {rowid: rowIdx, data: ''};

    var postParams = {
        url: this.DEFAULT_ADD_URL,
        data: this.GetDeathRecord(rowIdx),
        idparam: 'id',
        refreshmethod: $.proxy(this.batchCore.recordAdded, this.batchCore),
        refreshArgs: args,
        Context: this
    };

    this.savePerson(postParams);
}