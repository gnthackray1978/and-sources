
$(document).ready(function () {







    var jsMaster = new JSMaster();

    jsMaster.generateHeader('#1', function () {
        //var test = 'hello there';
        //alert(test);
        return false;
    });


    var jSTest = new JSTest();

    jSTest.adddata();

});


var JSTest = function () {
    this.monkey = 'monkey';


}

JSTest.prototype.logmessage = function (somecrap) {

    //if (console.log != undefined)
    //   console.log();
    var hello = 'called ' + this.monkey + ' sc ' + somecrap;

    var end = 0;
}

JSTest.prototype.adddata = function () {

    var dupeEvents = new Array();
    var _idx = 0;
    var xref = 'ref';

    dupeEvents.push({ key: 'd' + _idx, value: xref });

    var au = new AncUtils();

    au.addlinks(dupeEvents, this.logmessage, this);
}


//JSTest.prototype.addlinks = function (dupeEvents, func, context) {

//    for (var i = 0; i < dupeEvents.length; i++) {
//        var m = i;

//        $("#" + dupeEvents[i].key).live("click",
//            $.proxy(function () {
//                var va = m;

//                func.call(context,va);
//                return false;
//            }, context));
//    }
//    
//}





