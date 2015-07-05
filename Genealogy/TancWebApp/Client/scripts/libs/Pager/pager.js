var Pager = function () {
    
};

Pager.prototype = {



    _addlinks: function (dupeEvents, func, context) {
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

    },

    //ParentElement: $('#pager'),
    //Batch: data.Batch,
    //BatchLength: data.BatchLength,
    //Total: data.Total,
    //Function: this.getLink,
    //Context: this
    createpager: function (pagerparams) {

        var clickEvents = new Array();

        //   dupeEvents.push({ key: '#d' + _idx, value: sourceInfo.XREF });


        var blocksize = 5;

        var remainderPages = pagerparams.Total % pagerparams.BatchLength;
        var totalRequiredPages = (pagerparams.Total - remainderPages) / pagerparams.BatchLength;

        if (remainderPages > 0)
            totalRequiredPages++;

        var pagerBody = '';

        if (totalRequiredPages <= blocksize) {
            var idx0 = 0;

            while (idx0 < totalRequiredPages) {

                pagerBody += "<a id='cp_" + idx0 + "' href='' class = 'pagerlink'>" + String(idx0 + 1) + "</a>";
                clickEvents.push({ key: 'cp_' + idx0, value: idx0 });
                idx0++;
            }
        }
        else {
            var startpage = pagerparams.Batch - (pagerparams.Batch % blocksize);
            var limit = 0;

            if ((startpage + blocksize) > totalRequiredPages) {

                limit = totalRequiredPages;
            }
            else {
                limit = startpage + blocksize;

            }

            //   alert(startpage + ' ' + limit);


            if (startpage >= blocksize) {
                pagerBody += "<a id='cp_0' href='' class = 'pagerlink'>First</a>";
                clickEvents.push({ key: 'cp_0', value: 0 });

                // work out how far back to move the pager when the '..' is clicked.
                // if we are at the end of the record and there is only a few pages available
                // then the .. should be moved back to the start of block of pages boundary 
                // eg 01234 56789 1011121314 the block boundaries would be 0 5 and 10

                var countPagesAvailable = (limit - startpage);

                var linkPage = (startpage - blocksize);

                pagerBody += "<a id='cp_" + linkPage + "' href='' class = 'pagerlink'>..</a>";

                clickEvents.push({ key: 'cp_' + linkPage, value: linkPage });
            }

            var idx = startpage;
            while (idx < limit) {
                if (idx == pagerparams.Batch) {
                    pagerBody += "<a id='cp_" + idx + "' href='' class = 'pagerlink_selected'>" + String(idx + 1) + "</a>";
                    clickEvents.push({ key: 'cp_' + idx, value: idx });
                }
                else {
                    pagerBody += "<a id='cp_" + idx + "' href='' class = 'pagerlink' >" + String(idx + 1) + "</a>";
                    clickEvents.push({ key: 'cp_' + idx, value: idx });
                }
                idx++;
            }


            if (idx < totalRequiredPages) {

                var remainderAvailablePages = totalRequiredPages % blocksize;
                //zero based

                startpage += blocksize;
                startpage++;

                pagerBody += "<a id='cp_" + startpage + "' href='' class = 'pagerlink'>..</a>";
                clickEvents.push({ key: 'cp_' + startpage, value: startpage });

                pagerBody += "<a id='cp_" + (totalRequiredPages - remainderAvailablePages) + "' href='' class = 'pagerlink'>Last</a>";
                clickEvents.push({ key: 'cp_' + (totalRequiredPages - remainderAvailablePages), value: (totalRequiredPages - remainderAvailablePages) });

            }
        }

        // set pager html
        $('#' + pagerparams.ParentElement).html(pagerBody);

        // add click TotalEvents
        this._addlinks(clickEvents, pagerparams.Function, pagerparams.Context);
    }
};