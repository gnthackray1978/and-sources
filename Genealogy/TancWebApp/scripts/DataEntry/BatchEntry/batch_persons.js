
    //    this.elementsurname = $('#txtSurname');
    //    this.elementfathersurname = $('#txtFatherSurname');
    //    this.elementsource = $('#txtSource');
    //    this.elementbirthcounty = $('#txtBirthCounty');
    //Number($('#txtRows').val());


var BatchBirths = function (grid) {
    this.editableGrid = grid;
    this.rowcount = 0;
    this.surname = '';
    this.fathersurname = '';
    this.source = '';
    this.birthcounty = '';
    this.sourceparam = 'scs';
    this.parishparam = 'parl';
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

    displayData.metadata.push({ name: "InValid", label: "Invalid", datatype: "boolean", editable: true, class: 'colBoolWidth' });
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

    displayData.metadata.push({ name: "InValid", label: "Invalid", datatype: "boolean", editable: false, class: 'colBoolWidth' });
    displayData.metadata.push({ name: "IsMale", label: "Sex", datatype: "boolean", editable: true, class: 'colBoolWidth' });
    displayData.metadata.push({ name: "ChristianName", label: "Name", datatype: "string", editable: true, class: 'default' });


    displayData.metadata.push({ name: "DeathLocation", label: "Death Location", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "FatherChristianName", label: "Father Name", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "MotherChristianName", label: "Mother Name", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "MotherSurname", label: "Mother Surname", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "Notes", label: "Notes", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "Occupation", label: "Occupation", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "FatherOccupation", label: "Father Occupation", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "DeathDateStr", label: "Death Date", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "SpouseName", label: "Spouse Name", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "SpouseSurname", label: "Spouse Surname", datatype: "string", editable: true, class: 'default' });


    displayData.metadata.push({ name: "AgeYear", label: "Age Year", datatype: "int", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeMonth", label: "Age Month", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeDay", label: "Age Days", datatype: "int", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeWeek", label: "Age Weeks", datatype: "int", editable: true, class: 'default' });
    displayData.metadata.push({ name: "InValid", label: "Is Valid", datatype: "boolean", editable: true, class: 'colBoolWidth' });

    var idx = 1;


    while (idx < rowcount) {

        displayData.data.push({ id: idx, values: {
            "InValid": true,
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
    theData.personId = '';
    theData.birthparishId = getParameterByName('parl', '');


    theData.sources = getParameterByName('scs', '');
    theData.christianName = this.editableGrid.getValueAt(rowIdx, 2); //name
    theData.surname = $('#txtSurname').val();
    theData.fatherchristianname = this.editableGrid.getValueAt(rowIdx, 4); //father name
    theData.fathersurname = $('#txtFatherSurname').val();
    theData.motherchristianname = this.editableGrid.getValueAt(rowIdx, 5); //mother name
    theData.mothersurname = this.editableGrid.getValueAt(rowIdx, 6); //mother surname
    theData.source = $('#txtSource').val();
    theData.ismale = this.editableGrid.getValueAt(rowIdx, 1); ;
    theData.occupation = this.editableGrid.getValueAt(rowIdx, 8);
    theData.fatheroccupation = this.editableGrid.getValueAt(rowIdx, 9);
    theData.birthDate = this.editableGrid.getValueAt(rowIdx, 10);
    theData.baptismDate = this.editableGrid.getValueAt(rowIdx, 11);

    theData.birthloc = this.editableGrid.getValueAt(rowIdx, 3); //location
    theData.birthcounty = $('#txtBirthCounty').val();
    theData.notes = this.editableGrid.getValueAt(rowIdx, 7); //notes

    theData.datedeath = '';
    theData.deathloc = '';
    theData.deathcounty = '';

    theData.refdate = '';
    theData.refloc = '';

    theData.years = '';
    theData.months = '';
    theData.days = '';
    theData.weeks = '';




    return theData;
}




BatchBirths.prototype.GetDeathRecord = function (rowIdx) {
    var theData = {};

    theData.personId = '';

    theData.birthparishId = getParameterByName('parl', '');
    theData.sources = getParameterByName('scs', '');



    theData.ismale = this.editableGrid.getValueAt(rowIdx, 1); //sex 
    theData.christianName = this.editableGrid.getValueAt(rowIdx, 2); //name
    theData.deathLoc = this.editableGrid.getValueAt(rowIdx, 3); //location
    theData.fatherchristianname = this.editableGrid.getValueAt(rowIdx, 4); //father name
    theData.motherchristianname = this.editableGrid.getValueAt(rowIdx, 5); //mother name
    theData.mothersurname = this.editableGrid.getValueAt(rowIdx, 6); //mother surname
    theData.notes = this.editableGrid.getValueAt(rowIdx, 7); //notes
    theData.occupation = this.editableGrid.getValueAt(rowIdx, 8); //occupation
    theData.fatheroccupation = this.editableGrid.getValueAt(rowIdx, 9); //father occupation

    theData.deathDate = this.editableGrid.getValueAt(rowIdx, 10); //birth or death date

    theData.spouseCName = this.editableGrid.getValueAt(rowIdx, 11); //baptism date or spousename
    theData.spouseSName = this.editableGrid.getValueAt(rowIdx, 12); //baptism date or spousename

    theData.AgeYear = this.editableGrid.getValueAt(rowIdx, 13); // - age year
    theData.AgeMonth = this.editableGrid.getValueAt(rowIdx, 14); // - age month
    theData.AgeDays = this.editableGrid.getValueAt(rowIdx, 15); //- age days
    theData.AgeWeeks = this.editableGrid.getValueAt(rowIdx, 16); //- age weeks

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


BatchBirths.prototype.ValidateDeaths = function () {

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

    //    var localurl = getHost() + '/Person/Add';

    //    var stringy = JSON.stringify(theData);

    //    $.ajax({
    //        cache: false,
    //        type: "POST",
    //        async: false,
    //        url: localurl,
    //        data: stringy,
    //        contentType: "application/json",
    //        dataType: "json",
    //        success: function (department) {
    //            recordAdded();
    //        }

    //    });

    twaPostJSON('/Person/Add', theData,'', function (args) { recordAdded(); });
}


BatchBirths.prototype.saveBirth = function (rowIdx) {
    var _birth = this.GetBirthRecord(rowIdx);
    this.savePerson(_birth);
}


BatchBirths.prototype.saveDeath = function (rowIdx) {
    var _death = this.GetDeathRecord(rowIdx);
    this.savePerson(_death);
}