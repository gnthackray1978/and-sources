
displayReferences = function (displayData) {
    // this approach is interesting if you need to dynamically create data in Javascript 
    var metadata = [];

    displayData.metadata.push({ name: "IsValid", label: "Is Valid", datatype: "boolean", editable: true, class: 'colBoolWidth' });
    displayData.metadata.push({ name: "IsMale", label: "Sex", datatype: "boolean", editable: true, class: 'colBoolWidth' });
    displayData.metadata.push({ name: "ChristianName", label: "Name", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "BirthLocation", label: "Birth Location", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "BirthDateStr", label: "Birth Date", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "BaptismDateStr", label: "Baptism Date", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "DeathDateStr", label: "Death Date", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "DeathLocation", label: "Death Location", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "FatherChristianName", label: "Father Name", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "MotherChristianName", label: "Mother Name", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "MotherSurname", label: "Mother Surname", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "Notes", label: "Notes", datatype: "string", editable: true, class: 'default' });

    displayData.metadata.push({ name: "Occupation", label: "Occupation", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "ReferenceLocation", label: "Reference Location", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "ReferenceDateStr", label: "Reference Date", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "SpouseName", label: "Spouse Name", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "SpouseSurname", label: "Spouse Surname", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "FatherOccupation", label: "Father Occupation", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeYear", label: "Age Year", datatype: "int", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeMonth", label: "Age Month", datatype: "string", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeDay", label: "Age Days", datatype: "int", editable: true, class: 'default' });
    displayData.metadata.push({ name: "AgeWeek", label: "Age Weeks", datatype: "int", editable: true, class: 'default' });



    displayData.data.push({ id: 1, values: {
        "IsValid": false,
        "IsMale": false,
        "ChristianName": "",

        "BirthLocation": "",
        "BirthDateStr": "",
        "BaptismDateStr": "",
        "DeathDateStr": "",
        "DeathLocation": "",
        "FatherChristianName": "",

        "MotherChristianName": "",
        "MotherSurname": "",
        "Notes": "",

        "Occupation": "",
        "ReferenceLocation": "",
        "ReferenceDateStr": "",

        "SpouseName": "",
        "SpouseSurname": "",
        "FatherOccupation": "",
        "AgeYear": "",
        "AgeMonth": "",
        "AgeDay": "",
        "AgeWeek": ""

    }

    });


}
