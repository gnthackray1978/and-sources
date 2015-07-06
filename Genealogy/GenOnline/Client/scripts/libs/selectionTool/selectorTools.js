var SelectorTools = function() {
    
}

SelectorTools.prototype = {
    handleSelection: function (evt, selection, bodytag, id) {


        if (evt != undefined) {
            var arIdx = jQuery.inArray(evt, selection);

            if (arIdx == -1) {
                selection.push(evt);
            }
            else {
                selection.splice(arIdx, 1);
            }
        }


        $(bodytag).each(function () {
            $this = $(this);

            var quantity = $this.find(id).val();
            arIdx = jQuery.inArray(quantity, selection);

            if (arIdx == -1) {
                $this.removeClass('highLightRow');
            }
            else {
                $this.addClass('highLightRow');
            }
        }); //end each



        return selection;
    },

    addlinks: function (dupeEvents, func, context) {
        for (var i = 0; i < dupeEvents.length; i++) {

            $('body').off("click", "#" + dupeEvents[i].key);


            //     $("#" + dupeEvents[i].key).unbind("click");


            //console.log('creating event for : ' + dupeEvents[i].key);

            var somecrap = function (idx, val) {
                //probably not efficient to do this multiple times
                //this can be a future optimization.


                $('body').on("click", "#" + dupeEvents[idx].key, $.proxy(function () {
                    var va = val;

                    //console.log('clicked with : ' + va);

                    if (va !== null)
                        func.call(context, va);
                    else
                        func.call(context);

                    return false;
                }, context));

            };

            somecrap(i, dupeEvents[i].value);

        }

    }
}