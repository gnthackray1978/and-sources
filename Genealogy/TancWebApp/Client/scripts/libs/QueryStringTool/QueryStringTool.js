


var QryStrUtils = function () { }


QryStrUtils.prototype = {

    pad: function (number, length) {

        var str = '' + number;
        while (str.length < length) {
            str = '0' + str;
        }

        return str;

    },

    convertToCSV: function (array) {
        var csvStr = '';

        $.each(array, function (intIdx, objVal) {
            if (intIdx == 0)
                csvStr += objVal;
            else
                csvStr += ',' + objVal;
        })

        return csvStr;
    },




    makeIdQryString: function (paramName, path) {

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
            idParam = paramName + '=' + idParam;
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

            //             if ($.type(args[prop]) == "string")
            //                workingQry = this.updateStrForQry(workingQry, prop, args[prop]);
            //             else
            //                workingQry = this.updateStrForQry(workingQry, prop, args[prop].val());


            switch ($.type(args[prop])) {
                case "string":
                    workingQry = this.updateStrForQry(workingQry, prop, args[prop]);
                    break;
                case "boolean":
                    workingQry = this.updateStrForQry(workingQry, prop, args[prop]);
                    break;
                default:
                    workingQry = this.updateStrForQry(workingQry, prop, args[prop].val());
            }


        }

        window.location.hash = workingQry;
    },

    updateQryPar: function (parname, parval) {
        var qry = window.location.hash;
        // parameters always should be followed by = 
        // checking for this avoids screw ups where for example id is in the middle of another 
        // param name like fids
        if (qry.indexOf(parname + '=') < 0) {
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
    getParameterByName: function (name, defvalue) {
        var match = RegExp('[?&]' + name + '=([^&]*)')
                        .exec(window.location.href);

        if (defvalue != undefined && defvalue != null) {
            if (match != null) {

                var codedUri = match[1].replace(/\+/g, ' ');

                if (codedUri != '%') {
                    return match && decodeURIComponent(codedUri);
                } else {
                    return '%';
                }
            }

            else
                return defvalue;
        } else {
            return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
        }

    },

    getParameterByNameFromString: function (qry, name) {
        var match = RegExp('[?&]' + name + '=([^&]*)')
                        .exec(qry);

        var codedUri = match[1].replace(/\+/g, ' ');


        if (codedUri == '%')
            return '%';
        else
            return match && decodeURIComponent(codedUri);


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
    }


}




