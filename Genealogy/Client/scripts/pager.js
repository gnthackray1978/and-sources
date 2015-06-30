

function createpager(currentPage, recordsPerPage, totalRecords, functionname) {



    // we need page size
    // total num records

    // ok !
    // calc how many pages we need

    //            totalRecords = 103;//21 pages
    //            recordsPerPage = 5;
    //            currentPage = 19;

    // this needs 8 pages of records
    // which is 2 blocks!


    var blocksize = 5;

    var remainderPages = totalRecords % recordsPerPage;
    var totalRequiredPages = (totalRecords - remainderPages) / recordsPerPage;

    if (remainderPages > 0)
        totalRequiredPages++;


    // number of blocks of pages!
    //  5 -((8 % 5)) = 2
    // 2 + 8 /
  //  var //reqnumberblocks = ((recordsPerPage - (totalRequiredPages % recordsPerPage)) + totalRequiredPages) / recordsPerPage;
    var pagerBody = '';

    if (totalRequiredPages <= blocksize) {
        var idx0 = 0;

        while (idx0 < totalRequiredPages) {
            pagerBody += "<a href='' onClick ='" + functionname + "(" + idx0 + ");return false' class = 'pagerlink'>" + String(idx0 + 1) + "</a>";
            idx0++;
        }
    }
    else {




        var startpage = currentPage - (currentPage % blocksize);

        var limit = 0;

        //  alert(totalRequiredPages);

        //5 + 5 > 8 
        if ((startpage + blocksize) > totalRequiredPages) {

            limit = totalRequiredPages;
        }
        else {
            limit = startpage + blocksize;

        }

        //   alert(startpage + ' ' + limit);


        if (startpage >= blocksize) {
            pagerBody += "<a href='' onClick ='" + functionname + "(" + 0 + ");return false' class = 'pagerlink'>First</a>";

            // work out how far back to move the pager when the '..' is clicked.
            // if we are at the end of the record and there is only a few pages available
            // then the .. should be moved back to the start of block of pages boundary 
            // eg 01234 56789 1011121314 the block boundaries would be 0 5 and 10

            var countPagesAvailable = (limit - startpage);
//            if (countPagesAvailable < blocksize) {
//                pagerBody += "<a href='' onClick ='" + functionname + "(" + (startpage - (blocksize+ countPagesAvailable)) + ");return false' class = 'pagerlink'>..</a>";

//            } else {

                pagerBody += "<a href='' onClick ='" + functionname + "(" + (startpage - blocksize) + ");return false' class = 'pagerlink'>..</a>";
           // }

        }

        var idx = startpage;
        while (idx < limit) {
            if (idx == currentPage) {
                pagerBody += "<a href='' onClick ='" + functionname + "(" + idx + ");return false' class = 'pagerlink_selected'>" + String(idx + 1) + "</a>";
            }
            else {
                pagerBody += "<a href='' onClick ='" + functionname + "(" + idx + ");return false' class = 'pagerlink'>" + String(idx + 1) + "</a>";
            }
            idx++;
        }


        if (idx < totalRequiredPages) {

            var remainderAvailablePages = totalRequiredPages % blocksize;
            //zero based

            startpage += blocksize;
            startpage++;

            pagerBody += "<a href='' onClick ='" + functionname + "(" + startpage + ");return false' class = 'pagerlink'>..</a>";

            pagerBody += "<a href='' onClick ='" + functionname + "(" + (totalRequiredPages - remainderAvailablePages) + ");return false' class = 'pagerlink'>Last</a>";
        }



    }
    return pagerBody;
}
