/*
* jQuery throttle / debounce - v1.1 - 3/7/2010
* http://benalman.com/projects/jquery-throttle-debounce-plugin/
* 
* Copyright (c) 2010 "Cowboy" Ben Alman
* Dual licensed under the MIT and GPL licenses.
* http://benalman.com/about/license/
*/
(function (b, c) { var $ = b.jQuery || b.Cowboy || (b.Cowboy = {}), a; $.throttle = a = function (e, f, j, i) { var h, d = 0; if (typeof f !== "boolean") { i = j; j = f; f = c } function g() { var o = this, m = +new Date() - d, n = arguments; function l() { d = +new Date(); j.apply(o, n) } function k() { h = c } if (i && !h) { l() } h && clearTimeout(h); if (i === c && m > e) { l() } else { if (f !== true) { h = setTimeout(i ? k : l, i === c ? e - m : e) } } } if ($.guid) { g.guid = j.guid = j.guid || $.guid++ } return g }; $.debounce = function (d, e, f) { return f === c ? a(d, e, false) : a(d, f, e !== false) } })(this);






var style_cookie_name = "style";
var style_cookie_duration = 30;





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


function setHeader(xhr) {



    var access_token = '';

    if(FB != undefined)
        access_token = FB.getAuthResponse()['accessToken'];


    xhr.setRequestHeader('fb', access_token);
    //xhr.setRequestHeader('passkey', 'Bar');
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
    
    $('#errorDialog').html(error);

    $("#errorDialog").dialog();
}

function encodeToHex(str) {
    var r = "";
    var e = str.length;
    var c = 0;
    var h;
    while (c < e) {
        h = str.charCodeAt(c++).toString(16);
        while (h.length < 3) h = "0" + h;
        r += h;
    }
    return r;
}
function decodeFromHex(str) {
    var r = "";
    var e = str.length;
    var s;
    while (e >= 0) {
        s = e - 3;
        r = String.fromCharCode("0x" + str.substring(s, e)) + r;
        e = s;
    }
    return r;
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
        var oldVal = getParameterByName(parname);

        if (!oldVal) oldVal = '';

        var pageQry = parname + '=' + oldVal;
        var replaceQry = parname + '=' + parval;

        qry = qry.replace(pageQry, replaceQry);

       //  window.location.href = window.location.href.replace(pageQry, replaceQry);
        window.location.replace(qry);

    }


}



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

function getParameterByName(name) {

    var match = RegExp('[?&]' + name + '=([^&]*)')
                    .exec(window.location.href);

    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));

}

//function getParameterByName(name) {

//    var match = RegExp('[?&]' + name + '=([^&]*)')
//                    .exec(window.location.hash);

//    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));

//}

function switch_style(css_title) {

   
    set_cookie(style_cookie_name, css_title, style_cookie_duration);
    //window.location.reload()
}

function set_style_from_cookie() {

 
    var css_title = get_cookie(style_cookie_name);
    if (css_title.length) {
       switch_style(css_title);
    }
}

function set_cookie(cookie_name, cookie_value,
    lifespan_in_days, valid_domain) {
    // http://www.thesitewizard.com/javascripts/cookies.shtml
    var domain_string = valid_domain ?
                       ("; domain=" + valid_domain) : '';
    document.cookie = cookie_name +
                       "=" + encodeURIComponent(cookie_value) +
                       "; max-age=" + 60 * 60 *
                       24 * lifespan_in_days +
                       "; path=/" + domain_string;
}
function get_cookie(cookie_name) {
    // http://www.thesitewizard.com/javascripts/cookies.shtml
    var cookie_string = document.cookie;
    if (cookie_string.length != 0) {
        var cookie_value = cookie_string.match(
                        '(^|;)[\s]*' +
                        cookie_name +
                        '=([^;]*)');
        return decodeURIComponent(cookie_value[2]);
    }
    return '';
}
