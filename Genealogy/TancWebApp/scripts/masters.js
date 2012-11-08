
//var backgrounds = ['photo4', 'waterfall', 'gard_tree', 'stone_tree', 'back_trees', 'back_stones', 'back_hole_stone', 'back_trees_bw'];
//var imgIdx = 0;

//var facebookReady = null;

//$(document).ready(function () {

//    //$('.mainbackground').style.background = url('../Images/damask.jpg');


//    var jsMaster = new JSMaster();

//    jsMaster.setBackground();

//  //  setBackground();

//    $(window).resize($.debounce(250, this.jsMaster.setBackground));
//});


//window.fbAsyncInit = function () {

//    FB.init({ appId: 205401136237103, status: true, cookie: true, xfbml: true });

//    FB.getLoginStatus(function (response) {
//        if (response.status == 'connected') {
//            // showError('connected');
//            getLoggedInUserName();

//            if (facebookReady != null) {
//                facebookReady.apply();
//            }
//        }
//        else {
//            facebookReady.apply();
//        }
//    });
//};


//function makeImageData(name, width, height) {

//    var imagedata = {};

//    imagedata.name = name;
//    imagedata.width = width;
//    imagedata.height = height;

//    return imagedata;

//}



//function setBackground() {

////'photo4', 'waterfall', 'gard_tree', 'stone_tree', 'back_trees', 'back_stones', 'back_hole_stone', 'back_trees_bw'

//    var imgIdx = getCookie('gnt_back');


//    if (imgIdx == undefined)
//        imgIdx = 0;


//    var imgArray = new Array();

//    imgArray.push(makeImageData('photo4', 2592, 1936));
//    imgArray.push(makeImageData('waterfall', 1424, 951));
//    imgArray.push(makeImageData('gard_tree', 800, 600));
//    imgArray.push(makeImageData('stone_tree', 640, 800));
//    imgArray.push(makeImageData('back_trees', 640, 800));
//    imgArray.push(makeImageData('back_stones', 640, 800));
//    imgArray.push(makeImageData('back_hole_stone', 640, 800));
//    imgArray.push(makeImageData('back_trees_bw', 640, 800));
// 

//    $('.mainbackground').css("background", "url(../Images/backgrounds/" + backgrounds[imgIdx] + ".jpg) no-repeat "); //center top
//    $('.mainbackground').css("position", "absolute");
//    $('.mainbackground').css("height", "1290px");
//    $('.mainbackground').css("z-index", "-1");
//    $('.mainbackground').css("background-size", "100% 100%");
//    $('.mainbackground').css("background-attachment", "fixed");

//    //stops annoying white bars appearing when you scroll around.
//    if (window.innerWidth > 1200) {
//        $('.mainbackground').css("width", "100%");
//    }
//    else { 
//        $('.mainbackground').css("width", "1250px");
//    }
//     


//    if (imgIdx == 7) {

//        $('.middle').css("background-color", "");

//        $('.middle').css("opacity", "");
//        $('.middle').css("filter", "");


//        $('.middle').css("border", "thin solid #000000");

//        $('.middle').css("background-image", "url('../Images/main_back.gif')");
//    }
//    else {


//        $('.middle').css("background-color", "#FFFFFF");

//        $('.middle').css("opacity", "0.9");
//        $('.middle').css("filter", "alpha(opacity=90)");

//        $('.middle').css("border", "");

//        $('.middle').css("background-image", "");
//    }
//}


//function nextBackground() {


//    var backIndex = getCookie('gnt_back');

//    if (backIndex == undefined) {
//        imgIdx = 1;
//    }
//    else {
//        imgIdx = backIndex;

//        if (imgIdx < backgrounds.length-1)
//            imgIdx++;
//        else
//            imgIdx = 0;

//    }


//    setCookie("gnt_back", imgIdx, 365);

//    setBackground(imgIdx);
//}

//function prevBackground() {
//    var backIndex = getCookie('gnt_back');

//    if (backIndex == undefined) {
//        imgIdx = backgrounds.length -1;               
//    }
//    else {
//        imgIdx = backIndex;

//        if (imgIdx == 0) {
//            imgIdx = backgrounds.length -1;
//        }
//        else {
//            imgIdx--;
//        }
//    }

//    setCookie("gnt_back", imgIdx, 365);

//    setBackground(imgIdx);
//}



//function setCookie(c_name, value, exdays) {
//    var exdate = new Date();
//    exdate.setDate(exdate.getDate() + exdays);
//    var c_value = escape(value) + ((exdays == null) ? ";path=/;" : "; path=/; expires=" + exdate.toUTCString());

//    document.cookie = c_name + "=" + c_value;
//    
//    
//    //+ getHost() +'/';

//}



//function getCookie(c_name) {
//    var i, x, y, ARRcookies = document.cookie.split(";");
//    for (i = 0; i < ARRcookies.length; i++) {
//        x = ARRcookies[i].substr(0, ARRcookies[i].indexOf("="));
//        y = ARRcookies[i].substr(ARRcookies[i].indexOf("=") + 1);
//        x = x.replace(/^\s+|\s+$/g, "");
//        if (x == c_name) {
//            return unescape(y);
//        }
//    }
//}



//function createLimitedHeader(readyfunction) {

//    facebookReady = readyfunction;
//}


//function createHeader(selectorid, readyfunction) {

//    facebookReady = readyfunction;

//var headersection = '';



////if (userName == '') {
////   userName = '<a href=\'../Forms/Login.aspx\'><span>Log in</span></a>'
////}
////else { 
////    
////}

////headersection += '<div id = "1" class = "midtop">';      
////headersection += '<div id="fb-root">';  
//headersection += '<div class = "mtropt">'; 
//  
//headersection += '<div class = "mtrlnk">';   
//headersection += '<a href=\'\' onclick="masterShowTab(\'1\');return false" ><span>Main Options</span></a>';   
//headersection += '<a href=\'\' onclick="masterShowTab(\'2\');return false" ><span>Along With</span></a>';   
//headersection += '<a href=\'\' onclick="masterShowTab(\'3\');return false" ><span>Tools</span></a>';
//headersection += '<a href=\'\' onclick="masterShowTab(\'4\');return false" ><span>Settings</span></a>';
//headersection += '<a href=\'../HtmlPages/MapView.html\'><span>Map View</span></a>';  
//headersection += '</div>';    

//headersection += '<div>';
//headersection += '<div id="panelA" class = "displayPanel">';
//headersection += '<div class = "mtrlnk">';
//headersection += '<a href=\'../Default.html\'><span>Home</span></a>';
//headersection += '<a href=\'../HtmlPages/MarriageSearch.html\'><span>Marriages</span></a>';
//headersection += '<a href=\'../HtmlPages/PersonSearch.html\'><span>Persons</span></a>';
//headersection += '<a href=\'../HtmlPages/SourceSearch.html\'><span>Sources</span></a>';
//headersection += '</div>';     
//headersection += '</div>';

//headersection += '<div id="panelB" class = "hidePanel">';
//headersection += '<div class = "mtrlnk">';
//headersection += '<a href=\'../HtmlPages/ParishSearch.html\'><span>Parishs</span></a>';
//headersection += '<a href=\'../HtmlPages/Events.html\'><span>Events</span></a>';
//headersection += '<a href=\'../HtmlPages/batchEntry.html\'><span>Batch Entry</span></a>';
//headersection += '<a href=\'../Forms/FrmFiles.aspx\'><span>Files</span></a>';
//headersection += '<a href=\'../HtmlPages/SourceTypesSearch.html\'><span>Source Types</span></a>';
//headersection += '</div>';               
//headersection += '</div>';
//headersection += '<div id="panelC" class = "hidePanel">';
//headersection += '<div class = "mtrlnk">';


//headersection += '<a href=\'../Forms/FrmImportCSV.aspx?Type=PARISH\'><span>Import Parishs</span></a>';
//headersection += '<a href=\'../Forms/FrmImportCSV.aspx?Type=MAR\'><span>Import Marriages</span></a>';
//headersection += '<a href=\'../Forms/FrmImportCSV.aspx?Type=SOURCE\'><span>Import Sources</span></a>';
//headersection += '<a href=\'../HtmlPages/TreeSearch.html\'><span>View Trees</span></a>';
//headersection += '</div>';
//headersection += '</div>';
//headersection += '<div id="panelD" class = "hidePanel">';
//headersection += '<div class = "mtrlnk">';
//headersection += '<a id="prv_next" href="../Default.aspx" onclick="prevBackground();return false"  ><span>Previous Style</span></a>';
//headersection += '<a id="lnk_next" href="../Default.aspx" onclick="nextBackground();return false"  ><span>Next Style</span></a>';
//headersection += '</div>';
//headersection += '</div>';
//headersection += '</div>';

//headersection += '</div>';

//headersection += '<div id="usrinfo" class = "mtrusr">';


//    headersection += '<div id="fb-root">';
//    headersection += '<fb:login-button autologoutlink="true" &nbsp;perms="email,user_birthday,status_update,publish_stream"></fb:login-button>';
//    headersection += '</div>';

//headersection += '<div id = "usr_nam"></div>';
//headersection += '</div>';

//headersection += '<div class = "mtrlog">Ancestry Notes Database</div>';
//headersection += '<div id="errorDialog" title="Error"></div>';
// 

//headersection += '</div>';  



//headersection += '<br />';

////headersection += '</div>';

//$(selectorid).addClass('midtop');

//$(selectorid).html(headersection);


// 
//}






//getLoggedInUserName = function () {


//    //  xhr.setRequestHeader('custom-header', 'value');


//    var params = {};

//    params[0] = 'hello';

//    twaGetJSON("/TestLogin", params, processData2);



//    return false;
//}

//function processData2(data) {

//    $('#usr_nam').html(data);

//}



//function masterShowTab(panel) {



//    if (panel == 1) {
//        $("#panelA").removeClass("hidePanel").addClass("displayPanel");
//        $("#panelB").removeClass("displayPanel").addClass("hidePanel");
//        $("#panelC").removeClass("displayPanel").addClass("hidePanel");
//        $("#panelD").removeClass("displayPanel").addClass("hidePanel");
//    }

//    if (panel == 2) {


//        $("#panelA").removeClass("displayPanel").addClass("hidePanel");
//        $("#panelB").removeClass("hidePanel").addClass("displayPanel");
//        $("#panelC").removeClass("displayPanel").addClass("hidePanel");
//        $("#panelD").removeClass("displayPanel").addClass("hidePanel");
//    }

//    if (panel == 3) {

//        $("#panelA").removeClass("displayPanel").addClass("hidePanel");
//        $("#panelB").removeClass("displayPanel").addClass("hidePanel");
//        $("#panelC").removeClass("hidePanel").addClass("displayPanel");
//        $("#panelD").removeClass("displayPanel").addClass("hidePanel");
//    }

//    if (panel == 4) {

//        $("#panelA").removeClass("displayPanel").addClass("hidePanel");
//        $("#panelB").removeClass("displayPanel").addClass("hidePanel");
//        $("#panelC").removeClass("displayPanel").addClass("hidePanel");
//        $("#panelD").removeClass("hidePanel").addClass("displayPanel");
//    }

//}







var JSMaster = function () {
    this.facebookReady = null;
    this.backgrounds = ['photo4', 'waterfall', 'gard_tree', 'stone_tree', 'back_trees', 'back_stones', 'back_hole_stone', 'back_trees_bw'];
    this.imgIdx = 0;
    this.setBackground();



    this.ancUtils = new AncUtils();
    window.fbAsyncInit =
    function () {

        var getloggedinuser = this.getLoggedInUserName;

        FB.init({ appId: 205401136237103, status: true, cookie: true, xfbml: true });

        FB.getLoginStatus(function (response) {



            if (response.status == 'connected') {
                // showError('connected');
                //this.getLoggedInUserName();
                getloggedinuser.apply();

                if (this.facebookReady != null) {
                    this.facebookReady.apply();
                }
            }
            else {
                this.facebookReady.apply();
            }
        });
    };

    //this.initFacebook;

    $(window).resize($.debounce(250, this.setBackground));

    this.testvar = 'var';
}

JSMaster.prototype.getLoggedInUserName = function () {

        var params = {};

        params[0] = 'hello';

        this.ancUtils.twaGetJSON("/TestLogin", params, function (data) { $('#usr_nam').html(data); });

        return false;
    }



JSMaster.prototype = {


    initFacebook: function () {

        var getloggedinuser = this.getLoggedInUserName;

        FB.init({ appId: 205401136237103, status: true, cookie: true, xfbml: true });

        FB.getLoginStatus(function (response) {

           

            if (response.status == 'connected') {
                // showError('connected');
                //this.getLoggedInUserName();
                getloggedinuser.apply();

                if (this.facebookReady != null) {
                    this.facebookReady.apply();
                }
            }
            else {
                this.facebookReady.apply();
            }
        });
    },

    generateHeader: function (selectorid, readyfunction) {



        this.facebookReady = readyfunction;

        var headersection = '';

        headersection += '<div class = "mtropt">';

        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_mainoptions"   href=\'\' onclick="masterShowTab(\'1\');return false" ><span>Main Options</span></a>';
        headersection += '<a id="lnk_alongwith"     href=\'\' onclick="masterShowTab(\'2\');return false" ><span>Along With</span></a>';
        headersection += '<a id="lnk_tools"         href=\'\' onclick="masterShowTab(\'3\');return false" ><span>Tools</span></a>';
        headersection += '<a id="lnk_settings"      href=\'\' onclick="masterShowTab(\'4\');return false" ><span>Settings</span></a>';
        headersection += '<a id="lnk_mapview"       href=\'../HtmlPages/MapView.html\'><span>Map View</span></a>';
        headersection += '</div>';

        headersection += '<div>';
        headersection += '<div id="panelA" class = "displayPanel">';
        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_home"      href=\'../Default.html\'><span>Home</span></a>';
        headersection += '<a id="lnk_marriages" href=\'../HtmlPages/MarriageSearch.html\'><span>Marriages</span></a>';
        headersection += '<a id="lnk_persons"   href=\'../HtmlPages/PersonSearch.html\'><span>Persons</span></a>';
        headersection += '<a id="lnk_sources"   href=\'../HtmlPages/SourceSearch.html\'><span>Sources</span></a>';
        headersection += '</div>';
        headersection += '</div>';

        headersection += '<div id="panelB" class = "hidePanel">';
        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_parishs"       href=\'../HtmlPages/ParishSearch.html\'><span>Parishs</span></a>';
        headersection += '<a id="lnk_events"        href=\'../HtmlPages/Events.html\'><span>Events</span></a>';
        headersection += '<a id="lnk_batchevents"   href=\'../HtmlPages/batchEntry.html\'><span>Batch Entry</span></a>';
        headersection += '<a id="lnk_files"         href=\'../Forms/FrmFiles.aspx\'><span>Files</span></a>';
        headersection += '<a id="lnk_sourcetypes"   href=\'../HtmlPages/SourceTypesSearch.html\'><span>Source Types</span></a>';
        headersection += '</div>';
        headersection += '</div>';
        headersection += '<div id="panelC" class = "hidePanel">';
        headersection += '<div class = "mtrlnk">';


        headersection += '<a id="lnk_importparishs" href=\'../Forms/FrmImportCSV.aspx?Type=PARISH\'><span>Import Parishs</span></a>';
        headersection += '<a id="lnk_importmarriages" href=\'../Forms/FrmImportCSV.aspx?Type=MAR\'><span>Import Marriages</span></a>';
        headersection += '<a id="lnk_importsources" href=\'../Forms/FrmImportCSV.aspx?Type=SOURCE\'><span>Import Sources</span></a>';
        headersection += '<a id="lnk_viewtrees" href=\'../HtmlPages/TreeSearch.html\'><span>View Trees</span></a>';
        headersection += '</div>';
        headersection += '</div>';
        headersection += '<div id="panelD" class = "hidePanel">';
        headersection += '<div class = "mtrlnk">';
        headersection += '<a id="lnk_prevback" href="../Default.aspx"><span>Previous Style</span></a>';
        headersection += '<a id="lnk_nextback" href="../Default.aspx"><span>Next Style</span></a>';
        headersection += '</div>';
        headersection += '</div>';
        headersection += '</div>';

        headersection += '</div>';

        headersection += '<div id="usrinfo" class = "mtrusr">';


        headersection += '<div id="fb-root">';
        headersection += '<fb:login-button autologoutlink="true" &nbsp;perms="email,user_birthday,status_update,publish_stream"></fb:login-button>';
        headersection += '</div>';

        headersection += '<div id = "usr_nam"></div>';
        headersection += '</div>';

        headersection += '<div class = "mtrlog">Ancestry Notes Database</div>';
        headersection += '<div id="errorDialog" title="Error"></div>';


        headersection += '</div>';



        headersection += '<br />';

        //headersection += '</div>';

        $(selectorid).addClass('midtop');

        $(selectorid).html(headersection);

        $("#lnk_mainoptions").live("click", function () { masterShowTab("1"); return false; });
        $("#lnk_alongwith").live("click", function () { masterShowTab("2"); return false; });
        $("#lnk_tools").live("click", function () { masterShowTab("3"); return false; });
        $("#lnk_settings").live("click", function () { masterShowTab("4"); return false; });
        //$("#lnk_mapview").live("click", function() { masterShowTab("1"); return false;  });

        //$("#lnk_home").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_marriages").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_persons").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_sources").live("click", function() { masterShowTab("1"); return false;  });

        //$("#lnk_parishs").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_events").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_batchevents").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_files").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_sourcetypes").live("click", function() { masterShowTab("1"); return false;  });

        //$("#lnk_importparishs").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_importmarriages").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_importsources").live("click", function() { masterShowTab("1"); return false;  });
        //$("#lnk_viewtrees").live("click", function() { masterShowTab("1"); return false;  });

        $("#lnk_prevback").live("click", function () { prevBackground(); return false; });
        $("#lnk_nextback").live("click", function () { nextBackground(); return false; });


    },

    masterShowTab: function (panel) {
        if (panel == 1) {
            $("#panelA").removeClass("hidePanel").addClass("displayPanel");
            $("#panelB").removeClass("displayPanel").addClass("hidePanel");
            $("#panelC").removeClass("displayPanel").addClass("hidePanel");
            $("#panelD").removeClass("displayPanel").addClass("hidePanel");
        }

        if (panel == 2) {
            $("#panelA").removeClass("displayPanel").addClass("hidePanel");
            $("#panelB").removeClass("hidePanel").addClass("displayPanel");
            $("#panelC").removeClass("displayPanel").addClass("hidePanel");
            $("#panelD").removeClass("displayPanel").addClass("hidePanel");
        }

        if (panel == 3) {

            $("#panelA").removeClass("displayPanel").addClass("hidePanel");
            $("#panelB").removeClass("displayPanel").addClass("hidePanel");
            $("#panelC").removeClass("hidePanel").addClass("displayPanel");
            $("#panelD").removeClass("displayPanel").addClass("hidePanel");
        }

        if (panel == 4) {

            $("#panelA").removeClass("displayPanel").addClass("hidePanel");
            $("#panelB").removeClass("displayPanel").addClass("hidePanel");
            $("#panelC").removeClass("displayPanel").addClass("hidePanel");
            $("#panelD").removeClass("hidePanel").addClass("displayPanel");
        }
    },

    setCookie: function (c_name, value, exdays) {
        var exdate = new Date();
        exdate.setDate(exdate.getDate() + exdays);
        var c_value = escape(value) + ((exdays == null) ? ";path=/;" : "; path=/; expires=" + exdate.toUTCString());

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

        if (backIndex == undefined) {
            this.imgIdx = 1;
        }
        else {
            this.imgIdx = backIndex;

            if (this.imgIdx < this.backgrounds.length - 1)
                this.imgIdx++;
            else
                this.imgIdx = 0;

        }


        this.setCookie("gnt_back", imgIdx, 365);

        this.setBackground(imgIdx);
    },


    prevBackground: function () {
        var backIndex = this.getCookie('gnt_back');

        if (backIndex == undefined) {
            this.imgIdx = this.backgrounds.length - 1;
        }
        else {
            this.imgIdx = backIndex;

            if (this.imgIdx == 0) {
                this.imgIdx = backgrounds.length - 1;
            }
            else {
                this.imgIdx--;
            }
        }

        this.setCookie("gnt_back", imgIdx, 365);

        this.setBackground(imgIdx);
    },

    makeImageData: function (name, width, height) {
        var imagedata = {};
        imagedata.name = name;
        imagedata.width = width;
        imagedata.height = height;

        return imagedata;

    },

    setBackground: function () {
        var imgIdx = this.getCookie('gnt_back');


        if (this.imgIdx == undefined)
            this.imgIdx = 0;


        var imgArray = new Array();

        imgArray.push(this.makeImageData('photo4', 2592, 1936));
        imgArray.push(this.makeImageData('waterfall', 1424, 951));
        imgArray.push(this.makeImageData('gard_tree', 800, 600));
        imgArray.push(this.makeImageData('stone_tree', 640, 800));
        imgArray.push(this.makeImageData('back_trees', 640, 800));
        imgArray.push(this.makeImageData('back_stones', 640, 800));
        imgArray.push(this.makeImageData('back_hole_stone', 640, 800));
        imgArray.push(this.makeImageData('back_trees_bw', 640, 800));


        $('.mainbackground').css("background", "url(../Images/backgrounds/" + this.backgrounds[imgIdx] + ".jpg) no-repeat "); //center top
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
