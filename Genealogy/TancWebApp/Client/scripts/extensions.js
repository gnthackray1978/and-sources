Array.prototype.ContainsRec = function (_rec) {

    for (var i = 0; i < this.length; i++) {

        if (this[i].latx == _rec.latx &&
                 this[i].laty == _rec.laty &&
                 this[i].boxlen == _rec.boxlen) {
            return true;
        }
    }
    return false;

}



$.fn.pasteEvents = function (delay) {
    if (delay == undefined) delay = 1000;
    return $(this).each(function () {
        var $el = $(this);
        $el.on("paste", function () {
            $el.trigger("prepaste");
            setTimeout(function () { $el.trigger("postpaste"); }, delay);
        });
    });
};





//remove invalid selections from an array
Array.prototype.RemoveInvalid = function (selection) {
    var filteredArray = new Array();
    for (var si = 0; si < selection.length; si++) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == selection[si]) {
                filteredArray.push(this[i]);
                break;
            }
        }
    }
    return filteredArray;
}



if ($.fn.dataTableExt)
    $.fn.dataTableExt.oApi.fnFindRow = function (oSettings, sSearch, iColumn) {
        var i, iLen;

        var retVale = { row: undefined, idx:-1 };

        for (i = 0, iLen = oSettings.aoData.length ; i < iLen ; i++) {

            var result = $(oSettings.aoData[i].nTr).attr(iColumn);

            if (result != undefined && result == sSearch)
                return { row: $(oSettings.aoData[i].nTr), idx: i };

        }

        return retVale;
    };