

// we need a list of layouts, drawedges, drawnodes
// 1 clear function
//{ layout: layout, edges: drawEdges, nodes: drawNodes  }

var __bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; }; // stolen from coffeescript, thanks jashkenas! ;-)

CombinedRenderer.requestAnimationFrame = __bind(window.requestAnimationFrame ||
	window.webkitRequestAnimationFrame ||
	window.mozRequestAnimationFrame ||
	window.oRequestAnimationFrame ||
	window.msRequestAnimationFrame ||
	function (callback, element) {
	    window.setTimeout(callback, 10);
	}, window);


function CombinedRenderer(clear, layouts) {
  //  this.interval = interval;
    this.layouts = layouts;
    this.clear = clear;

  //  this.map = layout.mapHandler;

  

}

CombinedRenderer.prototype = {
    start: function () {

        if (this._started) return;
        this._started = true;

        var that = this;

        CombinedRenderer.requestAnimationFrame(function step() {

            var idx = 0;

            while (idx < that.layouts.length) {
                if (that.layouts[idx].layout.graph.eventListeners.length == 0)
                    that.layouts[idx].layout.graph.addGraphListener(that);

                idx++;
            }

            var idx = 0;
            var energyCount = 0;

            that.clear(that.layouts[0].layout.mapHandler);

            while (idx < that.layouts.length) {
                that.layouts[idx].layout.applyCoulombsLaw();
                that.layouts[idx].layout.applyHookesLaw();
                that.layouts[idx].layout.attractToCentre();
                that.layouts[idx].layout.updateVelocity(0.03);
                that.layouts[idx].layout.updatePosition(0.03);


                var map = that.layouts[idx].layout.mapHandler;

                // render 
                that.layouts[idx].layout.eachEdge(function (edge, spring) {

                    that.layouts[idx].edges(map, edge, spring.point1.p, spring.point2.p);
                });

                that.layouts[idx].layout.eachNode(function (node, point) {
                    
                    that.layouts[idx].nodes(map, node, point.p);

                });


                if (idx==0 && that.layouts[0].layout.nearest != null && that.layouts[0].layout.nearest.node != null) {
                    var nearestNodePoint = that.layouts[0].layout.nodePoints[that.layouts[0].layout.nearest.node.id];

                    that.layouts[0].nodes(map, that.layouts[0].layout.nearest.node, nearestNodePoint.p);
                }

                energyCount += that.layouts[idx].layout.totalEnergy();

                idx++;
            }

            // stop simulation when energy of the system goes below a threshold
            if (energyCount < 0.01) {
                that._started = false;
                if (typeof (done) !== 'undefined') { done(); }
            } else {

                CombinedRenderer.requestAnimationFrame(step);

            }
        });

    },
    done: function () {

    },
    graphChanged: function (e) {
        this.start();
    }
}
