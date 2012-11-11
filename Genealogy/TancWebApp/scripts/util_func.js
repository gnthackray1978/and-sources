//USAGE EXAMPLES 

// using debounce in a constructor or initialization function to debounce
// focus events for a widget (onFocus is the original handler):
//this.debouncedOnFocus = this.onFocus.debounce(500, false);
//this.inputNode.addEventListener('focus', this.debouncedOnFocus, false);

//// to coordinate the debounce of a method for all objects of a certain class, do this:
//MyClass.prototype.someMethod = function () {
//    /* do something here, but only once */
//} .debounce(100, true); // execute at start and use a 100 msec detection period

//// wait until the user is done moving the mouse, then execute
//// (using the stand-alone version)
//document.onmousemove = debounce(function (e) {
//    /* do something here, but only once after mouse cursor stops */
//}, 250, false);
//http://ajaxian.com/archives/debounce-your-javascript-functions
Function.prototype.debounce = function (threshold, execAsap) {
    var func = this, // reference to original function
            timeout; // handle to setTimeout async task (detection period)
    // return the new debounced function which executes the original function only once
    // until the detection period expires
    return function debounced() {
        var obj = this, // reference to original context object
                args = arguments; // arguments at execution time
        // this is the detection function. it will be executed if/when the threshold expires
        function delayed() {
            // if we're executing at the end of the detection period
            if (!execAsap)
                func.apply(obj, args); // execute now
            // clear timeout handle
            timeout = null;
        };
        // stop any current detection period
        if (timeout)
            clearTimeout(timeout);
        // otherwise, if we're not already waiting and we're executing at the beginning of the waiting period
        else if (execAsap)
            func.apply(obj, args); // execute now
        // reset the waiting period
        timeout = setTimeout(delayed, threshold || 100);
    };
}






//remove invalid selections from an array
Array.prototype.RemoveInvalid = function (selection) {
    var filteredArray = new Array();
    for (var si = 0; si < selection.length; si++) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == selection[si]) {
                filteredArray.push(this[i]);
                break;
            }
        }
    }
    return filteredArray;
}










//ANCUTILS
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

        //beforeSend: setHeader
        beforeSend: proxy(FB)
    });


}

 

function proxy(_fb) {

    return function(xhr) {
        var access_token = '';
        if (_fb != undefined) {
            if (_fb.getAuthResponse() != null)
                access_token = _fb.getAuthResponse()['accessToken'];
        }
        xhr.setRequestHeader('fb', access_token);
    }

}


 
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

//ANCUTILS
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


AncUtils.prototype = {

     convertToCSV: function(array) {
        var csvStr = '';

        $.each(array, function (intIdx, objVal) {
            if (intIdx == 0)
                csvStr += objVal;
            else
                csvStr += ',' + objVal;
        })

        return csvStr;
    },
    
    sort_inner: function (sort_col, param_name) {

        var col_name = 'sort_col';

        if (param_name != undefined && param_name != '')
            col_name = param_name;

        var existing_col = this.getParameterByName(col_name);

        if (existing_col) {

            if (existing_col.indexOf(sort_col) >= 0) {
                if (existing_col.indexOf('DESC') < 0) {
                    sort_col += ' DESC';
                }
            }
        }

        this.updateQryPar(col_name, sort_col);   
    },


    handleSelection: function(evt, selection, bodytag, id)
    {
        var arIdx = jQuery.inArray(evt, selection);

        if (arIdx == -1) {
            selection.push(evt);
        }
        else {
            selection.splice(arIdx, 1);
        }

        $(bodytag).each(function () {
            $this = $(this)

            var quantity = $this.find(id).val();
            arIdx = jQuery.inArray(quantity, selection);

            if (arIdx == -1) {
                $this.removeClass('highLightRow');
            }
            else {
                $this.addClass('highLightRow');
            }
        }); //end each
    },


    addlinks: function (dupeEvents, func, context) {
        for (var i = 0; i < dupeEvents.length; i++) {
            var m = i;
            $("#" + dupeEvents[i].key).live("click",
                $.proxy(function () {
                    var va = m;

                    func.call(context,va);
                    return false;
                }, context));
        }
    
    },

    getHost: function()
    {
        if (window.location.hostname.indexOf("local") == -1)
            return 'http://www.gnthackray.net'
        else
            return 'http://local.gnthackray.net:666';
    },

    // gets json set
    twaGetJSON:function (url, paramsArg, methodArg, fbArg) {

        var aburl = this.getHost() + url;

        $.ajaxSetup({ cache: false });

        $.ajax({
            url: aburl,
            dataType: "json",

            data: paramsArg,
            success: methodArg,

            //call back function needs to have specific sig.
        
            beforeSend: this.addFBToHeader(FB)
        });
    },

    //ANCUTILS
    twaPostJSON:function (url, data, redirectUrl, idparam, successFunc) {

        var localurl = this.getHost() + url;

        var stringy = JSON.stringify(data);

        if (successFunc == undefined) {
            successFunc = function (error) {
                if (redirectUrl != undefined && redirectUrl != '') {
                    this.handleReturnCodeWithReturn(error, redirectUrl, idparam);
                }
                else {
                    this.handleReturnCode(error, idparam);
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
            success: successFunc
        });

    },


    handleReturnCodeWithReturn: function(message, redirectUrl, idParam) {

        var result = this.getValueFromKey(message, 'Id');

        this.updateQryPar(idParam, result);

        var error = this.getValueFromKey(message, 'error');

        if (error != '' && error != null) {
            this.showError(error);
        }
        else {
            var _hash = window.location.hash;
            window.location = redirectUrl + _hash;
        }
    },

    getValueFromKey: function(qry, name) {
        var match = RegExp(name + '=([^&]*)')
                    .exec(qry);
        return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
    },

    
    showError: function(error) {
        if (error != '' && error != null) {
            $('#errorDialog').html(error);
            $("#errorDialog").dialog();       
        }
    },


    //beforeSend: function (xhr) { passToProxy(xhr, url); }
    // sets facebook token to request header
    addFBToHeader :function (fb) {
        return function (xhr) {
            var access_token = '';
            if (FB != undefined) {
                if (FB.getAuthResponse() != null)
                    access_token = FB.getAuthResponse()['accessToken'];
            }
            xhr.setRequestHeader('fb', access_token);
        }
    }
}









var QryStrUtils = function () { }


QryStrUtils.prototype = {

     makeIdQryString: function(paramName,path) {

        var _loc = window.location.hash;

        var idParam = this.getParameterByName(paramName);

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
    },


    updateQry: function (args) {
        // var myJSONObject = { "ircEvent": "PRIVMSG", "method": "newURI", "regex": "^http://.*" };

         var workingQry = window.location.hash;

         for (var prop in args) {

             if ($.type(args[prop]) == "string")
                workingQry = this.updateStrForQry(workingQry, prop, args[prop]);
             else
                workingQry = this.updateStrForQry(workingQry, prop, args[prop].val());

         }

         window.location.replace(workingQry);
     },

     updateQryPar: function (parname, parval) {   
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
            var oldVal = this.getParameterByName(parname, '');
            var pageQry = parname + '=' + oldVal;
            var replaceQry = parname + '=' + parval;
            qry = qry.replace(pageQry, replaceQry);         
            window.location.replace(qry);
        }
    },

    //update parameters in a string NOT the address bar
    updateStrForQry: function (qry, parname, parval) {
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
            var oldVal = this.getParameterByNameFromString(qry, parname);

            if (!oldVal) oldVal = '';

            var pageQry = parname + '=' + oldVal;
            var replaceQry = parname + '=' + parval;
            qry = qry.replace(pageQry, replaceQry);
        }
        return qry;
    },

    //get parameter specify defvalue if you want a default value if it doesnt exist
    getParameterByName: function(name, defvalue) { 
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




}
