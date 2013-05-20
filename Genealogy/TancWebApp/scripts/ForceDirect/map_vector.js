﻿


// Vector
Vector = function (x, y) {
    this.x = x;
    this.y = y;
};

Vector.random = function () {
    return new Vector(10.0 * (Math.random() - 0.5), 10.0 * (Math.random() - 0.5));
};

Vector.prototype.add = function (v2) {
    return new Vector(this.x + v2.x, this.y + v2.y);
};

Vector.prototype.subtract = function (v2) {
    return new Vector(this.x - v2.x, this.y - v2.y);
};

Vector.prototype.multiply = function (n) {
    return new Vector(this.x * n, this.y * n);
};

Vector.prototype.divide = function (n) {
    return new Vector((this.x / n) || 0, (this.y / n) || 0); // Avoid divide by zero errors..
};

Vector.prototype.magnitude = function () {
    return Math.sqrt(this.x * this.x + this.y * this.y);
};

Vector.prototype.normal = function () {
    return new Vector(-this.y, this.x);
};

Vector.prototype.normalise = function () {
    return this.divide(this.magnitude());
};

Vector.prototype.distance = function (d2) {

    var x = d2.x - this.x;
    x = x * x;

    var y = d2.y - this.y;
    y = y * y;

    return Math.sqrt(x + y);
};



