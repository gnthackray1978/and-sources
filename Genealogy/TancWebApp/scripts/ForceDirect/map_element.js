
var Node = function (id, data) {
    this.id = id;
    this.data = typeof (data) !== 'undefined' ? data : {};
};



Node.prototype.getWidth = function (ctx) {
    var text = typeof (this.data.label) !== 'undefined' ? this.data.label : this.id;
    if (this._width && this._width[text])
        return this._width[text];

    ctx.save();
    ctx.font = "16px Verdana, sans-serif";
    var width = ctx.measureText(text).width + 10;
    ctx.restore();

    this._width || (this._width = {});
    this._width[text] = width;

    return width;
};

Node.prototype.getHeight = function (ctx) {
    return 20;
};