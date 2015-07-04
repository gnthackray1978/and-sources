var FB;



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
            $this = $(this);

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
            beforeSend: $.proxy(this._addFBToHeader(), this),
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
          
                $('#errorDialog').html(error);
                $("#errorDialog").dialog();
                
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
            beforeSend: $.proxy(this._addFBToHeader(), this),
            success: successFunc
        });

    },

    getValueFromKey: function (qry, name) {
        var match = RegExp(name + '=([^&]*)')
                    .exec(qry);
        return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
    },

  

    //beforeSend: function (xhr) { passToProxy(xhr, url); }
    // sets facebook token to request header
    _addFBToHeader: function () {
        return function (xhr) {
            var access_token = '';
            if (this.localfb != null) {
                if (this.localfb.getAuthResponse() != null)
                    access_token = this.localfb.getAuthResponse()['accessToken'];
            }
            xhr.setRequestHeader('fb', access_token);
        }
    }

};




