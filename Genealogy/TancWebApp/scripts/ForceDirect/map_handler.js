

var mapHandler = function (colourScheme, startwidth, startheight) {

    this.layout = null;
    //this.graph = graph;

    this.currentBB = null; // layout.getBoundingBox();
   // this.targetBB = null;
    this.targetBB = { bottomleft: new Vector(-2, -2), topright: new Vector(2, 2) };

    this.colourScheme = colourScheme;


    // graph size 
    this.original_graph_width = startwidth;
    this.original_graph_height = startheight;

    // graph size 
    this.graph_width = this.original_graph_width;
    this.graph_height = this.original_graph_height;

    //display size
    this.display_width = window.innerWidth + 500;
    this.display_height = window.innerHeight + 500;

    //save screen width/height
    this.screenHeight = screen.height;
    this.screenWidth = screen.width;

    //positional controls
    this.centrePoint = 0;
    this.centreVerticalPoint = 0;
    this.zoomOffset = 0;

    this.centrePointXOffset = 0.0;
    this.centrePointYOffset = 0.0;

    this.mouse_x = 0;
    this.mouse_y = 0;

    // queue of points to move graph to 
    this.mouseQueue = [];

    this.mouseXPercLocat = 0.0;
    this.mouseYPercLocat = 0.0;

    this.percX1 = 0.0;
    this.percY1 = 0.0;

    // zoom level
    this.zoompercentage = 0;

    //info tracker 
    this.infoDisplayed = new Array();
};


mapHandler.prototype = {

    SetCentrePoint: function (param_x, param_y) {
        if (param_x == 1000000 && param_y == 1000000) {
            this.centrePointXOffset = 0;
            this.centrePointYOffset = 0;
        }
        else {
            if (this.centrePointXOffset === 0) {
                this.centrePointXOffset = this.centrePoint - param_x;
            }
            else {

                this.centrePoint = param_x + this.centrePointXOffset;
            }
            if (this.centrePointYOffset === 0) {
                this.centrePointYOffset = this.centreVerticalPoint - param_y;
            }
            else {

                this.centreVerticalPoint = param_y + this.centrePointYOffset;
            }
        }
    },
    SetZoomStart: function () {
        this.GetPercDistances();
        this.mouseXPercLocat = this.percX1;
        this.mouseYPercLocat = this.percY1;
    },
    GetPercDistances: function () {


        var _distanceFromX1 = 0.0;
        var _distanceFromY1 = 0.0;
        var _onePercentDistance = 0.0;

        this.percX1 = 0.0;
        this.percY1 = 0.0;



        //   this.drawingWidth = this.drawingX2 - this.drawingX1;
        //  this.drawingHeight = this.drawingY2 - this.drawingY1;



        if (this.graph_width !== 0 && this.graph_height !== 0) {
            if (this.centrePoint > 0) {
                _distanceFromX1 = this.mouse_x - this.centrePoint; //;
            }
            else {
                _distanceFromX1 = Math.abs(this.centrePoint) + this.mouse_x;
            }

            _onePercentDistance = this.graph_width / 100;
            this.percX1 = _distanceFromX1 / _onePercentDistance;

            if (this.centreVerticalPoint > 0) {
                _distanceFromY1 = this.mouse_y - this.centreVerticalPoint; // ;                
            }
            else {
                _distanceFromY1 = Math.abs(this.centreVerticalPoint) + this.mouse_y;
            }

            _onePercentDistance = this.graph_height / 100;
            this.percY1 = _distanceFromY1 / _onePercentDistance;

        }


    },
    UpdatePosition: function (_dir) {

        var increment = 2;

        if (_dir == 'SOUTH') {
            this.centreVerticalPoint -= increment;
        }
        if (_dir == 'NORTH') {
            this.centreVerticalPoint += increment;
        }
        if (_dir == 'EAST') {
            this.centrePoint += increment;
        }
        if (_dir == 'WEST') {

            this.centrePoint -= increment;
        }
        if (_dir == 'UP' || _dir == 'DOWN') {

            this.mouse_x = this.screenWidth / 2;
            this.mouse_y = this.screenHeight / 2;

            this.GetPercDistances();

            this.mouseXPercLocat = this.percX1;
            this.mouseYPercLocat = this.percY1;

            // zero the centre point 
            this.SetCentrePoint(1000000, 1000000);

            if (_dir == 'UP') {
                this.graph_width += 50;
                this.graph_height += 50;
            } else {
                this.graph_width -= 50;
                this.graph_height -= 50;
            }

            this.GetPercDistances();


            //console.log('y zoom ' + percY1 + ' ' + mouseYPercLocat);
            this.centreVerticalPoint += (this.graph_height / 100) * (this.percY1 - this.mouseYPercLocat);
            //console.log('x zoom ' + percX1 + ' ' + mouseXPercLocat);

            this.centrePoint += (this.graph_width / 100) * (this.percX1 - this.mouseXPercLocat);
        }

        if (_dir == 'DEBUG') {

            console.log('debug: ' + this.onscreenNodes(25));
        }
        var old_area = this.original_graph_width * this.original_graph_height;
        var new_area = this.graph_width * this.graph_height;
        this.zoompercentage = (new_area - old_area) / old_area * 100;

        $('#map_zoom').html(Math.round(this.zoompercentage));
        $('#map_X').html(Math.round(this.centrePoint));
        $('#map_Y').html(Math.round(this.centreVerticalPoint));
    },

    zoomCurrentBB: function (targetBB, amount) {

        this.currentBB = {
            bottomleft: this.currentBB.bottomleft.add(targetBB.bottomleft.subtract(this.currentBB.bottomleft)
    			.divide(amount)),
            topright: this.currentBB.topright.add(targetBB.topright.subtract(this.currentBB.topright)
				.divide(amount))
        };


    },

    currentPositionFromScreen: function (pos, e) {
        var utils = new Utils(this.currentBB, this.graph_width, this.graph_height);
        var p = utils.fromScreen({ x: (e.pageX - this.centrePoint) - pos.left, y: (e.pageY - this.centreVerticalPoint) - pos.top });
        return p;
    },
    
    currentPositionToScreen: function (pos, e) {
        var utils = new Utils(this.currentBB, this.graph_width, this.graph_height);
        var p = utils.toScreen({ x: (e.pageX - this.centrePoint) - pos.left, y: (e.pageY - this.centreVerticalPoint) - pos.top });
        return p;
    },

    addToMouseQueue: function (x, y) {
        var _point = new Array(x, y);
        this.mouseQueue[this.mouseQueue.length] = _point;
    },

    validToDraw: function (x1, y1, margin) {

        var validDraw = true;

        if (margin == undefined) margin = 500;

        if (x1 > (this.display_width + margin)) validDraw = false;
        if (x1 < (0 - margin)) validDraw = false;

        if (y1 > (this.display_height + margin)) validDraw = false;
        if (y1 < (0 - margin)) validDraw = false;

        return validDraw;
    },
    
    mapOffset: function (v1) {

        v1.x += this.centrePoint;
        v1.y += this.centreVerticalPoint;

        return v1;
    },

    adjustPosition: function (_dir) {

        if (this.layout.parentNode == undefined) {
            this.targetBB = this.layout.getBoundingBox();



            // current gets 20% closer to target every iteration
            this.zoomCurrentBB(this.targetBB, 10);

            while (this.mouseQueue.length > 0) {
                var _point = this.mouseQueue.shift();
                this.SetCentrePoint(_point[0], _point[1]);
            }

            this.UpdatePosition(_dir);
        }
        else {
            if (this.layout.parentNode && this.layout.firstNode) {
                var firstNodePoint = this.layout.nodePoints[this.layout.firstNode.id].p;


                var currentUtils = new Utils(this.currentBB, this.graph_width, this.graph_height);

                var screenFirstNode = currentUtils.toScreen(firstNodePoint);

                var parentMapHandler = this.layout.parentLayout.mapHandler;

                var parentUtils = new Utils(parentMapHandler.currentBB, parentMapHandler.graph_width, parentMapHandler.graph_height);


                var parentPoint = this.layout.parentLayout.nodePoints[this.layout.parentNode.id].p;

                var screenParentNode = parentUtils.toScreen(parentPoint);

                // add parentlayout centre points !
                this.centrePoint = parentMapHandler.centrePoint + screenParentNode.x - screenFirstNode.x; // (this.graph_width / 2);

                this.centreVerticalPoint = parentMapHandler.centreVerticalPoint + screenParentNode.y - screenFirstNode.y; // (this.graph_height / 2);
            }
        }

    },

    onscreenNodes: function (maxnumber) {

        var that = this;
        var countonscreen = 0;
        var onscreenNodes = new Array();
        var offscreenNodes = new Array();
        var maxNodes = false;


        //console.log('debug');

        this.layout.eachNode(function (node, point) {

            //console.log(node.data.label);
            var _utils = new Utils(that.currentBB, that.graph_width, that.graph_height);

            var x1 = that.mapOffset(_utils.toScreen(point.p)).x;
            var y1 = that.mapOffset(_utils.toScreen(point.p)).y;

            if (that.validToDraw(x1, y1, 0)) {

                if (countonscreen <= maxnumber)
                    onscreenNodes.push(node);

                if (node.data.type == 'normal')
                    countonscreen++;


            }
            else {

                if (!that.validToDraw(x1, y1, 2000)) {
                    if (node.data.type == 'normal')
                        offscreenNodes.push(node);
                }
            }



            // console.log(node.data.label + ' - ' + x1 + ' ' + y1);
        });

        // if there are more nodes on the screen than the max allowed then we arent interested
        if (countonscreen > maxnumber) onscreenNodes = [];

        return onscreenNodes;
    },

    countOnscreenNodes: function () {

        var that = this;
        var countonscreen = 0;
        var onscreenNodes = new Array();
        var offscreenNodes = new Array();
        var maxNodes = false;

        this.layout.eachNode(function (node, point) {

            //console.log(node.data.label);
            var _utils = new Utils(that.currentBB, that.graph_width, that.graph_height);

            var x1 = that.mapOffset(_utils.toScreen(point.p)).x;
            var y1 = that.mapOffset(_utils.toScreen(point.p)).y;

            if (that.validToDraw(x1, y1, 0)) {
                onscreenNodes.push(node);
                if (node.data.type == 'normal')
                    countonscreen++;
            }
            else {
                if (!that.validToDraw(x1, y1, 2000)) {
                    if (node.data.type == 'normal')
                        offscreenNodes.push(node);
                }
            }

        });

        return countonscreen;
    }

};



