
// Renderer handles the layout rendering loop
function Renderer(interval,layout, clear, drawEdge, drawNode) {
    this.interval = interval;
    this.layout = layout;
    this.clear = clear;

    this.drawEdge = drawEdge;

    this.drawNode = drawNode;




    this.map = layout.mapHandler;

    this.layout.graph.addGraphListener(this);
}


Renderer.prototype.graphChanged = function (e) {

    this.start();
};

Renderer.prototype.start = function () {
    var t = this;

    this.layout.start(1, function render() {
        t.clear();

     
        t.layout.eachEdge(function (edge, spring) {
            t.drawEdge(edge, spring.point1.p, spring.point2.p);
        });

        t.layout.eachNode(function (node, point) {
            t.drawNode(node, point.p);
        });

    });
};

