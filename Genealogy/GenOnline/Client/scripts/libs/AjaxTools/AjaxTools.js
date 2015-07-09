var FB;

var AncUtils = function () {
    this.localfb = FB;
};

AncUtils.prototype = {

    getHost: function () {
        if (window.location.hostname.indexOf("local") == -1)
            return 'http://www.gendb.net';
        else
            return 'http://local.gendb.net:666';
    },

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

    _addFBToHeader: function () {
        //beforeSend: function (xhr) { passToProxy(xhr, url); }
        // sets facebook token to request header
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




