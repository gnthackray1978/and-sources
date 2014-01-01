
var Edge = function (id, source, target, data) {
    this.id = id;
    this.source = source;
    this.target = target;
    this.data = typeof (data) !== 'undefined' ? data : {};
};

