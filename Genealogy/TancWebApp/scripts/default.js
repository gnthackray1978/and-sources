



$(document).ready(function () {


    var jsMaster = new JSMaster();
    jsMaster.generateHeader('#1', getPerson);

   // createHeader('#1', getPerson);

 
});

getPerson = function () {

    var params = {};

    params[0] = 'hello';

    twaGetJSON("/TestLogin", params, function (data) { } );

    return false;
}



 