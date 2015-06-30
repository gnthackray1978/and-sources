





var JSMaster = function () {

    //this.facebookReady = null;
    console.log('setting initFacebook');



    window.fbAsyncInit = this.initFacebook;

    //Window.prototype.facebookReady = readyfunction;

    this.backgrounds = [ 'stone_tree', 'back_trees', 'back_stones', 'back_hole_stone', 'back_trees_bw'];
    this.imgIdx = 4;
  //  this.urlroot = 'https://c9.io/gnthackray1978/and-sources/workspace/Genealogy/TancWebApp';
    this.urlroot = '..';  
    this.setBackground();
    this.ancUtils = new AncUtils();
    this.qryStrUtils = new QryStrUtils();



    $(window).resize($.proxy(this.setBackground.debounce(250, false), this));

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
        headersection += '<a id="lnk_home"      href=\'' + this.urlroot + '/Default.html\'><span>Home</span></a>';
        headersection += '<a id="lnk_marriages" href=\'' + this.urlroot + '/HtmlPages/MarriageSearch.html\'><span>Marriages</span></a>';
        headersection += '<a id="lnk_persons"   href=\'' + this.urlroot + '/HtmlPages/PersonSearch.html\'><span>Persons</span></a>';
        headersection += '<a id="lnk_sources"   href=\'' + this.urlroot + '/HtmlPages/SourceSearch.html\'><span>Sources</span></a>';
        headersection += '</div>';
        headersection += '</div>';

        headersection += '<div id="panelB" class = "hidePanel">';
        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_parishs"       href=\'' + this.urlroot + '/HtmlPages/ParishSearch.html\'><span>Parishs</span></a>';
        headersection += '<a id="lnk_TotalEvents"        href=\'' + this.urlroot + '/HtmlPages/Events.html\'><span>Events</span></a>';
        headersection += '<a id="lnk_batchTotalEvents"   href=\'' + this.urlroot + '/HtmlPages/batchEntry.html\'><span>Batch Entry</span></a>';
        headersection += '<a id="lnk_files"         href=\'' + this.urlroot + '/Forms/FrmFiles.aspx\'><span>Files</span></a>';
        headersection += '<a id="lnk_sourcetypes"   href=\'' + this.urlroot + '/HtmlPages/SourceTypesSearch.html\'><span>Source Types</span></a>';
        headersection += '</div>';
        headersection += '</div>';
        headersection += '<div id="panelC" class = "hidePanel">';
        headersection += '<div class = "mtrlnk">';


        headersection += '<a id="lnk_importparishs" href=\'' + this.urlroot + '/Forms/FrmImportCSV.aspx?Type=PARISH\'><span>Import Parishs</span></a>';
        headersection += '<a id="lnk_importmarriages" href=\'' + this.urlroot + '/Forms/FrmImportCSV.aspx?Type=MAR\'><span>Import Marriages</span></a>';
        headersection += '<a id="lnk_importsources" href=\'' + this.urlroot + '/Forms/FrmImportCSV.aspx?Type=SOURCE\'><span>Import Sources</span></a>';
        headersection += '<a id="lnk_viewtrees" href=\'' + this.urlroot + '/HtmlPages/TreeSearch.html\'><span>View Trees</span></a>';
        headersection += '</div>';
        headersection += '</div>';
        headersection += '<div id="panelD" class = "hidePanel">';
        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_prevback" href=\'' + this.urlroot + '/Default.aspx\'><span>Previous Style</span></a>';
        headersection += '<a id="lnk_nextback" href=\'' + this.urlroot + '/Default.aspx\'><span>Next Style</span></a>';
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




        $('body').on("click", "#lnk_mainoptions", $.proxy(function () { panels.masterShowTab(1); return false; }, panels));
        $('body').on("click", "#lnk_alongwith", $.proxy(function () { panels.masterShowTab(2); return false; }, panels));
        $('body').on("click", "#lnk_tools", $.proxy(function () { panels.masterShowTab(3); return false; }, panels));
        $('body').on("click", "#lnk_settings", $.proxy(function () { panels.masterShowTab(4); return false; }, panels));

        $('body').on("click", "#lnk_prevback", $.proxy(function () { this.prevBackground(); return false; }, this));
        $('body').on("click", "#lnk_nextback", $.proxy(function () { this.nextBackground(); return false; }, this));

        this.connectfacebook(function () {
            console.log('facebook loaded');
        });

        readyfunction.call();

        var panels = new Panels();
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
    },

//    masterShowTab: function (panel) {
//        if (panel == 1) {
//            $("#panelA").removeClass("hidePanel").addClass("displayPanel");
//            $("#panelB").removeClass("displayPanel").addClass("hidePanel");
//            $("#panelC").removeClass("displayPanel").addClass("hidePanel");
//            $("#panelD").removeClass("displayPanel").addClass("hidePanel");
//        }

//        if (panel == 2) {
//            $("#panelA").removeClass("displayPanel").addClass("hidePanel");
//            $("#panelB").removeClass("hidePanel").addClass("displayPanel");
//            $("#panelC").removeClass("displayPanel").addClass("hidePanel");
//            $("#panelD").removeClass("displayPanel").addClass("hidePanel");
//        }

//        if (panel == 3) {

//            $("#panelA").removeClass("displayPanel").addClass("hidePanel");
//            $("#panelB").removeClass("displayPanel").addClass("hidePanel");
//            $("#panelC").removeClass("hidePanel").addClass("displayPanel");
//            $("#panelD").removeClass("displayPanel").addClass("hidePanel");
//        }

//        if (panel == 4) {

//            $("#panelA").removeClass("displayPanel").addClass("hidePanel");
//            $("#panelB").removeClass("displayPanel").addClass("hidePanel");
//            $("#panelC").removeClass("displayPanel").addClass("hidePanel");
//            $("#panelD").removeClass("hidePanel").addClass("displayPanel");
//        }
//    },

    setCookie: function (c_name, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value = escape(value) + ((exdays === null) ? ";path=/;" : "; path=/; expires=" + exdate.toUTCString());

        document.cookie = c_name + "=" + c_value;
    },

    getCookie: function (c_name) {
        var i, x, y, ARRcookies = document.cookie.split(";");
        for (i = 0; i < ARRcookies.length; i++) {
            x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
            y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
            x = x.replace(/^\s+|\s+$/g, "");
            if (x == c_name) {
                return unescape(y);
            }
        }
    },


    nextBackground: function () {
        var backIndex = this.getCookie('gnt_back');

        if (backIndex === undefined) {
            this.imgIdx = 1;
        }
        else {
            this.imgIdx = backIndex;

            if (this.imgIdx < this.backgrounds.length - 1)
                this.imgIdx++;
            else
                this.imgIdx = 0;

        }


        this.setCookie("gnt_back", this.imgIdx, 365);

        this.setBackground(this.imgIdx);
    },


    prevBackground: function () {
        var backIndex = this.getCookie('gnt_back');

        if (backIndex === undefined) {
            this.imgIdx = this.backgrounds.length - 1;
        }
        else {
            this.imgIdx = backIndex;

            if (this.imgIdx === 0) {
                this.imgIdx = this.backgrounds.length - 1;
            }
            else {
                this.imgIdx--;
            }
        }

        this.setCookie("gnt_back", this.imgIdx, 365);

        this.setBackground(this.imgIdx);
    },

    makeImageData: function (name, width, height) {
        var imagedata = {};
        imagedata.name = name;
        imagedata.width = width;
        imagedata.height = height;

        return imagedata;

    },

    setBackground: function () {

        //alert('set back');
        this.imgIdx = this.getCookie('gnt_back');


        if (this.imgIdx === undefined)
            this.imgIdx = 0;


        var imgArray = new Array();

     //   imgArray.push(this.makeImageData('photo4', 2592, 1936));
     //   imgArray.push(this.makeImageData('waterfall', 1424, 951));
      //  imgArray.push(this.makeImageData('gard_tree', 800, 600));
        imgArray.push(this.makeImageData('stone_tree', 640, 800));
        imgArray.push(this.makeImageData('back_trees', 640, 800));
        imgArray.push(this.makeImageData('back_stones', 640, 800));
        imgArray.push(this.makeImageData('back_hole_stone', 640, 800));
        imgArray.push(this.makeImageData('back_trees_bw', 640, 800));



        $('.mainbackground').css("background", "url(" + this.urlroot + "/Images/backgrounds/" + this.backgrounds[this.imgIdx] + ".jpg) no-repeat "); //center top


        $('.mainbackground').css("position", "absolute");
        $('.mainbackground').css("height", "1290px");
        $('.mainbackground').css("z-index", "-1");
        $('.mainbackground').css("background-size", "100% 100%");
        $('.mainbackground').css("background-attachment", "fixed");

        //stops annoying white bars appearing when you scroll around.
        if (window.innerWidth > 1200) {
            $('.mainbackground').css("width", "100%");
        }
        else {
            $('.mainbackground').css("width", "1250px");
        }

        if (this.imgIdx == 7) {

            $('.middle').css("background-color", "");

            $('.middle').css("opacity", "");
            $('.middle').css("filter", "");


            $('.middle').css("border", "thin solid #000000");

            $('.middle').css("background-image", "url('../Images/main_back.gif')");
        }
        else {


            $('.middle').css("background-color", "#FFFFFF");

            $('.middle').css("opacity", "0.9");
            $('.middle').css("filter", "alpha(opacity=90)");

            $('.middle').css("border", "");

            $('.middle').css("background-image", "");
        }
    }



}
