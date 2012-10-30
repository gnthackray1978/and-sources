/*
* jQuery throttle / debounce - v1.1 - 3/7/2010
* http://benalman.com/projects/jquery-throttle-debounce-plugin/
* 
* Copyright (c) 2010 "Cowboy" Ben Alman
* Dual licensed under the MIT and GPL licenses.
* http://benalman.com/about/license/
*/
(function (b, c) { var $ = b.jQuery || b.Cowboy || (b.Cowboy = {}), a; $.throttle = a = function (e, f, j, i) { var h, d = 0; if (typeof f !== "boolean") { i = j; j = f; f = c } function g() { var o = this, m = +new Date() - d, n = arguments; function l() { d = +new Date(); j.apply(o, n) } function k() { h = c } if (i && !h) { l() } h && clearTimeout(h); if (i === c && m > e) { l() } else { if (f !== true) { h = setTimeout(i ? k : l, i === c ? e - m : e) } } } if ($.guid) { g.guid = j.guid = j.guid || $.guid++ } return g }; $.debounce = function (d, e, f) { return f === c ? a(d, e, false) : a(d, f, e !== false) } })(this);




function twaPostJSON(url, data, redirectUrl, idparam,  successFunc) {

    var localurl = getHost() + url;

    var stringy = JSON.stringify(data);

    if(successFunc == undefined)
    {
        successFunc = function (error) {
            if (redirectUrl != undefined && redirectUrl != '') {
                handleReturnCodeWithReturn(error, redirectUrl, idparam);
            }
            else {
                handleReturnCode(error, idparam);
            }
        };
    }

    $.ajax({
        cache: false,
        type: "POST",
        async: false,
        url: localurl,
        data: stringy,
        contentType: "application/json",
        dataType: "json",
        beforeSend: setHeader,
        success:successFunc      
    });

}

//ANCUTILS
function twaGetJSON(url, paramsArg, methodArg) {

    var aburl = getHost() + url;

    $.ajaxSetup({ cache: false });
  
    $.ajax({
        url: aburl,
        dataType: "json",

        data: paramsArg,
        success: methodArg,

        beforeSend: setHeader
    });


}

//ANCUTILS
function setHeader(xhr) {



    var access_token = '';

    if (FB != undefined) {
        if(FB.getAuthResponse() != null)
            access_token = FB.getAuthResponse()['accessToken'];
    }


    xhr.setRequestHeader('fb', access_token);

}




function refreshWithErrorHandler(refreshMethod, message) {

    var error = getValueFromKey(message, 'error');

    if (error != '' && error != null) {
        showError(error);
    }
    else {
        refreshMethod.apply(this, ['1']);
    //    getParishs('1');
    }

}



function handleReturnCodeWithReturn(message, redirectUrl, idParam) {

    var result = getValueFromKey(message, 'Id');

    updateQryPar(idParam, result);

    var error = getValueFromKey(message, 'error');

    if (error != '' && error != null) {
        showError(error);
    }
    else {
        var _hash = window.location.hash;
        window.location = redirectUrl + _hash;
    }
}


function handleReturnCode(message, idParam) {

    var result = getValueFromKey(message, 'Id');

    updateQryPar(idParam, result);

    var error = getValueFromKey(message, 'error');

    if (error != '' && error != null) {
        showError(error);
    }
     
}


function showError(error) {

    if (error != '' && error != null) {
        $('#errorDialog').html(error);
        $("#errorDialog").dialog();       
    }


}





function getHost ()
{
 //   return 'http://www.gnthackray.net';
    // return 'http://localhost:666';


    if (window.location.hostname.indexOf("local") == -1)
        return 'http://www.gnthackray.net'
    else
        return 'http://local.gnthackray.net:666';
}





// *** END OF CUSTOMISABLE SECTION ***

function almostEqual(double1, double2, precision) {
    return (Math.abs(double1 - double2) <= precision);
}



function sort_inner(sort_col, param_name) {

    var col_name = 'sort_col';

    if (param_name != undefined && param_name != '')
        col_name = param_name;

    var existing_col = getParameterByName(col_name);

    if (existing_col) {

        if (existing_col.indexOf(sort_col) >= 0) {
            if (existing_col.indexOf('DESC') < 0) {
                sort_col += ' DESC';
            }

        }

    }

    updateQryPar(col_name, sort_col);   
}


function convertToCSV(array) {
    var csvStr = '';

    $.each(array, function (intIdx, objVal) {
        if (intIdx == 0)
            csvStr += objVal;
        else
            csvStr += ',' + objVal;
    })

    return csvStr;
}

function makeIdQryString(paramName,path) {

    var _loc = window.location.hash;


    // this will return an empty string even if 
    // the key was missing
    // i want something that returns null if the key is missing.

    //getParameterByName(paramName);



    //    var match = RegExp('[?&]' + name + '=([^&]*)')
    //                    .exec(window.location.hash);

    //    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));



    var idParam = getParameterByName(paramName);

    if (idParam == null) {
        if (_loc == '') {
            _loc += paramName + '=' + path;
        }
        else {
            _loc += '&' + paramName + '=' + path;
        }
    }
    else {
        idParam = paramName +'=' + idParam;
        _loc = _loc.replace(idParam, paramName + '=' + path);
    }


    if (_loc.indexOf('?') < 0) {
        _loc = '?' + _loc;
    }



    return _loc;
}


function updateQryPar(parname, parval) {

    //////////////////////
    // give this a try
    ///////////////////
    //location.replace(
    //http://stackoverflow.com/questions/2305069/can-you-use-hash-navigation-without-affecting-history

    var qry = window.location.hash;

    if (qry.indexOf(parname) < 0) {
        if (qry.indexOf('?') < 0) {

            qry = '?' + parname + '=' + parval;
            window.location.hash = qry;
        }
        else {
            qry += '&' + parname + '=' + parval;
            window.location.replace(qry);
        }

       

    }
    else {
        var oldVal = getParameterByName(parname, '');



        var pageQry = parname + '=' + oldVal;
        var replaceQry = parname + '=' + parval;

        qry = qry.replace(pageQry, replaceQry);

       //  window.location.href = window.location.href.replace(pageQry, replaceQry);
        window.location.replace(qry);

    }


}


//ANCUTILS
function updateStrForQry(qry, parname, parval) {

    //parameter not found in string
    if (qry.indexOf('?'+parname) < 0 && qry.indexOf('&' + parname) < 0) {
        

        if (qry.indexOf('?') < 0) {
            // the query string is completely empty
            qry = '?' + parname + '=' + parval;

        }
        else {
            // so tack it on the end
            qry += '&' + parname + '=' + parval;

        }

    }
    else {
        var oldVal = getParameterByNameFromString(qry, parname);

        if (!oldVal) oldVal = '';

        var pageQry = parname + '=' + oldVal;
        var replaceQry = parname + '=' + parval;
        qry = qry.replace(pageQry, replaceQry);
    }


    return qry;
}

 
function getValueFromKey(qry, name) {

    var match = RegExp(name + '=([^&]*)')
                    .exec(qry);

    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));

}


// for 'disconnected' editting of hash
// better to make lots of changes to the hash 
//in memory then update the hash in one go.
function getParameterByNameFromString(qry, name) {

    var match = RegExp('[?&]' + name + '=([^&]*)')
                    .exec(qry);

    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));

}

//ANCUTILS
function getParameterByName(name, defvalue) {

    var match = RegExp('[?&]' + name + '=([^&]*)')
                    .exec(window.location.href);

    if (defvalue != undefined && defvalue != null) {
        if (match != null)
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        else
            return defvalue;
    } else {
        return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
    }
      
}



// UTIL CLASS
//


var AncUtils = function () { }

//update parameters in a string NOT the address bar
AncUtils.updateStrForQry = function (qry, parname, parval) {
    //parameter not found in string
    if (qry.indexOf('?' + parname) < 0 && qry.indexOf('&' + parname) < 0) {
        if (qry.indexOf('?') < 0) {
            // the query string is completely empty
            qry = '?' + parname + '=' + parval;
        }
        else {
            // so tack it on the end
            qry += '&' + parname + '=' + parval;
        }
    }
    else {
        var oldVal = getParameterByNameFromString(qry, parname);

        if (!oldVal) oldVal = '';

        var pageQry = parname + '=' + oldVal;
        var replaceQry = parname + '=' + parval;
        qry = qry.replace(pageQry, replaceQry);
    }
    return qry;
};

//get parameter specify defvalue if you want a default value if it doesnt exist
AncUtils.getParameterByName = function(name, defvalue) { 
    var match = RegExp('[?&]' + name + '=([^&]*)')
                    .exec(window.location.href);

    if (defvalue != undefined && defvalue != null) {
        if (match != null)
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        else
            return defvalue;
    } else {
        return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
    }

}

// gets json set
AncUtils.twaGetJSON = function (url, paramsArg, methodArg, fbArg) {

    var aburl = getHost() + url;

    $.ajaxSetup({ cache: false });

    $.ajax({
        url: aburl,
        dataType: "json",

        data: paramsArg,
        success: methodArg,

        //call back function needs to have specific sig.
        
        beforeSend: AncUtils.addFBToHeader
    });
}




//beforeSend: function (xhr) { passToProxy(xhr, url); }
// sets facebook token to request header
AncUtils.addFBToHeader = function (xhr, fb) {
    return function (xhr) {
        var access_token = '';
        if (FB != undefined) {
            if (FB.getAuthResponse() != null)
                access_token = FB.getAuthResponse()['accessToken'];
        }
        xhr.setRequestHeader('fb', access_token);
    }
}





