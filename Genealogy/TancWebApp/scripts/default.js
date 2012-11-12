
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

    this.ancUtils = new AncUtils();
}

JSTest.prototype.logmessage = function (somecrap) {

    var params = {};
    params[0] = '';
    params[1] = '';
    params[2] = 'Thac';
    params[3] = '';
    params[4] = '';
    params[5] = '';
    params[6] = '1700';
    params[7] = '1720';
    params[8] = '';
    params[9] = '';
    params[10] = '0';
    params[11] = '30';
    params[12] =  'MarriageDate';

    this.ancUtils.twaGetJSON('/Marriages/GetMarriages/Select', params, this.marriageResult);



}

JSTest.prototype.marriageResult=function (data) {
            var test = 'hello there';
        alert(test);
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





