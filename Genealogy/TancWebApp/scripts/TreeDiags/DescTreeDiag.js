
var JSMaster, AncUtils,QryStrUtils,AncTree,Tree;


//var _tree = null;
 
//var _isMobile = false;
 
 
 
 
$(document).ready(function () {
    var jsMaster = new JSMaster();
    var decTreeDiag = new DecTreeDiag();
    
    jsMaster.connectfacebook(function () {        
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
        run: function() {
            var params = {};
    
            var int;
            var _sourceId = this.qryStrUtils.getParameterByName('sid', '');
            var _personId = this.qryStrUtils.getParameterByName('id', '');
          
 
            $(".button_box").mousedown(function (evt) {
            var _dir = '';

            if (evt.target.id == "up") _dir = 'UP';
            if (evt.target.id == "dn") _dir = 'DOWN';
            if (evt.target.id == "we") _dir = 'WEST';
            if (evt.target.id == "no") _dir = 'NORTH';
            if (evt.target.id == "es") _dir = 'EAST';
            if (evt.target.id == "so") _dir = 'SOUTH';

            if (_tree !== null) {
                int = setInterval(function () { _tree.MoveTree(_dir); }, 100);
            }

            }).mouseup(function () {
                clearInterval(int);
            });
    
            setTimeout(GameLoop, 1000 / 50);
            
            
            
            $("#myCanvas").mousedown(function (evt) {
                if (this.ancTree !== null) {
                    this._mouseDown = true;
                }
            });
            
            
            
            $("#myCanvas").mouseup(function (evt) {
                if (this.ancTree !== null) {
                    this._mouseDown = false;
    
                    var _point = new Array(1000000, 1000000);
                    this._moustQueue[this._moustQueue.length] = _point;
    
                    } 
            });
            
            $("#myCanvas").click(function (evt) {
                if (_tree !== null) {
                    _tree.PerformClick(evt.clientX, evt.clientY);
                    if (_tree.refreshData) {
                        window.location.hash = _tree.qryString;
                    }
                    var _point = new Array(1000000, 1000000);
    
    
                    this._moustQueue[this._moustQueue.length] = _point;
                }
            });
            $("#myCanvas").mousemove(function (evt) {
                if (_tree !== null) {
                    var _point = new Array(evt.clientX, evt.clientY);
                    _tree.SetMouse(_point[0], _point[1]);
                    if (this._mouseDown) {
                        this._moustQueue.push(_point);
                    }
                }
            });
            
             $("#ml .message").html('<span>Downloading Descendant Tree</span>');
   

            if(_sourceId === '' || _sourceId === '00000000-0000-0000-0000-000000000000')
            {
                params[0] = _sourceId;
                params[1] = _personId;
           
                this.ancUtils.twaGetJSON('/Trees/GetTreeDiagPerson', params, $.proxy(this.processData, this));
            }
            else
            {
                params[0] = _sourceId;
                this.ancUtils.twaGetJSON('/Trees/GetTreeDiag', params, $.proxy(this.processData, this));
              
            }


       


        
            
            
            
            
            
            
            
            
            
        },
        processData: function (data) {


            this.ancTree = new Tree();
            var _zoomLevel = this.qryStrUtils.getParameterByName('zoom', '');

            this.ancTree.SetInitialValues(Number(_zoomLevel), 30.0, 120.0, 70.0, 70.0, 100.0, 20.0, 40.0, 20.0, screen.width, screen.height);
            
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
  
            this.ancTree.RelocatePerson(_personId,_xpos,_ypos);

            if (this.ancTree.generations.length > 0) {
                if (this.ancTree.generations[0].length > 0) {
                    $("#map_label .message").html('<span>Descendants of ' + this.ancTree.generations[0][0].Name + '</span>');
                }
            }
            
            
            this.ancTree.refreshData = false;
        },
        GameLoop: function() {

            while (this._moustQueue.length > 0) {
                var _point = this._moustQueue.shift();
        
        
                this.ancTree.SetCentrePoint(_point[0], _point[1]);
                this.ancTree.DrawTree();
            }

            setTimeout($.proxy(this.GameLoop, this));
        }

};



 







 
