
var _ancTree = null;
var _moustQueue = new Array();
var _mouseDown = false;
//var _screenWidth = screen.width;
//var _screenHeight = screen.height;


$(document).ready(function () {


    createLimitedHeader(run);
    

});




function run() {
    var params = {};

    var int;

    $(".button_box").mousedown(function (evt) {
        var _dir = '';

        if (evt.target.id == "up") _dir = 'UP';
        if (evt.target.id == "dn") _dir = 'DOWN';
        if (evt.target.id == "we") _dir = 'WEST';
        if (evt.target.id == "no") _dir = 'NORTH';
        if (evt.target.id == "es") _dir = 'EAST';
        if (evt.target.id == "so") _dir = 'SOUTH';

        if (_ancTree != null) {
            int = setInterval(function () { _ancTree.MoveTree(_dir); }, 100);
        }

    }).mouseup(function () {
        clearInterval(int);
    });

    gLoop = setTimeout(GameLoop, 1000 / 50);

    $("#myCanvas").click(function (evt) {
        if (_ancTree != null) {
            //var _point = new Array(evt.clientX, evt.clientY);

            _ancTree.PerformClick(evt.clientX, evt.clientY);

            if (_ancTree.refreshData) {
                //window.location.hash = _ancTree.qryString;
                window.location = '../HtmlPages/DescendantsTree.html#' + _ancTree.qryString;

            }


            var _point = new Array(1000000, 1000000);


            _moustQueue[_moustQueue.length] = _point;
        }
    });


    $("#myCanvas").mousedown(function (evt) {
        if (_ancTree != null) {
            _mouseDown = true;
        }
    });



    $("#myCanvas").mouseup(function (evt) {

        if (_ancTree != null) {
            _mouseDown = false;

            //     _tree.SetZoomStart();

            var _point = new Array(1000000, 1000000);


            _moustQueue[_moustQueue.length] = _point;

        }
    });


    $("#myCanvas").mousemove(function (evt) {
        if (_ancTree != null) {
            var _point = new Array(evt.clientX, evt.clientY);

            //  console.log(_point[0] + '-' + _point[1] + '_' + _screenWidth)
            _ancTree.SetMouse(_point[0], _point[1]);

            if (_mouseDown) {

                _moustQueue.push(_point);
            }
        }
    });

    $("#map_label .message").html('<span>Downloading Ancestors</span>');




    params[0] = getParameterByName('id', ''); //'4d800222-65fc-42f0-b568-972cde7ce38f';

    twaGetJSON('/Trees/GetAncTreeDiag', params, processData);
}


function processData(data) {



    var _zoomLevel = getParameterByName('zoom', '');
    _ancTree = new AncTree();
    _ancTree.SetInitialValues(Number(_zoomLevel), 10.0, 125.0, 100.0, 90.0, 100.0, 5.0, 40.0, 20.0, screen.width, screen.height);


    var _personId = getParameterByName('id', '');

    var _xpos = getParameterByName('xpos', '');
    var _ypos = getParameterByName('ypos', '');

    _ancTree.generations = data.Generations;

    _ancTree.familiesPerGeneration = data.FamiliesPerGeneration;
    _ancTree.familySpanLines = data.FamilySpanLines;
    _ancTree.childlessMarriages = data.ChildlessMarriages;


    _ancTree.RelocatePerson(_personId, _xpos, _ypos);

    if (_ancTree.generations.length > 0) {
        if (_ancTree.generations[0].length > 0) {
            $("#map_label .message").html('<span>Ancestors of ' + _ancTree.generations[0][0].Name + '</span>');
        }
    }
}

function GameLoop() {

    while (_moustQueue.length > 0) {
        var _point = _moustQueue.shift();
        _ancTree.SetCentrePoint(_point[0], _point[1]);
        _ancTree.DrawTree();
    }

    setTimeout(GameLoop, 1000 / 50);
}