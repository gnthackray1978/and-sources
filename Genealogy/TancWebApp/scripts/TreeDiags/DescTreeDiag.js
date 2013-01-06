
var JSMaster, AncUtils,QryStrUtils,AncTree,Tree;


//var _tree = null;
 
//var _isMobile = false;




$(document).ready(function () {
    var jsMaster = new JSMaster();


    jsMaster.connectfacebook(function () {
        var decTreeDiag = new DecTreeDiag();

       

        decTreeDiag.run();


    });


});


 


var DecTreeDiag = function () {
    this.ancTree = null;
    this.ancUtils = new AncUtils();
    this.qryStrUtils = new QryStrUtils();
    this._moustQueue = [];
    this._mouseDown = false;

};

DecTreeDiag.prototype = {
    run: function () {
        var params = {};

        var int;
        var _sourceId = this.qryStrUtils.getParameterByName('sid', '');
        var _personId = this.qryStrUtils.getParameterByName('id', '');
        var that = this;


        $(".button_box").mousedown(function (evt) {
            var _dir = '';

            if (evt.target.id == "up") _dir = 'UP';
            if (evt.target.id == "dn") _dir = 'DOWN';
            if (evt.target.id == "we") _dir = 'WEST';
            if (evt.target.id == "no") _dir = 'NORTH';
            if (evt.target.id == "es") _dir = 'EAST';
            if (evt.target.id == "so") _dir = 'SOUTH';
            if (evt.target.id == "de") _dir = 'DEBUG';

            if (that.ancTree !== null) {

                int = setInterval(function () { that.ancTree.MoveTree(_dir); }, 100);

                that.ancTree.Debug();
            }

        }).mouseup(function () {
            clearInterval(int);
        });


        setTimeout($.proxy(this.GameLoop, this), 1000 / 50);



        $("#myCanvas").mousedown(function (evt) {
            if (that.ancTree !== null) {
                evt.originalEvent.preventDefault();
                that._mouseDown = true;
            }
        });



        $("#myCanvas").mouseup(function (evt) {
            if (that.ancTree !== null) {
                that._mouseDown = false;

                var _point = new Array(1000000, 1000000);
                that._moustQueue[that._moustQueue.length] = _point;

            }
        });

        $("#myCanvas").click(function (evt) {
            if (that.ancTree !== null) {
                that.ancTree.PerformClick(evt.clientX, evt.clientY);
                if (that.ancTree.refreshData) {
             
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
        $("#myCanvas").mousemove(function (evt) {
            if (that.ancTree !== null) {

                var _point = new Array(evt.clientX, evt.clientY);
                that.ancTree.SetMouse(_point[0], _point[1]);
                if (that._mouseDown) {
                    that._moustQueue.push(_point);
                }
            }
        });

        $("#ml .message").html('<span>Downloading Descendant Tree</span>');


        if (_sourceId === '' || _sourceId === '00000000-0000-0000-0000-000000000000') {
            params[0] = _sourceId;
            params[1] = _personId;

            this.ancUtils.twaGetJSON('/Trees/GetTreeDiagPerson', params, $.proxy(this.processData, this));
        }
        else {
            params[0] = _sourceId;
            this.ancUtils.twaGetJSON('/Trees/GetTreeDiag', params, $.proxy(this.processData, this));

        }

    },
    processData: function (data) {


        this.ancTree = new Tree();
        var _zoomLevel = this.qryStrUtils.getParameterByName('zoom', '');

        this.ancTree.SetInitialValues(Number(_zoomLevel), 30.0, 170.0, 70.0, 70.0, 100.0, 20.0, 40.0, 20.0, screen.width, screen.height);

        //    var _personId = '913501a6-1216-4764-be8c-ae11fd3a0a8b';
        //    var _zoomLevel = 100;
        //    var _xpos = 750.0;
        //    var _ypos = 100.0;

        this.ancTree.generations = data.Generations;

        this.ancTree.familiesPerGeneration = data.FamiliesPerGeneration;
        this.ancTree.familySpanLines = data.FamilySpanLines;
        this.ancTree.childlessMarriages = data.ChildlessMarriages;
        this.ancTree.sourceId = this.qryStrUtils.getParameterByName('sid', '');

        var _personId = this.qryStrUtils.getParameterByName('id', '');

        var _xpos = this.qryStrUtils.getParameterByName('xpos', '');
        var _ypos = this.qryStrUtils.getParameterByName('ypos', '');

        this.ancTree.RelocatePerson(_personId, _xpos, _ypos);

        if (this.ancTree.generations.length > 0) {
            if (this.ancTree.generations[0].length > 0) {
                $("#map_label .message").html('<span>Descendants of ' + this.ancTree.generations[0][0].Name + '</span>');
            }
        }


        this.ancTree.refreshData = false;
    },
    GameLoop: function () {

        while (this._moustQueue.length > 0) {
            var _point = this._moustQueue.shift();


            this.ancTree.SetCentrePoint(_point[0], _point[1]);
            this.ancTree.DrawTree();
        }

        setTimeout($.proxy(this.GameLoop, this));
    }

};



 







 
