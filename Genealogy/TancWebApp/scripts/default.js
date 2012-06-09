

$(document).ready(function () {



    createHeader('#1', getPerson);

 
});

getPerson = function () {

    var params = {};

    params[0] = 'hello';

    twaGetJSON("/TestLogin", params, processData);

    return false;
}


processData = function (data) {



}


 