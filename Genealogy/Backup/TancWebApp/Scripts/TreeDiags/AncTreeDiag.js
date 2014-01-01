var JSMaster, AncUtils,QryStrUtils,AncTree;





//var _screenWidth = screen.width;
//var _screenHeight = screen.height;


$(document).ready(function () {
    var jsMaster = new JSMaster();

    jsMaster.connectfacebook(function () {   
       
        var ancTreeDiag = new AncTreeDiag();
    
        ancTreeDiag.run();
    });    
});

var AncTreeDiag = function () {
    this.ancTree = null;
    this.ancUtils = new AncUtils();
    this.qryStrUtils = new QryStrUtils();
    this._moustQueue = [];
    this._mouseDown = false;
};

AncTreeDiag.prototype = {

    run: function () {
        var params = {};

        var int;
        var that = this;

        $("#myCanvas").css("background-color", "white");
        $("#myCanvas").css("top", "0");
        $("#myCanvas").css("left", "0");
        $("#myCanvas").css("right", "0");
        $("#myCanvas").css("bottom", "0");

        $("#myCanvas").css("width", "100%");
        $("#myCanvas").css("height", "100%");



        $(".button_box").mousedown(function (evt) {
            var _dir = '';

            if (evt.target.id == "up") _dir = 'UP';
            if (evt.target.id == "dn") _dir = 'DOWN';
            if (evt.target.id == "we") _dir = 'WEST';
            if (evt.target.id == "no") _dir = 'NORTH';
            if (evt.target.id == "es") _dir = 'EAST';
            if (evt.target.id == "so") _dir = 'SOUTH';

            if (this.ancTree !== null) {

                int = setInterval(function () { that.ancTree.MoveTree(_dir); }, 100);
            }

        }).mouseup(function () {
            clearInterval(int);
        });



        $("#myCanvas").click(function (evt) {
            if (that.ancTree !== null) {
                that.ancTree.PerformClick(evt.clientX, evt.clientY);
                if (that.ancTree.refreshData) {
                    //window.location = '../HtmlPages/DescendantsTree.html#' + that.ancTree.qryString;    

                    window.location = '../HtmlPages/DescendantsTree.html#' + that.ancTree.qryString;

                    _sourceId = that.qryStrUtils.getParameterByName('sid', '');
                    _personId = that.qryStrUtils.getParameterByName('id', '');

                    params[0] = _sourceId;
                    params[1] = _personId;

                    that.ancUtils.twaGetJSON('/Trees/GetTreeDiagPerson', params, $.proxy(that.processData, that));

                }
                var _point = new Array(1000000, 1000000);
                that._moustQueue[that._moustQueue.length] = _point;
            }
        });

   





        $("#myCanvas").mousedown(function (evt) {
            evt.preventDefault();

            if (that.ancTree !== null) {

                that._mouseDown = true;


            }
        });

        $("#myCanvas").mouseup(function (evt) {
            evt.preventDefault();
            if (that.ancTree !== null) {
                that._mouseDown = false;
                var _point = new Array(1000000, 1000000);
                that._moustQueue[that._moustQueue.length] = _point;
            }

        });

        $("#myCanvas").mousemove(function (evt) {
            if (that.ancTree !== null) {
                var _point = new Array(evt.clientX, evt.clientY);

                // pass into here is mouse down
                that.ancTree.SetMouse(_point[0], _point[1], that._mouseDown);
                if (that._mouseDown) {
                    that._moustQueue.push(_point);
                }
            }
        });

        $("#map_label .message").html('<span>Downloading Ancestors</span>');

        params[0] = this.qryStrUtils.getParameterByName('id', ''); //'4d800222-65fc-42f0-b568-972cde7ce38f';

        this.ancUtils.twaGetJSON('/Trees/GetAncTreeDiag', params, $.proxy(this.processData, this));

        //var gLoop = 
        setTimeout($.proxy(this.GameLoop, this), 1000 / 50);
    },

    processData: function (data) {
        var _zoomLevel = this.qryStrUtils.getParameterByName('zoom', '');
        this.ancTree = new AncTree();
        this.ancTree.SetInitialValues(Number(_zoomLevel), 10.0, 125.0, 100.0, 90.0, 100.0, 5.0, 40.0, 20.0, screen.width, screen.height);

        var _personId = this.qryStrUtils.getParameterByName('id', '');

        var _xpos = this.qryStrUtils.getParameterByName('xpos', '');
        var _ypos = this.qryStrUtils.getParameterByName('ypos', '');

        this.ancTree.generations = data.Generations;
        this.ancTree.familiesPerGeneration = data.FamiliesPerGeneration;
        this.ancTree.familySpanLines = data.FamilySpanLines;
        this.ancTree.childlessMarriages = data.ChildlessMarriages;
        this.ancTree.RelocatePerson(_personId, _xpos, _ypos);

        if (this.ancTree.generations.length > 0) {
            if (this.ancTree.generations[0].length > 0) {
                $("#map_label .message").html('<span>Ancestors of ' + this.ancTree.generations[0][0].Name + '</span>');
            }
        }
    },

    GameLoop: function () {

        while (this._moustQueue.length > 0) {
            var _point = this._moustQueue.shift();
            this.ancTree.SetCentrePoint(_point[0], _point[1]);
            this.ancTree.DrawTree();
        }

        setTimeout($.proxy(this.GameLoop, this), 1000 / 50);
    }
};











