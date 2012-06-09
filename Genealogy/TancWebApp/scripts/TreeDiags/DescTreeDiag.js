



var _tree = null;
var _moustQueue = new Array();
var _mouseDown = false;
var _isMobile = false;

//var _screenWidth = screen.width;
//var _screenHeight = screen.height;

$(window).bind('hashchange', function() {
  // alert('hash changed');
   run();
});






$(document).ready(function () {

 

    createLimitedHeader(run);
});






function run() {
    var params = {};
    var _sourceId = getParameterByName('sid');
    var _personId = getParameterByName('id');


    //switch css here!!!
    //switch css here!!!
    //switch css here!!!

    //what is the plan?
    //so we have a label
    //and a control that we need to move
    //depending on if we are running on a mobile or not.

    //we need a new set of styling classes
    //but we've got to keep the existing classes intact
    //because they're used to identify stuff. 

    // so we should have mobile set and desktop set 
    // the existing set should be empty.



    var int;
    // this is better =>
    //You can detect iPhone and Android devices by inspecting the navigator.userAgent property within the DOM:
    //************

    var is_touch_device = 'ontouchstart' in document.documentElement;

    var mode = '';

    if (is_touch_device == true) {
        _isMobile = true;
        mode = SwitchMode(true);

        $(mode).bind('touchstart', function (evt) {
            evt.preventDefault();

            var _dir = '';

            if (evt.target.id == "up") _dir = 'UP';
            if (evt.target.id == "dn") _dir = 'DOWN';
            if (evt.target.id == "we") _dir = 'WEST';
            if (evt.target.id == "no") _dir = 'NORTH';
            if (evt.target.id == "es") _dir = 'EAST';
            if (evt.target.id == "so") _dir = 'SOUTH';

            if (_tree != null) {
                int = setInterval(function () { _tree.MoveTree(_dir); }, 100);
            }
        });


        $(mode).bind('touchend', function (evt) {

            evt.preventDefault();
            clearInterval(int);
        });

    }
    else {
        _isMobile = false;
        mode = SwitchMode(false);


        $(mode).mousedown(function (evt) {
            var _dir = '';

            if (evt.target.id == "up") _dir = 'UP';
            if (evt.target.id == "dn") _dir = 'DOWN';
            if (evt.target.id == "we") _dir = 'WEST';
            if (evt.target.id == "no") _dir = 'NORTH';
            if (evt.target.id == "es") _dir = 'EAST';
            if (evt.target.id == "so") _dir = 'SOUTH';

            if (_tree != null) {
                int = setInterval(function () { _tree.MoveTree(_dir); }, 100);
            }

        }).mouseup(function () {
            clearInterval(int);
        });

        gLoop = setTimeout(GameLoop, 1000 / 50);

        $("#myCanvas").mousedown(function (evt) {
            if (_tree != null) {
                _mouseDown = true;
            }
        });


        $("#myCanvas").mouseup(function (evt) {

            if (_tree != null) {
                _mouseDown = false;

                //     _tree.SetZoomStart();

                var _point = new Array(1000000, 1000000);


                _moustQueue[_moustQueue.length] = _point;

            }
        });


        $("#myCanvas").click(function (evt) {
            if (_tree != null) {
                //var _point = new Array(evt.clientX, evt.clientY);

                _tree.PerformClick(evt.clientX, evt.clientY);

                if (_tree.refreshData) {
                    window.location.hash = _tree.qryString;


                }


                var _point = new Array(1000000, 1000000);


                _moustQueue[_moustQueue.length] = _point;
            }
        });


        $("#myCanvas").mousemove(function (evt) {
            if (_tree != null) {
                var _point = new Array(evt.clientX, evt.clientY);

                //  console.log(_point[0] + '-' + _point[1] + '_' + _screenWidth)
                _tree.SetMouse(_point[0], _point[1]);

                if (_mouseDown) {

                    _moustQueue.push(_point);
                }
            }
        });

    }


    if (_isMobile) {
        $("#ml .message").html('<span>Downloading</span>');
    } else {
        $("#ml .message").html('<span>Downloading Descendant Tree</span>');
    }

    if(_sourceId == '' || _sourceId == '00000000-0000-0000-0000-000000000000')
    {
        params[0] = _sourceId;
        params[1] = _personId;
        twaGetJSON('/Trees/GetTreeDiagPerson', params, processData);
    }
    else
    {
        params[0] = _sourceId;
        twaGetJSON('/Trees/GetTreeDiag', params, processData);
    }
}


function processData(data) {


    _tree = new Tree();
    var _zoomLevel = getParameterByName('zoom');

    _tree.SetInitialValues(Number(_zoomLevel), 30.0, 120.0, 70.0, 70.0, 100.0, 20.0, 40.0, 20.0, screen.width, screen.height);
//    var _personId = '913501a6-1216-4764-be8c-ae11fd3a0a8b';
//    var _zoomLevel = 100;
//    var _xpos = 750.0;
//    var _ypos = 100.0;

    _tree.generations = data.Generations;

    _tree.familiesPerGeneration = data.FamiliesPerGeneration;
    _tree.familySpanLines = data.FamilySpanLines;
    _tree.childlessMarriages = data.ChildlessMarriages;
    _tree.sourceId = getParameterByName('sid');

    var _personId = getParameterByName('id');

    var _xpos = getParameterByName('xpos');
    var _ypos = getParameterByName('ypos');


  
    _tree.RelocatePerson(_personId,_xpos,_ypos);


    if (_tree.generations.length > 0) {
        if (_tree.generations[0].length > 0) {
            if (_isMobile) {
                $("#ml .message").html('<span>' + _tree.generations[0][0].Name + '</span>');
            } else {
                $("#ml .message").html('<span>Descendants of ' + _tree.generations[0][0].Name + '</span>');
            }
        }
    }


    _tree.refreshData = false;
}


function GameLoop() {

    while (_moustQueue.length > 0) {
        var _point = _moustQueue.shift();


        _tree.SetCentrePoint(_point[0], _point[1]);
        _tree.DrawTree();
    }

    setTimeout(GameLoop, 1000 / 50);
}


function SwitchMode(ismobile) {

    //false = remove

    var removeoradd_desktop = false;
    var removeoradd_mobile = false;
    var retVal = '';

    if (ismobile) {
        retVal = '.mobile_button_box';
        removeoradd_desktop = false;
        removeoradd_mobile = true;
    } else {
        retVal = '.desktop_button_box';
        removeoradd_desktop = true;
        removeoradd_mobile = false;
    }



    $("#ml").toggleClass('desktop_map_label', removeoradd_desktop);
    $("#mc").toggleClass('desktop_map_control', removeoradd_desktop);
    $("#up").toggleClass('desktop_u', removeoradd_desktop);
    $("#up").toggleClass('desktop_button_box', removeoradd_desktop);
    $("#dn").toggleClass('desktop_d', removeoradd_desktop);
    $("#dn").toggleClass('desktop_button_box', removeoradd_desktop);
    $("#we").toggleClass('desktop_w', removeoradd_desktop);
    $("#we").toggleClass('desktop_button_box', removeoradd_desktop);
    $("#no").toggleClass('desktop_n', removeoradd_desktop);
    $("#no").toggleClass('desktop_button_box', removeoradd_desktop);
    $("#es").toggleClass('desktop_e', removeoradd_desktop);
    $("#es").toggleClass('desktop_button_box', removeoradd_desktop);
    $("#so").toggleClass('desktop_s', removeoradd_desktop);
    $("#so").toggleClass('desktop_button_box', removeoradd_desktop);


    $("#ml").toggleClass('mobile_map_label', removeoradd_mobile);
    $("#mc").toggleClass('mobile_map_control', removeoradd_mobile);
    $("#up").toggleClass('mobile_u', removeoradd_mobile);
    $("#up").toggleClass('mobile_button_box', removeoradd_mobile);
    $("#dn").toggleClass('mobile_d', removeoradd_mobile);
    $("#dn").toggleClass('mobile_button_box', removeoradd_mobile);
    $("#we").toggleClass('mobile_w', removeoradd_mobile);
    $("#we").toggleClass('mobile_button_box', removeoradd_mobile);
    $("#no").toggleClass('mobile_n', removeoradd_mobile);
    $("#no").toggleClass('mobile_button_box', removeoradd_mobile);
    $("#es").toggleClass('mobile_e', removeoradd_mobile);
    $("#es").toggleClass('mobile_button_box', removeoradd_mobile);
    $("#so").toggleClass('mobile_s', removeoradd_mobile);
    $("#so").toggleClass('mobile_button_box', removeoradd_mobile);

    //$(".desktop_map_label .message").html('<span>Downloading Descendant Tree</span>');


    return retVal;
}
