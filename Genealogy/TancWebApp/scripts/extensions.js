


$.fn.pasteEvents = function (delay) {
    if (delay == undefined) delay = 20;
    return $(this).each(function () {
        var $el = $(this);
        $el.on("paste", function () {
            $el.trigger("prepaste");
            setTimeout(function () { $el.trigger("postpaste"); }, delay);
        });
    });
};



//USAGE EXAMPLES 

// using debounce in a constructor or initialization function to debounce
// focus events for a widget (onFocus is the original handler):
//this.debouncedOnFocus = this.onFocus.debounce(500, false);
//this.inputNode.addEventListener('focus', this.debouncedOnFocus, false);

//// to coordinate the debounce of a method for all objects of a certain class, do this:
//MyClass.prototype.someMethod = function () {
//    /* do something here, but only once */
//} .debounce(100, true); // execute at start and use a 100 msec detection period

//// wait until the user is done moving the mouse, then execute
//// (using the stand-alone version)
//document.onmousemove = debounce(function (e) {
//    /* do something here, but only once after mouse cursor stops */
//}, 250, false);
//http://ajaxian.com/archives/debounce-your-javascript-functions
Function.prototype.debounce = function (threshold, execAsap) {
    var func = this, // reference to original function
            timeout; // handle to setTimeout async task (detection period)
    // return the new debounced function which executes the original function only once
    // until the detection period expires
    return function debounced() {
        var obj = this, // reference to original context object
                args = arguments; // arguments at execution time
        // this is the detection function. it will be executed if/when the threshold expires
        function delayed() {
            // if we're executing at the end of the detection period
            if (!execAsap)
                func.apply(obj, args); // execute now
            // clear timeout handle
            timeout = null;
        };
        // stop any current detection period
        if (timeout)
            clearTimeout(timeout);
        // otherwise, if we're not already waiting and we're executing at the beginning of the waiting period
        else if (execAsap)
            func.apply(obj, args); // execute now
        // reset the waiting period
        timeout = setTimeout(delayed, threshold || 100);
    };
}

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




Array.prototype.LinkContainingPoint = function (mx,my) {

    for (var i = 0; i < this.length; i++) {

        if ((this[i].x1 <= mx && this[i].x2 >= mx) 
        && (this[i].y1 <= my && this[i].y2 >= my))                      
        {
            return this[i];
        }
    }



    return null;

};



Array.prototype.ContainsPerson = function (value) {

    for (var i = 0; i < this.length; i++) {

        if (this[i].PersonId == value.PersonId) {
            return true;
        }
    }



    return false;

};


Array.prototype.SortByGenIdx = function()
{
    for(var i=0;i<this.length;i++)
	{
		for(var j=i+1;j<this.length;j++)
		{
			if(Number(this[i].GenerationIdx) < Number(this[j].GenerationIdx))
			{
				var tempValue = this[j];
				this[j] = this[i];
				this[i] = tempValue;
			}
		}
	}
};
