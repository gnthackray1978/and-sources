





var JSMaster = function () {

    //this.facebookReady = null;
    console.log('setting initFacebook');
    this.urlroot = '..';
    this.imageTools = new ImageTools();
    this.ancUtils = new AncUtils();
    this.qryStrUtils = new QryStrUtils();
    this.panels = new Panels();

    window.fbAsyncInit = this.initFacebook;

    //Window.prototype.facebookReady = readyfunction;

    this.imageTools.setBackground();

    console.log('finished creating JSMaster');
};


JSMaster.prototype = {


    initFacebook: function () {

        console.log('jsmaster FB.init');

        FB.init({ appId: 205401136237103, status: true, cookie: true, xfbml: true, channelUrl: this.urlroot + '/HtmlPages/channel.html' });

        console.log('jsmaster FB.getLoginStatus');
        
        FB.getLoginStatus(function (response) {


            console.log('jsmaster init getLoginStatus');

            if (response.status == 'connected') {
                // showError('connected');
                //window.getLoggedInUserName();
                console.log('jsmaster init connected');
                
                var params = {};
                params[0] = 'hello';
                var ancUtils = new AncUtils();
                ancUtils.twaGetJSON("/TestLogin", params, function (data) {
                    $('#usr_nam').html(data);
                });

                console.log('jsmaster init facebookReady.apply');
                if (window.facebookReady != null) {
                    console.log('jsmaster init facebookReady.apply confirmed to go');
                    window.facebookReady.apply();
                }
            }
            else {
                console.log('jsmaster init facebookReady.apply');
                window.facebookReady.apply();
            }
            

        }, true);

    },

    generateHeader: function (selectorid, readyfunction) {

        //todo rewrite this to use .proxy - i think it shouldnt be necessary to use the window.prototype for this!

        console.log('jsmaster generateHeader');

        var that = this;

        // this.facebookReady = readyfunction;

        var headersection = '';

        headersection += '<div class = "mtropt">';

        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_mainoptions"   href=""  ><span>Main Options</span></a>';
        headersection += '<a id="lnk_alongwith"     href=""  ><span>Along With</span></a>';
        headersection += '<a id="lnk_tools"         href=""  ><span>Tools</span></a>';
        headersection += '<a id="lnk_settings"      href=""  ><span>Settings</span></a>';
        headersection += '<a id="lnk_mapview"       href=\'../HtmlPages/MapView.html\'><span>Map View</span></a>';
        headersection += '</div>';

        headersection += '<div>';
        headersection += '<div id="panelA" class = "displayPanel">';
        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_home"      href=\'' + that.urlroot + '/HtmlPages/Default.html\'><span>Home</span></a>';
        headersection += '<a id="lnk_marriages" href=\'' + that.urlroot + '/HtmlPages/MarriageSearch.html\'><span>Marriages</span></a>';
        headersection += '<a id="lnk_persons"   href=\'' + that.urlroot + '/HtmlPages/PersonSearch.html\'><span>Persons</span></a>';
        headersection += '<a id="lnk_sources"   href=\'' + that.urlroot + '/HtmlPages/SourceSearch.html\'><span>Sources</span></a>';
        headersection += '</div>';
        headersection += '</div>';

        headersection += '<div id="panelB" class = "hidePanel">';
        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_parishs"       href=\'' + that.urlroot + '/HtmlPages/ParishSearch.html\'><span>Parishs</span></a>';
        headersection += '<a id="lnk_TotalEvents"        href=\'' + that.urlroot + '/HtmlPages/Events.html\'><span>Events</span></a>';
        headersection += '<a id="lnk_batchTotalEvents"   href=\'' + that.urlroot + '/HtmlPages/batchEntry.html\'><span>Batch Entry</span></a>';
        headersection += '<a id="lnk_files"         href=\'' + that.urlroot + '/Forms/FrmFiles.aspx\'><span>Files</span></a>';
        headersection += '<a id="lnk_sourcetypes"   href=\'' + that.urlroot + '/HtmlPages/SourceTypesSearch.html\'><span>Source Types</span></a>';
        headersection += '</div>';
        headersection += '</div>';
        headersection += '<div id="panelC" class = "hidePanel">';
        headersection += '<div class = "mtrlnk">';


        headersection += '<a id="lnk_importparishs" href=\'' + that.urlroot + '/Forms/FrmImportCSV.aspx?Type=PARISH\'><span>Import Parishs</span></a>';
        headersection += '<a id="lnk_importmarriages" href=\'' + that.urlroot + '/Forms/FrmImportCSV.aspx?Type=MAR\'><span>Import Marriages</span></a>';
        headersection += '<a id="lnk_importsources" href=\'' + that.urlroot + '/Forms/FrmImportCSV.aspx?Type=SOURCE\'><span>Import Sources</span></a>';
        headersection += '<a id="lnk_viewtrees" href=\'' + that.urlroot + '/HtmlPages/TreeSearch.html\'><span>View Trees</span></a>';
        headersection += '</div>';
        headersection += '</div>';
        headersection += '<div id="panelD" class = "hidePanel">';
        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_prevback" href=\'' + that.urlroot + '/Default.aspx\'><span>Previous Style</span></a>';
        headersection += '<a id="lnk_nextback" href=\'' + that.urlroot + '/Default.aspx\'><span>Next Style</span></a>';
        headersection += '</div>';
        headersection += '</div>';
        headersection += '</div>';

        headersection += '</div>';

        headersection += '<div id="usrinfo" class = "mtrusr">';


        headersection += '<div id="fb-root">';
           headersection += '<fb:login-button autologoutlink="true" perms="email,user_birthday,status_update"></fb:login-button>';
     //   headersection += '<fb:login-button show-faces="false" width=200px!important max-rows="1"></fb:login-button>';
        headersection += '</div>';

        headersection += '<div id = "usr_nam"></div>';
        headersection += '</div>';

        headersection += '<div class = "mtrlog">JS Family History</div>';
        headersection += '<div id="errorDialog" title="Error"></div>';

        headersection += '</div>';

        headersection += '<br />';

        $(selectorid).addClass('midtop');

        $(selectorid).html(headersection);




        $('body').on("click", "#lnk_mainoptions", $.proxy(function () { that.panels.masterShowTab(1); return false; }, that.panels));
        $('body').on("click", "#lnk_alongwith", $.proxy(function () { that.panels.masterShowTab(2); return false; }, that.panels));
        $('body').on("click", "#lnk_tools", $.proxy(function () { that.panels.masterShowTab(3); return false; }, that.panels));
        $('body').on("click", "#lnk_settings", $.proxy(function () { that.panels.masterShowTab(4); return false; }, that.panels));

        $('body').on("click", "#lnk_prevback", $.proxy(function () { that.imageTools.prevBackground(); return false; }, that));
        $('body').on("click", "#lnk_nextback", $.proxy(function () { that.imageTools.nextBackground(); return false; }, that));

        this.connectfacebook(function () {
            console.log('facebook loaded');
        });

        readyfunction.call();

       
    },

    connectfacebook: function (readyfunction) {

        if (this.urlroot == '..') {
            Window.prototype.facebookReady = readyfunction;

            (function () {
                var e = document.createElement('script');
                e.src = 'http://connect.facebook.net/en_US/all.js';
                e.async = true;
                document.getElementById('fb-root').appendChild(e);
            } ());
        }
        else {
            readyfunction.call();

        }
    }



}
