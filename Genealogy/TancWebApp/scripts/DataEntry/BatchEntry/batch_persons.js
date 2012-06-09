displayBirths = function (displayData) {

    // this approach is interesting if you need to dynamically create data in Javascript 
    var total = Number($('#txtRows').val());

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

    while (idx < total) {

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






displayDeaths = function (displayData) {


    // this approach is interesting if you need to dynamically create data in Javascript 

    var total = Number($('#txtRows').val());

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


    while (idx < total) {

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

}



GetBirthRecord = function (rowIdx) {

    var theData = {};
    theData.personId = '';
    theData.birthparishId = getParameterByName('parl');


    theData.sources = getParameterByName('scs');
    theData.christianName = editableGrid.getValueAt(rowIdx, 2); //name
    theData.surname = $('#txtSurname').val();
    theData.fatherchristianname = editableGrid.getValueAt(rowIdx, 4); //father name
    theData.fathersurname = $('#txtFatherSurname').val();
    theData.motherchristianname = editableGrid.getValueAt(rowIdx, 5); //mother name
    theData.mothersurname = editableGrid.getValueAt(rowIdx, 6); //mother surname
    theData.source = $('#txtSource').val();
    theData.ismale = editableGrid.getValueAt(rowIdx, 1); ;
    theData.occupation = editableGrid.getValueAt(rowIdx, 8);
    theData.fatheroccupation = editableGrid.getValueAt(rowIdx, 9);
    theData.birthDate = editableGrid.getValueAt(rowIdx, 10);
    theData.baptismDate = editableGrid.getValueAt(rowIdx, 11);

    theData.birthloc = editableGrid.getValueAt(rowIdx, 3); //location
    theData.birthcounty = $('#txtBirthCounty').val();
    theData.notes = editableGrid.getValueAt(rowIdx, 7); //notes

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




GetDeathRecord = function (rowIdx) {
    var theData = {};

    theData.personId = '';
    theData.birthparishId = getParameterByName('parl');


    theData.sources = getParameterByName('scs');

    theData.ismale = editableGrid.getValueAt(rowIdx, 1); //sex 
    theData.christianName = editableGrid.getValueAt(rowIdx, 2); //name
    theData.deathLoc = editableGrid.getValueAt(rowIdx, 3); //location
    theData.fatherchristianname = editableGrid.getValueAt(rowIdx, 4); //father name
    theData.motherchristianname = editableGrid.getValueAt(rowIdx, 5); //mother name
    theData.mothersurname = editableGrid.getValueAt(rowIdx, 6); //mother surname
    theData.notes = editableGrid.getValueAt(rowIdx, 7); //notes
    theData.occupation = editableGrid.getValueAt(rowIdx, 8); //occupation
    theData.fatheroccupation = editableGrid.getValueAt(rowIdx, 9); //father occupation

    theData.deathDate = editableGrid.getValueAt(rowIdx, 10); //birth or death date

    theData.spouseCName = editableGrid.getValueAt(rowIdx, 11); //baptism date or spousename
    theData.spouseSName = editableGrid.getValueAt(rowIdx, 12); //baptism date or spousename

    theData.AgeYear = editableGrid.getValueAt(rowIdx, 13); // - age year
    theData.AgeMonth = editableGrid.getValueAt(rowIdx, 14); // - age month
    theData.AgeDays = editableGrid.getValueAt(rowIdx, 15); //- age days
    theData.AgeWeeks = editableGrid.getValueAt(rowIdx, 16); //- age weeks

    return theData;
}



ValidateBirths = function (rowIdx) {

    var isValidRow = true;

    var personRecord = GetBirthRecord(rowIdx);

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


ValidateDeaths = function () {

    var rowIdx = 0;
    var isValidRow = true;

    var colCount = editableGrid.getColumnCount();
    //  alert(colCount);

    while (rowIdx < editableGrid.getRowCount()) {

        if (personRecord.christianName == '')
            isValidRow = false;

        if (personRecord.surname == '')
            isValidRow = false;

        rowIdx++;
    }

    return isValidRow;
}


savePerson = function (theData) {

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