var FB;

// *** END OF CUSTOMISABLE SECTION ***
function almostEqual(double1, double2, precision) {
    return (Math.abs(double1 - double2) <= precision);
}



// UTIL CLASS
//


var AncUtils = function () {    
    this.localfb= FB;
};


AncUtils.prototype = {

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

    handleSelection: function (evt, selection, bodytag, id) {


        if (evt != undefined) {
            var arIdx = jQuery.inArray(evt, selection);

            if (arIdx == -1) {
                selection.push(evt);
            }
            else {
                selection.splice(arIdx, 1);
            }
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
      


        return selection;
    },

    addlinks: function (dupeEvents, func, context) {
        for (var i = 0; i < dupeEvents.length; i++) {

            $('body').off("click", "#" + dupeEvents[i].key);


       //     $("#" + dupeEvents[i].key).unbind("click");
            

            //console.log('creating event for : ' + dupeEvents[i].key);

            var somecrap = function (idx, val) {
                //probably not efficient to do this multiple times
                //this can be a future optimization.


                $('body').on("click","#" + dupeEvents[idx].key, $.proxy(function () {
                    var va = val;

                    //console.log('clicked with : ' + va);

                    if (va !== null)
                        func.call(context, va);
                    else
                        func.call(context);

                    return false;
                }, context));

            };

            somecrap(i, dupeEvents[i].value);

        }

    },

    getHost: function () {
        if (window.location.hostname.indexOf("local") == -1)
            return 'http://www.gnthackray.net'
        else
            return 'http://local.gnthackray.net:666';
    },

    // gets json set
    twaGetJSON: function (url, paramsArg, methodArg, fbArg) {
        var aburl = this.getHost() + url;
        $.ajaxSetup({ cache: false });

        $.ajax({
            url: aburl,
            dataType: "jsonp",
            data: paramsArg,
            success: methodArg,
            beforeSend: $.proxy(this.addFBToHeader(), this),
            error: function (request, status, error) {
            //    alert(request.responseText);
            }
        });
    },

    //ANCUTILS
    twaPostJSON: function (postParams) {

        //        var postParams = { url: '',
        //            data: data.Batch,
        //            idparam: data.BatchLength,
        //            refreshmethod: data.Total,
        //            refreshArgs: this.getLink,
        //            Context: this
        //        };

        var localurl = this.getHost() + postParams.url;

        var stringy = JSON.stringify(postParams.data);

        var that = this;

        var successFunc = function (message) {
            // was there a error
            var error = that.getValueFromKey(message, 'Error');

            if (error != '' && error != null) {
                //yes
                that.showError(error);
            }
            else {
                //everything was fine - supposedly.
                if (postParams.idparam != undefined) {
                    var result = that.getValueFromKey(message, 'Id'); // make this Id value less arbitary
                    var qutils = new QryStrUtils();
                    qutils.updateQryPar(postParams.idparam, result);
                }
                if (postParams.refreshmethod != undefined) {

                    if (postParams.refreshArgs != undefined) {
                        if (postParams.refreshArgs.data != undefined)
                            postParams.refreshArgs.data = message;
                    }

                    postParams.refreshmethod.call(postParams.Context, postParams.refreshArgs);
                }
            }
        };

        $.ajax({
            cache: false,
            type: "POST",
            async: false,
            url: localurl,
            data: stringy,
            contentType: "application/json",
            dataType: "json",
            beforeSend: $.proxy(this.addFBToHeader(), this),
            success: successFunc
        });

    },

    getValueFromKey: function (qry, name) {
        var match = RegExp(name + '=([^&]*)')
                    .exec(qry);
        return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
    },

    showError: function (error) {
        if (error != '' && error != null) {
            $('#errorDialog').html(error);
            $("#errorDialog").dialog();
        }
    },

    //beforeSend: function (xhr) { passToProxy(xhr, url); }
    // sets facebook token to request header
    addFBToHeader: function () {
        return function (xhr) {
            var access_token = '';
            if (this.localfb != null) {
                if (this.localfb.getAuthResponse() != null)
                    access_token = this.localfb.getAuthResponse()['accessToken'];
            }
            xhr.setRequestHeader('fb', access_token);
        }
    },

    //ParentElement: $('#pager'),
    //Batch: data.Batch,
    //BatchLength: data.BatchLength,
    //Total: data.Total,
    //Function: this.getLink,
    //Context: this
    createpager: function (pagerparams) {

        var clickEvents = new Array();

        //   dupeEvents.push({ key: '#d' + _idx, value: sourceInfo.XREF });


        var blocksize = 5;

        var remainderPages = pagerparams.Total % pagerparams.BatchLength;
        var totalRequiredPages = (pagerparams.Total - remainderPages) / pagerparams.BatchLength;

        if (remainderPages > 0)
            totalRequiredPages++;

        var pagerBody = '';

        if (totalRequiredPages <= blocksize) {
            var idx0 = 0;

            while (idx0 < totalRequiredPages) {

                pagerBody += "<a id='cp_" + idx0 + "' href='' class = 'pagerlink'>" + String(idx0 + 1) + "</a>";
                clickEvents.push({ key: 'cp_' + idx0, value: idx0 });
                idx0++;
            }
        }
        else {
            var startpage = pagerparams.Batch - (pagerparams.Batch % blocksize);
            var limit = 0;

            if ((startpage + blocksize) > totalRequiredPages) {

                limit = totalRequiredPages;
            }
            else {
                limit = startpage + blocksize;

            }

            //   alert(startpage + ' ' + limit);


            if (startpage >= blocksize) {
                pagerBody += "<a id='cp_0' href='' class = 'pagerlink'>First</a>";
                clickEvents.push({ key: 'cp_0', value: 0 });

                // work out how far back to move the pager when the '..' is clicked.
                // if we are at the end of the record and there is only a few pages available
                // then the .. should be moved back to the start of block of pages boundary 
                // eg 01234 56789 1011121314 the block boundaries would be 0 5 and 10

                var countPagesAvailable = (limit - startpage);

                var linkPage = (startpage - blocksize);

                pagerBody += "<a id='cp_" + linkPage + "' href='' class = 'pagerlink'>..</a>";

                clickEvents.push({ key: 'cp_' + linkPage, value: linkPage });
            }

            var idx = startpage;
            while (idx < limit) {
                if (idx == pagerparams.Batch) {
                    pagerBody += "<a id='cp_" + idx + "' href='' class = 'pagerlink_selected'>" + String(idx + 1) + "</a>";
                    clickEvents.push({ key: 'cp_' + idx, value: idx });
                }
                else {
                    pagerBody += "<a id='cp_" + idx + "' href='' class = 'pagerlink' >" + String(idx + 1) + "</a>";
                    clickEvents.push({ key: 'cp_' + idx, value: idx });
                }
                idx++;
            }


            if (idx < totalRequiredPages) {

                var remainderAvailablePages = totalRequiredPages % blocksize;
                //zero based

                startpage += blocksize;
                startpage++;

                pagerBody += "<a id='cp_" + startpage + "' href='' class = 'pagerlink'>..</a>";
                clickEvents.push({ key: 'cp_' + startpage, value: startpage });

                pagerBody += "<a id='cp_" + (totalRequiredPages - remainderAvailablePages) + "' href='' class = 'pagerlink'>Last</a>";
                clickEvents.push({ key: 'cp_' + (totalRequiredPages - remainderAvailablePages), value: (totalRequiredPages - remainderAvailablePages) });

            }
        }

        // set pager html
        $('#' + pagerparams.ParentElement).html(pagerBody);

        // add click TotalEvents
        this.addlinks(clickEvents, pagerparams.Function, pagerparams.Context);
    }
};






var QryStrUtils = function () { }


QryStrUtils.prototype = {

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


    }



}





