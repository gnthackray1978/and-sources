
// Layout.ForceDirected.Spring.prototype.distanceToPoint = function(point)
// {
// 	// hardcore vector arithmetic.. ohh yeah!
// 	// .. see http://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment/865080#865080
// 	var n = this.point2.p.subtract(this.point1.p).normalise().normal();
// 	var ac = point.p.subtract(this.point1.p);
// 	return Math.abs(ac.x * n.x + ac.y * n.y);
// };

// -----------
var Layout = {};

Layout.ForceDirected = function (graph, mapHandler, stiffness, repulsion, damping, parentNode, parentLayout, firstNode) {
    this.selected = null;
    this.nearest = null;
    this.dragged = null;
    this.parentNode = parentNode;
    this.parentLayout = parentLayout;

    this.firstNode = firstNode;
    this.mapHandler = mapHandler;

    this.canvasId = '#springydemo';
    this.mouseup = true;

    this.graph = graph;
    this.stiffness = stiffness; // spring stiffness constant
    this.repulsion = repulsion; // repulsion constant
    this.damping = damping; // velocity damping factor

    this.nodePoints = {}; // keep track of points associated with nodes
    this.edgeSprings = {}; // keep track of springs associated with edges

    this.mapHandler.layout = this; // oh dear george! 
    this.mapHandler.currentBB = this.getBoundingBox(); // fix this!!!
};

Layout.ForceDirected.prototype = {
    point: function (node) {
        if (typeof (this.nodePoints[node.id]) === 'undefined') {
            var mass = typeof (node.data.mass) !== 'undefined' ? node.data.mass : 1.0;
            this.nodePoints[node.id] = new Layout.ForceDirected.Point(Vector.random(), mass);
        }

        return this.nodePoints[node.id];
    },

    spring: function (edge) {
        if (typeof (this.edgeSprings[edge.id]) === 'undefined') {
            var length = typeof (edge.data.length) !== 'undefined' ? edge.data.length : 1.0;

            var existingSpring = false;

            var from = this.graph.getEdges(edge.source, edge.target);
            from.forEach(function (e) {
                if (existingSpring === false && typeof (this.edgeSprings[e.id]) !== 'undefined') {
                    existingSpring = this.edgeSprings[e.id];
                }
            }, this);

            if (existingSpring !== false) {
                return new Layout.ForceDirected.Spring(existingSpring.point1, existingSpring.point2, 0.0, 0.0);
            }

            var to = this.graph.getEdges(edge.target, edge.source);
            from.forEach(function (e) {
                if (existingSpring === false && typeof (this.edgeSprings[e.id]) !== 'undefined') {
                    existingSpring = this.edgeSprings[e.id];
                }
            }, this);

            if (existingSpring !== false) {
                return new Layout.ForceDirected.Spring(existingSpring.point2, existingSpring.point1, 0.0, 0.0);
            }

            this.edgeSprings[edge.id] = new Layout.ForceDirected.Spring(
            this.point(edge.source), this.point(edge.target), length, this.stiffness
            );
        }

        return this.edgeSprings[edge.id];
    },
    // callback should accept two arguments: Node, Point
    eachNode: function (callback) {
        var t = this;
        this.graph.nodes.forEach(function (n) {
            callback.call(t, n, t.point(n));
        });
    },

    // callback should accept two arguments: Edge, Spring
    eachEdge: function (callback) {
        var t = this;
        this.graph.edges.forEach(function (e) {
            callback.call(t, e, t.spring(e));
        });
    },

    // callback should accept one argument: Spring
    eachSpring: function (callback) {
        var t = this;
        this.graph.edges.forEach(function (e) {
            callback.call(t, t.spring(e));
        });
    },

    // Physics stuff
    applyCoulombsLaw: function () {
        this.eachNode(function (n1, point1) {
            this.eachNode(function (n2, point2) {
                if (point1 !== point2) {
                    var d = point1.p.subtract(point2.p);
                    var distance = d.magnitude() + 0.1; // avoid massive forces at small distances (and divide by zero)
                    var direction = d.normalise();

                    // apply force to each end point
                    point1.applyForce(direction.multiply(this.repulsion).divide(distance * distance * 0.5));
                    point2.applyForce(direction.multiply(this.repulsion).divide(distance * distance * -0.5));
                }
            });
        });
    },

    applyHookesLaw: function () {
        this.eachSpring(function (spring) {
            var d = spring.point2.p.subtract(spring.point1.p); // the direction of the spring
            var displacement = spring.length - d.magnitude();
            var direction = d.normalise();

            // apply force to each end point
            spring.point1.applyForce(direction.multiply(spring.k * displacement * -0.5));
            spring.point2.applyForce(direction.multiply(spring.k * displacement * 0.5));
        });
    },

    attractToCentre: function () {
        this.eachNode(function (node, point) {
            var direction = point.p.multiply(-1.0);
            point.applyForce(direction.multiply(this.repulsion / 50.0));
        });
    },
    updateVelocity: function (timestep) {
        this.eachNode(function (node, point) {
            // Is this, along with updatePosition below, the only places that your
            // integration code exist?
            point.v = point.v.add(point.a.multiply(timestep)).multiply(this.damping);
            point.a = new Vector(0, 0);
        });
    },

    updatePosition: function (timestep) {
        this.eachNode(function (node, point) {
            // Same question as above; along with updateVelocity, is this all of
            // your integration code?
            point.p = point.p.add(point.v.multiply(timestep));
        });
    },

    // Calculate the total kinetic energy of the system
    totalEnergy: function (timestep) {
        var energy = 0.0;
        this.eachNode(function (node, point) {
            var speed = point.v.magnitude();
            energy += 0.5 * point.m * speed * speed;
        });

        return energy;
    },

    mouseDown: function (e) {

        this.mouseup = false;

        var pos = $(this.canvasId).offset();

        var p = this.mapHandler.currentPositionFromScreen(pos, e);    // fromScreen({ x: (e.pageX - centrePoint) - pos.left, y: (e.pageY - centreVerticalPoint) - pos.top });


        this.selected = this.nearest = this.dragged = this.nearestPoint(p);

        if (this.selected.node !== null) {
            this.dragged.point.m = 10000.0;
        }

    },

    mouseUp: function () {
        this.mapHandler.addToMouseQueue(1000000, 1000000);
        this.dragged = null;
        this.mouseup = true;
    },

    mouseMove: function (e) {

        var pos = $(this.canvasId).offset();
        var p = this.mapHandler.currentPositionFromScreen(pos, e);

        if (!this.mouseup && this.selected.node === null) {
            this.mapHandler.addToMouseQueue(e.clientX, e.clientY);
        }

        this.nearest = this.nearestPoint(p);

        if (this.dragged !== null && this.dragged.node !== null) {
            this.dragged.point.p.x = p.x;
            this.dragged.point.p.y = p.y;
        }
    },

    
      getSelection: function (node) {
            // 1 nothing 
            // 2 nearest 
            // 3 selected 
            var selectedPersonId = '';
            var nodePersonId = '';

            if (this.selected != null && this.selected.node != undefined && this.selected.node.data != undefined && this.selected.node.data.person  !=undefined) {
                selectedPersonId = this.selected.node.data.person.PersonId;
            }

            if (node.data != undefined && node.data.person != undefined) {
                nodePersonId = node.data.person.PersonId;
            }

            if (selectedPersonId == nodePersonId && node.data.type != 'infonode') {
                return 3;
            }
            else if (this.nearest !== null && this.nearest.node !== null && this.nearest.node.id === node.id) {
                return 2;
            } else {
                return 1;
            }
        }

};


var __bind = function (fn, me) { return function () { return fn.apply(me, arguments); }; }; // stolen from coffeescript, thanks jashkenas! ;-)

Layout.requestAnimationFrame = __bind(window.requestAnimationFrame ||
	window.webkitRequestAnimationFrame ||
	window.mozRequestAnimationFrame ||
	window.oRequestAnimationFrame ||
	window.msRequestAnimationFrame ||
	function (callback, element) {
	    window.setTimeout(callback, 10);
	}, window);


// start simulation
Layout.ForceDirected.prototype.start = function (interval, render, done) {
    var t = this;

    if (this._started) return;
    this._started = true;

    Layout.requestAnimationFrame(function step() {
        t.applyCoulombsLaw();
        t.applyHookesLaw();
        t.attractToCentre();
        t.updateVelocity(0.03);
        t.updatePosition(0.03);

        if (typeof (render) !== 'undefined')
            render();

        // stop simulation when energy of the system goes below a threshold
        if (t.totalEnergy() < 0.01) {
            t._started = false;
            if (typeof (done) !== 'undefined') { done(); }
        } else {

            Layout.requestAnimationFrame(step);

        }
    });
};

// Find the nearest point to a particular position
Layout.ForceDirected.prototype.nearestPoint = function (pos) {

    var min = { node: null, point: null, distance: 1 };
    var t = this;
    this.graph.nodes.forEach(function (n) {
        if (n.data.type == 'normal') {
            var point = t.point(n);
            var distance = point.p.subtract(pos).magnitude();

            if (min.distance === null || distance < min.distance) {
                min = { node: n, point: point, distance: distance };
            }
        }
    });



    return min;
};

// returns [bottomleft, topright]
// get boundaries of graph by looping through all the 
// nodes
// adds a 5% padding around edge

Layout.ForceDirected.prototype.getBoundingBox = function () {
    var bottomleft = new Vector(-2, -2);
    var topright = new Vector(2, 2);

    this.eachNode(function (n, point) {
        if (point.p.x < bottomleft.x) {
            bottomleft.x = point.p.x;
        }
        if (point.p.y < bottomleft.y) {
            bottomleft.y = point.p.y;
        }
        if (point.p.x > topright.x) {
            topright.x = point.p.x;
        }
        if (point.p.y > topright.y) {
            topright.y = point.p.y;
        }
    });

    var padding = topright.subtract(bottomleft).multiply(0.07); // ~5% padding

    return { bottomleft: bottomleft.subtract(padding), topright: topright.add(padding) };
};




// Point
Layout.ForceDirected.Point = function (position, mass) {
    this.p = position; // position
    this.m = mass; // mass
    this.v = new Vector(0, 0); // velocity
    this.a = new Vector(0, 0); // acceleration
};

Layout.ForceDirected.Point.prototype.applyForce = function (force) {
    this.a = this.a.add(force.divide(this.m));
};

// Spring
Layout.ForceDirected.Spring = function (point1, point2, length, k) {
    this.point1 = point1;
    this.point2 = point2;
    this.length = length; // spring length at rest
    this.k = k; // spring constant (See Hooke's law) .. how stiff the spring is
};


