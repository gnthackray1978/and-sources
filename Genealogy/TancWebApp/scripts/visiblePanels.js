


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

    answerPanel: function (panel) {
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
    },

    masterShowTab: function (panel) {
        if (panel == 1) {
            $("#panelA").removeClass("hidePanel").addClass("displayPanel");
            $("#panelB").removeClass("displayPanel").addClass("hidePanel");
            $("#panelC").removeClass("displayPanel").addClass("hidePanel");
            $("#panelD").removeClass("displayPanel").addClass("hidePanel");
        }

        if (panel == 2) {
            $("#panelA").removeClass("displayPanel").addClass("hidePanel");
            $("#panelB").removeClass("hidePanel").addClass("displayPanel");
            $("#panelC").removeClass("displayPanel").addClass("hidePanel");
            $("#panelD").removeClass("displayPanel").addClass("hidePanel");
        }

        if (panel == 3) {

            $("#panelA").removeClass("displayPanel").addClass("hidePanel");
            $("#panelB").removeClass("displayPanel").addClass("hidePanel");
            $("#panelC").removeClass("hidePanel").addClass("displayPanel");
            $("#panelD").removeClass("displayPanel").addClass("hidePanel");
        }

        if (panel == 4) {

            $("#panelA").removeClass("displayPanel").addClass("hidePanel");
            $("#panelB").removeClass("displayPanel").addClass("hidePanel");
            $("#panelC").removeClass("displayPanel").addClass("hidePanel");
            $("#panelD").removeClass("hidePanel").addClass("displayPanel");
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