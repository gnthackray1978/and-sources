



$(document).ready(function () {


    var jsMaster = new JSMaster();

    jsMaster.generateHeader('#1', function () {

        //        var params = {};

        //        params[0] = 'hello';

        //        twaGetJSON("/TestLogin", params, function (data) { });

        return false;
    });

    // createHeader('#1', getPerson);



    var j = new JSTest();


});

var JSTest = function () {
    var monkey = 'hello';
   // var hello = $.proxy(this.test, this);

   // var debounced = jQuery.debounce(250, false, this.test);

    $(window).resize(__debounce(this.test,250));
}



JSTest.prototype.test = function () {
    alert(this.monkey);
};

//JSTest.prototype.helloworld = function () {
//    
//}





//getPerson = function () {

//    var params = {};

//    params[0] = 'hello';

//    twaGetJSON("/TestLogin", params, function (data) { });

//    return false;
//};







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



// using debounce in a constructor or initialization function to debounce
// focus events for a widget (onFocus is the original handler):
this.debouncedOnFocus = this.onFocus.debounce(500, false);
this.inputNode.addEventListener('focus', this.debouncedOnFocus, false);

// to coordinate the debounce of a method for all objects of a certain class, do this:
MyClass.prototype.someMethod = function () {
    /* do something here, but only once */
} .debounce(100, true); // execute at start and use a 100 msec detection period

// wait until the user is done moving the mouse, then execute
// (using the stand-alone version)
document.onmousemove = debounce(function (e) {
    /* do something here, but only once after mouse cursor stops */
}, 250, false);
