function writeLine(label, text) {
    //$("#result").val(responseText);

    var existingText = $("#result").val();
    var oldheight = $("#result").height();

    var rowHeight = 22;

    
    if (text.length > 200) rowHeight = 44;
    if (text.length > 300) rowHeight = 66;

    var newText = existingText + '\n' + label + ' ' + text;



    $("#result").height(oldheight + rowHeight);
    $("#result").val(newText);
}

function arraytocsvstring(array) {

    var idx = 0;
    var retStr = '';

    while (idx < array.length) {
        retStr += array[idx];

        if(idx !== (array.length-1)) {
            retStr += ',';
        }
    
        idx++;
    }

    return retStr;
}

function reset() {
   
    $("#result").height(50);
    $("#result").val('');
    $("#prettyResult").html('');
    $("#prettyResult").height(50);
}


function success(xhr, textStatus) {
    hljs.initHighlightingOnLoad();



    // set prettytext
    //var data = JSON.parse(xhr);
    var stringify = JSON.stringify(xhr, undefined, 2);
    var prettify = hljs.highlightAuto(stringify).value;
    prettify = hljs.fixMarkup(prettify);

    writeLine('STATUS: ', textStatus);
    //	writeLine('RESPONSE: ', prettify);
    $("#prettyResult").html(prettify);
}

function error(jqXHR, exception) {
    writeLine('STATUS: ', exception);
    writeLine('jqXHR', parseError(jqXHR, exception));
}

function getUrl() {
    //return "http://www.gendb.net/';
    return "http://localhost:63154/";
}

function getByPost() {

    var username = 'hello';
    var password = 'george';

    var p = {
        loginInfo: { username: username, password: password }
    };

    $.ajax({
        url: getUrl() + "test/",
        dataType: "json",

        type: "POST",
        data: { username: username, password: password },



        complete: function (xhr, textStatus) {
            //     // set status
            $("#status").html(textStatus);

            //    // set plaintext
            $("#result").val(xhr.responseText);

            // set prettytext
            var data = JSON.parse(xhr.responseText);
            var stringify = JSON.stringify(data, undefined, 2);
            var prettify = hljs.highlightAuto(stringify).value;
            prettify = hljs.fixMarkup(prettify);
            $("#prettyResult").html(prettify);
        }
    });
};



function parseError(jqXHR, exception) {
    var msg = '';
    if (jqXHR.status === 0) {
        msg = 'Not connect.\n Verify Network.';
    } else if (jqXHR.status == 404) {
        msg = 'Requested page not found. [404]';
    } else if (jqXHR.status == 500) {
        msg = 'Internal Server Error [500].';
    } else if (exception === 'parsererror') {
        msg = 'Requested JSON parse failed.';
    } else if (exception === 'timeout') {
        msg = 'Time out error.';
    } else if (exception === 'abort') {
        msg = 'Ajax request aborted.';
    } else {
        msg = 'Uncaught Error.\n' + jqXHR.responseText;
    }

    return msg;
}

function resetStyling() {
    $("#result").val('');
    $("#result").height(20);
}

