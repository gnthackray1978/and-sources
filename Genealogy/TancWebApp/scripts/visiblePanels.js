


var Panels = function () { }


Panels.prototype = {

    marriagesShowPanel: function(panel) {
        if (panel == 1) {
            $("#pMain").removeClass("hidePanel").addClass("displayPanel");
            $("#pMore").removeClass("displayPanel").addClass("hidePanel");
        }
        if (panel == 2) {
            $("#pMain").removeClass("displayPanel").addClass("hidePanel");
            $("#pMore").removeClass("hidePanel").addClass("displayPanel");
        }
    }, 

    personsShowPanel: function (panel) {
        if (panel == 1) {
            $("#pMain").removeClass("hidePanel").addClass("displayPanel");
            $("#pMore").removeClass("displayPanel").addClass("hidePanel");
        }
        if (panel == 2) {
            $("#pMain").removeClass("displayPanel").addClass("hidePanel");
            $("#pMore").removeClass("hidePanel").addClass("displayPanel");
        }
    },
   
    sourcesShowPanel: function(panel) {
        if (panel == 1) {
            $("#pMain").removeClass("hidePanel").addClass("displayPanel");
            $("#pMore").removeClass("displayPanel").addClass("hidePanel");
            $("#pAdd").removeClass("displayPanel").addClass("hidePanel");

        }

        if (panel == 2) {


            $("#pMain").removeClass("displayPanel").addClass("hidePanel");
            $("#pMore").removeClass("hidePanel").addClass("displayPanel");
            $("#pAdd").removeClass("displayPanel").addClass("hidePanel");

        }

        if (panel == 3) {

            $("#pMain").removeClass("displayPanel").addClass("hidePanel");
            $("#pMore").removeClass("displayPanel").addClass("hidePanel");
            $("#pAdd").removeClass("hidePanel").addClass("displayPanel");
        }
    }

}

















function marriagesShowPanel(panel) {
    if (panel == 1) {
        $("#pMain").removeClass("hidePanel").addClass("displayPanel");
        $("#pMore").removeClass("displayPanel").addClass("hidePanel");
    }
    if (panel == 2) {
        $("#pMain").removeClass("displayPanel").addClass("hidePanel");
        $("#pMore").removeClass("hidePanel").addClass("displayPanel");
    }
}


function personsShowPanel(panel) {
    if (panel == 1) {
        $("#pMain").removeClass("hidePanel").addClass("displayPanel");
        $("#pMore").removeClass("displayPanel").addClass("hidePanel");
    }
    if (panel == 2) {
        $("#pMain").removeClass("displayPanel").addClass("hidePanel");
        $("#pMore").removeClass("hidePanel").addClass("displayPanel");
    }
}


function sourcesShowPanel(panel) {



    if (panel == 1) {
        $("#pMain").removeClass("hidePanel").addClass("displayPanel");
        $("#pMore").removeClass("displayPanel").addClass("hidePanel");
        $("#pAdd").removeClass("displayPanel").addClass("hidePanel");

    }

    if (panel == 2) {


        $("#pMain").removeClass("displayPanel").addClass("hidePanel");
        $("#pMore").removeClass("hidePanel").addClass("displayPanel");
        $("#pAdd").removeClass("displayPanel").addClass("hidePanel");

    }

    if (panel == 3) {

        $("#pMain").removeClass("displayPanel").addClass("hidePanel");
        $("#pMore").removeClass("displayPanel").addClass("hidePanel");
        $("#pAdd").removeClass("hidePanel").addClass("displayPanel");
    }
}